using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwayShade : MonoBehaviour
{
    public Material glitchEffect;
    public GameObject[] shades;
    public GameObject quad;

    public GameObject UICam;

    public float staggerMax = 0.2f;
    public float staggerMin = 0.01f;

    private bool isClosed = false;

    // Start is called before the first frame update
    void Start()
    {

        Material newMat;

        for (int i=0; i < shades.Length; i++)
        {
            //Clone material
            newMat = new Material(glitchEffect);
            newMat.SetFloat("GlitchAmount", 1.0f);
            shades[i].GetComponent<SpriteRenderer>().material = newMat;
            shades[i].SetActive(false);
        }

        newMat = new Material(glitchEffect);
        newMat.SetFloat("GlitchAmount", 1.0f);
        quad.GetComponent<SpriteRenderer>().material = newMat;
        quad.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            if (!isClosed)
            {
                shutter(true);
            } else
            {
                shutter(false);
            }
        }
    }

    public void shutter(bool shutDown)
    {
        if (shutDown && !isClosed)
        {
            StartCoroutine(glitchIn());
        } else if(!shutDown && isClosed)
        {
            StartCoroutine(glitchOut());
        }
    }

    //
    private IEnumerator glitchIn()
    {
        //UICam.SetActive(false);

        for (int i = 0; i < shades.Length; i++)
        {
            yield return new WaitForSeconds(Random.Range(staggerMin, staggerMax));
            StartCoroutine(panelResolve(shades[i]));
        }

        yield return new WaitForSeconds(Random.Range(staggerMin, staggerMax));
        StartCoroutine(panelResolve(quad));
        yield return new WaitForSeconds(Random.Range(staggerMin, staggerMax));

        for (int i = 0; i < shades.Length; i++)
        {
            shades[i].SetActive(false);
        }

        isClosed = true;
        yield return null;
    }

    private IEnumerator glitchOut()
    {
        StartCoroutine(panelRemove(quad));

        //yield return new WaitForSeconds(1.0f);
        //UICam.SetActive(true);
        isClosed = false;
        yield return null;
    }

    private IEnumerator panelResolve(GameObject panel)
    {
        Material m = panel.GetComponent<SpriteRenderer>().material;
        float glitchTime = 0.65f;
        float resolveTime = 0.05f;
        float elapsedTime = 0.0f;

        panel.SetActive(true);

        // Glitch for a moment
        yield return new WaitForSeconds(glitchTime);

        // Switch to normal sprite
        while(elapsedTime < resolveTime)
        {
            elapsedTime += Time.deltaTime;
            m.SetFloat("GlitchAmount", Mathf.Lerp(1.0f, 0.0f, elapsedTime / resolveTime));
            yield return null;
        }


        yield return null;
    }

    private IEnumerator panelRemove(GameObject panel)
    {
        Material m = panel.GetComponent<SpriteRenderer>().material;
        float glitchTime = 0.1f;
        float resolveTime = 0.05f;
        float elapsedTime = 0.0f;

        // Switch to glitch sprite
        while (elapsedTime < resolveTime)
        {
            elapsedTime += Time.deltaTime;
            m.SetFloat("GlitchAmount", Mathf.Lerp(0.0f, 1.0f, elapsedTime / resolveTime));
            yield return null;
        }

        // Glitch for a moment
        yield return new WaitForSeconds(glitchTime);

        // Fizzle out
        panel.SetActive(false);

        yield return null;
    }
}
