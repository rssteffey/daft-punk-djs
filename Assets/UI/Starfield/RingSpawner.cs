using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSpawner : MonoBehaviour
{
    public float bpm;

    public Sprite[] ringTextures;

    public GameObject ring;

    private float lastBpm, calculatedBpm;
    private SpriteRenderer ringRenderer;
    private Color startCol, endCol;


    // Start is called before the first frame update
    void Start()
    {
        ringRenderer = ring.GetComponent<SpriteRenderer>();
        startCol = new Color(ringRenderer.color.r, ringRenderer.color.g, ringRenderer.color.b, 0.0f);
        endCol = new Color(ringRenderer.color.r, ringRenderer.color.g, ringRenderer.color.b, ringRenderer.color.a);

        StartCoroutine(pulser());
    }

    // Update is called once per frame
    void Update()
    {
        if (bpm == 0)
        {
            
        }
        else if (bpm != lastBpm)
        {
            lastBpm = bpm;
            calculatedBpm = (1.0f / (bpm / 60.0f));
        }
    }

    private IEnumerator pulser()
    {
        yield return null;

        float elapsedTime = 0.0f;

        while (true)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > calculatedBpm)
            {
                //spawn ring
                StartCoroutine(spawnRing());
                elapsedTime = 0.0f;
            }
            yield return null;
        }
    }

    private IEnumerator spawnRing()
    {
        ring.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        ringRenderer.sprite = ringTextures[Random.Range(0, ringTextures.Length - 1)];
        ring.transform.Rotate(Vector3.forward, Random.Range(0, 360));

        float lifeTime = 0.25f;

        // Start scale-up
        StartCoroutine(scaleUp(lifeTime));

        //Fade in
        float fadeTime = lifeTime / 5.0f;
        float elapsedTime = 0.0f;

        while(elapsedTime < fadeTime)
        {
            ringRenderer.color = Color.Lerp(startCol, endCol, elapsedTime / fadeTime);
            elapsedTime+= Time.deltaTime;
            yield return null;
        }

        // Wait for rest of scale routine to finish
        yield return new WaitForSeconds((lifeTime / 5.0f) * 4.0f) ;

        elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            ringRenderer.color = Color.Lerp(endCol, startCol, elapsedTime / fadeTime);
            elapsedTime+= Time.deltaTime;
            yield return null;
        }

        ringRenderer.color = startCol;

       yield return null;
    }

    private IEnumerator scaleUp(float scaleTime)
    {
        yield return null;
        Vector3 endScale = new Vector3(0.37f, 0.37f, 0.37f);
        Vector3 startScale = ring.transform.localScale;

        float elapsedTime = 0.0f;

        while(elapsedTime < scaleTime)
        {
            ring.transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / scaleTime);
            elapsedTime+= Time.deltaTime;
            yield return null;
        }
    }
}
