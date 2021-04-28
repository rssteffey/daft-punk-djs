using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexFreq : MonoBehaviour
{
    public float squareSize; // Needs to match values on tunnel
    public int columns; // Needs to match values on tunnel

    public LineRenderer[] quads;
    public LineRenderer border;

    public HexTunnel tunnelScript;

    public float noiseSpeed = 2.0f;

    private Vector3 origin;
    private float max_frequency, radius;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = tunnelScript.transform.position;
        origin = this.transform.position;
        instantiateHex();
    }

    // Update is called once per frame
    void Update()
    {
        //for (int h = 0; h < 6; h++) // Each side
        //{
        //    float value = Mathf.PerlinNoise((Time.time * noiseSpeed), h);
        //    setSize(h, value);
        //}
    }


    private void instantiateHex()
    {
        //Calculate values
        radius = squareSize * columns; //Hexagons are very handy in that this also happens to be the edge length
        float edge = radius;
        max_frequency = radius / 2;

        // Same as the tunnel file, but with vertices this time
        // 1) Travel (radius - frequency_max) forward - Point A
        // 2) frequency_max forward to Corner - point B
        // 3) Rotate, travel edge distance forward, point C
        // 4) Rotate, travel frequency_max forward, point D
        // 5) (radius - frequency_max) forward back to origin
        // 6) rotate 60 degrees, repeat for next quad

        // 0) Create our unwitting pawn
        GameObject traverser = new GameObject();
        traverser.transform.position = this.transform.position;
        traverser.transform.rotation = this.transform.rotation;
        traverser.transform.SetParent(this.transform);
        traverser.gameObject.name = "Traverser";

        //init by pointing along path for tunnel
        traverser.transform.Rotate(new Vector3(0, 30, 0));

        for (int h = 0; h < 6; h++)
        {
            LineRenderer lr = quads[h];

            // Point A
            traverser.transform.Translate(new Vector3(radius - max_frequency, 0, 0));
            lr.SetPosition(0, traverser.transform.position);

            // Point B
            traverser.transform.Translate(new Vector3(max_frequency, 0, 0));
            lr.SetPosition(1, traverser.transform.position);

            //Also set border point while we're here
            border.SetPosition(h, traverser.transform.position);

            // Point C
            traverser.transform.Rotate(new Vector3(0, 120, 0));
            traverser.transform.Translate(new Vector3(edge, 0, 0));
            lr.SetPosition(2, traverser.transform.position);

            // Point D
            traverser.transform.Rotate(new Vector3(0, 120, 0));
            traverser.transform.Translate(new Vector3(max_frequency, 0, 0));
            lr.SetPosition(3, traverser.transform.position);

            // Back to origin and rotate
            traverser.transform.Translate(new Vector3(radius - max_frequency, 0, 0));
            traverser.transform.Rotate(new Vector3(0, 60, 0));
        }
    }

    // Change points 0 and 3 to move along the radius with scale change
    public void setSize(int quad, float scale)
    {
        LineRenderer lr = quads[quad];
        Vector3 newHeight = Vector3.Lerp(lr.GetPosition(1), origin, scale * 0.6f);//(max_frequency / radius)) ;
        lr.SetPosition(0, newHeight);
        newHeight = Vector3.Lerp(lr.GetPosition(2), origin, scale * 0.6f);// (max_frequency / radius));
        lr.SetPosition(3, newHeight);
    }
}
