using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public int test;
    public TrackInformation[] tracks;

    private List<TrackInformation> playlist;
    private int currentTrack;

    private AudioSource musicPlayer;
    private bool currentlyPlaying, stopped;


    void Start()
    {
        //Locate the boombox
        musicPlayer = this.gameObject.GetComponent<AudioSource>();

        // Add all available tracks to the setlist
        playlist = new List<TrackInformation>();
        playlist.AddRange(tracks);
    }

    void Update()
    {
        if (!musicPlayer.isPlaying)
        {
            nextTrack();
        }

        if (Input.GetKeyDown("s"))
        {
            shuffle();
        }
    }

    public TrackInformation GetCurrentTrack()
    {
        return playlist[currentTrack];
    }

    public void nextTrack()
    {
        // Next up
        currentTrack++;
        if(currentTrack == playlist.Count)
        {
            //shuffle songs?

            //Back to zero
            currentTrack = 0;
        }

        // Start the track
        Debug.Log("Now playing: " + playlist[currentTrack].Name);
        musicPlayer.clip = playlist[currentTrack].Track;
        musicPlayer.Play();
    }

    private void shuffle()
    {
        foreach(TrackInformation track in playlist)
        {
            track.shuffle();
        }

        playlist.Sort((x, y) => x.getShuffle().CompareTo(y.getShuffle()));
    }

    /*
     * Class for music tracks
     */
    [System.Serializable]
    public class TrackInformation
    {
        public AudioClip Track; //
        public string Name;
        public int Bpm; // Track BPM to help with beat animation
        public float Pep; //Arbitrary value to blend motion intensity on

        private float shuffleVal = 0.0f;

        TrackInformation()
        {
            
        }

        public void shuffle()
        {
            shuffleVal = Random.Range(0.0f, 1.0f);
        }

        public float getShuffle()
        {
            return shuffleVal;
        }
    }
}
