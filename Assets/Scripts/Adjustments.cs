using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adjustments : MonoBehaviour
{

    public Material leftPulse;
    public Material gridDots;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("up"))
        {
            leftPulse.SetFloat("OffsetFactor", leftPulse.GetFloat("OffsetFactor") + 10);
        } else if (Input.GetKeyDown("down"))
        {
            leftPulse.SetFloat("OffsetFactor", leftPulse.GetFloat("OffsetFactor") - 5);
        }

        if (Input.GetKeyDown("right"))
        {
            gridDots.SetFloat("OffsetFactor", gridDots.GetFloat("OffsetFactor") + .05f);
        }
        else if (Input.GetKeyDown("left"))
        {
            gridDots.SetFloat("OffsetFactor", gridDots.GetFloat("OffsetFactor") - .025f);
        }
    }
}
