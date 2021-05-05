using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrianglePulser : MonoBehaviour
{
    public float bpm;
    public Material triMat;

    private Coroutine pulse;
    private float calculatedBpm, lastBpm;

    // Start is called before the first frame update
    void Start()
    {
        pulse = StartCoroutine(pulser());
    }

    // Update is called once per frame
    void Update()
    {
        if(bpm == 0)
        {
            Debug.Log("Stopped");
            StopCoroutine(pulse);
        } else if(bpm != lastBpm)
        {
            lastBpm = bpm;
            calculatedBpm = (1.0f / (bpm / 60.0f)) * 0.5f;
        }
    }

    private IEnumerator pulser()
    {
        yield return null;

        float elapsedTime = 0.0f;

        while (true)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime > calculatedBpm)
            {
                elapsedTime = 0.0f;
                triMat.SetFloat("StartPoint", Random.Range(0, 25) * 10);
            }
            yield return null;
        }
    }
}
