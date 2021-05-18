using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vocoder : MonoBehaviour
{
    public LineRenderer lr;
    public AnimationCurve curve;
    public AnimationCurve volumeCurve;
    public Material AlexaMat;

    private Vector3[] originalPoints, updatePoints;

    private bool talking;

    private float speechVolume;
    private Color startCol, startAlexaCol;
    private Material lineMat;

    // Start is called before the first frame update
    void Start()
    {
        //Save LineRenderer start points
        originalPoints = new Vector3[lr.positionCount];
        for (int i = 0; i < lr.positionCount; i++)
        {
            originalPoints[i] = lr.GetPosition(i);
        }

        updatePoints = new Vector3[lr.positionCount];
        speechVolume = 0.0f;
        lineMat = lr.materials[0];
        startCol = lineMat.color;

        //Default hidden
        lineMat.color = new Color(startCol.r, startCol.g, startCol.b, 0.0f);
        AlexaMat.SetFloat("Opacity", 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            speak(5.0f);
        }
        
    }

    public void speak(float speakTime)
    {
        StartCoroutine(alexaSpeak(speakTime));
    }

    private IEnumerator alexaSpeak(float speakTime)
    {
        // Fade 'er
        StartCoroutine(fadeIn(0.5f));

        float elapsedTime = 0.0f;

        while (elapsedTime < speakTime)
        {
            for (int i = 0; i < lr.positionCount; i++)
            {
                float multiplier = Random.Range(-1.0f, 1.0f) * curve.Evaluate(i / (lr.positionCount * 1.0f)) * volumeCurve.Evaluate(elapsedTime);

                updatePoints[i] = new Vector3(originalPoints[i].x, originalPoints[i].y + multiplier, originalPoints[i].z);
            }
            lr.SetPositions(updatePoints);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lr.SetPositions(originalPoints);

        // Fade away
        StartCoroutine(fadeOut(1.0f));
        yield return null;  
    
    }

    private IEnumerator fadeIn(float fadeTime)
    {
        float elapsedTime = 0.0f;

        Color endCol = new Color(startCol.r, startCol.g, startCol.b, 0.0f);

        while (elapsedTime < fadeTime)
        {
            lineMat.color = Color.Lerp(endCol, startCol, (elapsedTime / fadeTime));
            AlexaMat.SetFloat("Opacity", Mathf.Lerp(0.0f, 1.0f, (elapsedTime / fadeTime)));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        AlexaMat.SetFloat("Opacity", 1.0f);
    }

    private IEnumerator fadeOut(float fadeTime)
    {
        float elapsedTime = 0.0f;

        Color endCol = new Color(startCol.r, startCol.g, startCol.b, 0.0f);

        while(elapsedTime < fadeTime)
        {
            lineMat.color = Color.Lerp(startCol, endCol, (elapsedTime / fadeTime));
            AlexaMat.SetFloat("Opacity", Mathf.Lerp(1.0f, 0.0f, (elapsedTime / fadeTime)));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        AlexaMat.SetFloat("Opacity", 0.0f);
    }
}
