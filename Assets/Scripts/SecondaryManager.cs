using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryManager : MonoBehaviour
{

    public Material LeftMaterial, triRGB, barRGB;

    public RingSpawner rings;

    public GameObject StarSpawner;

    private float currentBpm = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            StarSpawner.SetActive(!StarSpawner.activeSelf);
        }
    }

    public void receiveTrackInfo(string title, string artist, float bpm)
    {
        Debug.Log("Final Step: " + title + ", " + artist + ", " + bpm);

        FindObjectOfType<TrackInfo>().updateTrackInfo(title, artist);
        StartCoroutine(adjustBPM(bpm));
    }

    private IEnumerator adjustBPM(float newBpm)
    {
        // Wait til start of next pulse to transition
        float newVal = Mathf.FloorToInt(Time.time * 100) % 254;
        float lastCheckedValue;
        float bpmMultiplier = (currentBpm / 60) * 254;

        do
        {
            yield return null;
            lastCheckedValue = newVal;
            newVal = Mathf.FloorToInt(Time.time * bpmMultiplier) % 254;
        } while (lastCheckedValue <= newVal);


        currentBpm = newBpm;
        setBpmMaterials(newBpm);
        yield return null;
    }


    public void setBpmMaterials(float newBpm)
    {
        LeftMaterial.SetFloat("BPM", newBpm);
        triRGB.SetFloat("BPM", newBpm);
        barRGB.SetFloat("BPM", newBpm);

        rings.bpm = newBpm;
    }
}
