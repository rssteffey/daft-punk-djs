using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTunnel : MonoBehaviour
{
    // Radius to corner of front hexagon
    public float radius;

    // Grid size per tunnel side
    public int columns = 4;
    public int rows = 15;


    private Vector3 locPos;
    private Quaternion locRot;

    // Start is called before the first frame update
    void Start()
    {
        instantiateTunnel();
        locPos = this.transform.localPosition;
        locRot = this.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void instantiateTunnel()
    {
        instantiateSide();
    }

    private void instantiateSide()
    {
        GameObject plane;
        //
        plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localPosition = locPos;
        plane.transform.localRotation = locRot;
        Debug.Log("Instantiate it!");
        
    }
}
