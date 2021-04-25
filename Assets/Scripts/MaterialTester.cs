using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTester : MonoBehaviour
{
    public Material dotMatrix;

    private Vector4 lines,dots;

    // Start is called before the first frame update
    void Start()
    {
        lines = dotMatrix.GetVector("LineTiling");
        dots = dotMatrix.GetVector("DotTiling");
    }

    // Update is called once per frame
    void Update()
    {
        //dotMatrix.SetFloat("", newY);
        dotMatrix.SetVector("", new Vector2());

        // Lines Arrow Keys
        if (Input.GetKeyDown("down"))
        {
            changeVector("lines", "y", -0.5f);
        }
        if (Input.GetKeyDown("up"))
        {
            changeVector("lines", "y", 0.5f);
        }
        if (Input.GetKeyDown("right"))
        {
            changeVector("lines", "x", 0.5f);
        }
        if (Input.GetKeyDown("left"))
        {
            changeVector("lines", "x", -0.5f);
        }

        // Dots WASD
        if (Input.GetKeyDown("s"))
        {
            changeVector("dots", "y", -0.5f);
        }
        if (Input.GetKeyDown("w"))
        {
            changeVector("dots", "y", 0.5f);
        }
        if (Input.GetKeyDown("d"))
        {
            changeVector("dots", "x", 0.5f);
        }
        if (Input.GetKeyDown("a"))
        {
            changeVector("dots", "x", -0.5f);
        }
    }

    public void changeVector(string vectorName, string axis, float amount)
    {
        Vector4 vector;

        switch (vectorName)
        {
            case "lines":
                vector = lines;
                break;
            case "dots":
                vector = dots;
                break;
            default:
                vector = lines;
                break;
        }

        switch (axis)
        {
            case "y":
                vector.y += amount;
                break;
            case "x":
                vector.x += amount;
                break;
        }

        switch (vectorName)
        {
            case "lines":
                dotMatrix.SetVector("LineTiling", vector);
                Debug.Log("New Lines: " + vector.x + ", " + vector.y);
                break;
            case "dots":
                dotMatrix.SetVector("DotTiling", vector);
                Debug.Log("New Dots: " + vector.x + ", " + vector.y);
                break;
        }

        lines = vector;
    }
}
