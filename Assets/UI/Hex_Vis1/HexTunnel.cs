using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTunnel : MonoBehaviour
{
    // Radius to corner of front hexagon
    public float radius;

    public Material glowyMaterial;

    // Grid size per tunnel side
    public int columns = 4;
    public int rows = 15;
    public float squareSize = 1.0f;

    public float depthScale = 2.0f;

    public float perlinScale = 1.0f;  //Overall scale (lower value will have more opacity)
    public float perlinSpeedX = 0.0001f;  //Speed to scroll on x axis
    public float perlinSpeedY = 0.0001f;  //Speed to scroll on y axis
    public float perlinRand = 3.0f;   //Additional random factor?
    public float perlinSpread = 1.0f; //How far apart samples should be


    private HexSide[] sides;
    public GameObject uiCam;

    // Start is called before the first frame update
    void Start()
    {
        instantiateTunnel();
        rotateTunnel();
    }

    // Update is called once per frame
    void Update()
    {
        animateNoise();
    }

    private void instantiateTunnel()
    {
        //Calculate values
        float radius = squareSize * columns; //Hexagons are very handy in that this also happens to be the edge length
        float edge = radius;
        float halfEdge = (edge * 0.5f);
        float inCircleRadius = 0.86602540378f * edge; //We can lose precision since this is just to set our start point

        // We could do math.
        // Or we could do it easy.
        // I'm choosing the latter.
        // Origin is the object where this script is attached, so theoretically we can:
        // 1) create a dummy object
        // 2) point that object down and travel forward 1 radius.
        // 3) Instantiate a side
        // 4) Travel a side and Rotate 60 degrees
        // repeat 3&4 6x in total

        // 1) Create our unwitting pawn
        GameObject traverser = new GameObject();
        traverser.transform.position = this.transform.position;
        traverser.transform.rotation = this.transform.rotation;
        traverser.transform.SetParent(this.transform);
        traverser.gameObject.name = "Traverser";

        // 1.5) Translate back half an edge to make the loop portion cleaner
        traverser.transform.Translate(new Vector3(halfEdge * -1, 0, 0));

        // 2) Rotate 90 around Z axis, Travel one inCircleRadius forward on X, rotate parallel to edge
        traverser.transform.Rotate(new Vector3(0, 0, 90));
        traverser.transform.Translate(new Vector3(inCircleRadius, 0, 0));
        traverser.transform.Rotate(new Vector3(0, 0, -90));

        sides = new HexSide[6];
        for(int h = 0; h < 6; h++)
        {
            // 4a) travel half an edge *before* making the side
            traverser.transform.Translate(new Vector3(halfEdge, 0, 0));

            // 3) Rotate to face origin, Instantiate side, Rotate back (local rotations mean we have to FIFO)
            traverser.transform.Rotate(new Vector3(0, -90, 0));
            traverser.transform.Rotate(new Vector3(-90, 0, 0));
            sides[h] = instantiateSide(traverser.transform.localPosition, traverser.transform.localRotation, h+1);
            traverser.transform.Rotate(new Vector3(90, 0, 0));
            traverser.transform.Rotate(new Vector3(0, 90, 0));

            // 4b) travel half an edge, rotate 60 degrees
            traverser.transform.Translate(new Vector3(halfEdge, 0, 0));
            traverser.transform.Rotate(new Vector3(0, 0, -60));
        }
    }

    private void animateNoise()
    {
        float h_offset, value, clampedValue;
        for (int h = 0; h < 6; h++) // Each side
        {
            for (int i = 0; i < columns; i++) // Each column
            {
                for (int j = 0; j < rows; j++) // Each row
                {
                    h_offset = h * perlinSpread * i;
                    // Sample Perlin Noise on grid. (scrolls right)
                    value = perlinScale * Mathf.PerlinNoise(
                        (Time.time * perlinSpeedX) + (i * perlinSpread) + h_offset, 
                        (Time.time * perlinSpeedY) + (j * perlinSpread));
                    value *= perlinRand;
                    value -= (perlinRand / 2.0f);
                    clampedValue = Mathf.Clamp(value, 0.0f, 1.0f);
                    sides[h].updateSquare(i, j, clampedValue);
                }
            }
        }
                
    }

    private void rotateTunnel()
    {
        this.transform.LookAt(uiCam.transform);
    }

    private HexSide instantiateSide(Vector3 loc, Quaternion rot, int sideNum)
    {
        GameObject side = new GameObject("Side " + sideNum);
        side.transform.SetParent(this.gameObject.transform);
        HexSide current = new HexSide(columns, rows, loc, rot, squareSize, glowyMaterial, side, depthScale);
        return current;
    }

    private class HexSide
    {
        public GameObject parentEmpty;
        public GameObject[,] squares;
        public Vector3 location;
        public Quaternion rotation;

        private float squareSize, depthScale;

        private Material glowyMaterial;

        public HexSide(int cols, int rows, Vector3 loc, Quaternion rot, float square, Material glow, GameObject par, float depth)
        {
            parentEmpty = par;
            squareSize = square;
            glowyMaterial = glow;
            depthScale = depth;

            createSquares(cols, rows);
            parentEmpty.transform.localPosition = loc;
            parentEmpty.transform.localRotation = rot;
        }

        private void createSquares(int cols, int rows)
        {
            squares = new GameObject[cols, rows];

            for(int i = 0; i<cols; i++)
            {
                for(int j = 0; j < rows; j++)
                {
                    GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    plane.transform.SetParent(parentEmpty.transform);
                    plane.transform.localScale = new Vector3(
                        plane.transform.localScale.x * depthScale, 
                        plane.transform.localScale.y,
                        plane.transform.localScale.z);
                    plane.transform.localScale *= squareSize;

                    float depthSize = squareSize * depthScale;

                    // Set location of grid square
                    float locOffsetY = squareSize * ((i + 0.5f) - ((cols / 2.0f)));
                    float locOffsetX = -1 * ((j * depthSize) + (depthSize / 2.0f));
                    plane.transform.Translate(new Vector3(locOffsetX, locOffsetY, 0));

                    //Set material and render options
                    plane.GetComponent<MeshRenderer>().material = glowyMaterial;
                    plane.layer = LayerMask.NameToLayer("UI");

                    squares[i, j] = plane;
                }
            }
            
        }

        public void updateSquare(int col, int row, float newScale)
        {
            squares[col, row].transform.localScale = new Vector3(1 * newScale * depthScale, 1 * newScale, 1 * newScale);
        }
    }
}
