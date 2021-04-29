using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackInfo : MonoBehaviour
{
    public TMPro.TextMeshProUGUI title_text;
    public TMPro.TextMeshProUGUI artist_text;

    private string curr_title, curr_artist;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateTrackInfo(string name, string artist)
    {
        title_text.text = name;
        artist_text.text = "" + artist;
    }
}
