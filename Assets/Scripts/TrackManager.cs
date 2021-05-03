using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public bool includeExtendedTracks;
    public TrackInformation[] tracks;

    public TrackInformation[] extendedTracks;

    private List<TrackInformation> playlist;
    private int currentTrack;

    public SpectrumAnalyzer spectrum;

    public TrackInfo trackInfoDisplay;

    public Material LeftMaterial, CenterMaterial, TriMaterial;

    private AudioSource musicPlayer;
    private bool currentlyPlaying, stopped;
    private int currentBpm = 0;


    void Start()
    {
        //Locate the boombox
        musicPlayer = this.gameObject.GetComponent<AudioSource>();

        // Add all available tracks to the setlist
        playlist = new List<TrackInformation>();
        playlist.AddRange(tracks);

        if (includeExtendedTracks)
        {
            playlist.AddRange(extendedTracks);
        }

        shuffle();
        musicPlayer.Play();
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

        if (Input.GetKeyDown("n"))
        {
            nextTrack();
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
            shuffle();

            //Back to zero
            currentTrack = 0;
        }

        //Update track info in UI visualizer
        //trackInfoDisplay.updateTrackInfo(playlist[currentTrack].Name, playlist[currentTrack].Artist);
        FindObjectOfType<SocketListener>().updateTrack(playlist[currentTrack].Name, playlist[currentTrack].Artist, playlist[currentTrack].Bpm);

        // Start the track
        Debug.Log("Now playing: " + playlist[currentTrack].Name);
        musicPlayer.clip = playlist[currentTrack].Track;

        //Use track overrides for spectrum analysis
        spectrum.freq_normalizer = playlist[currentTrack].frequencyModifier;
        spectrum.curve_modifier = playlist[currentTrack].curveMultiplier;
        spectrum.spectrum_divider = playlist[currentTrack].freqDivisions;

        //Adjust BPM on Pulse UI
        StartCoroutine(adjustBPM(playlist[currentTrack].Bpm));

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

    private IEnumerator adjustBPM(int newBpm)
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
        TriMaterial.SetFloat("BPM", newBpm / 4.0f);
        CenterMaterial.SetFloat("BPM", newBpm);
    }

    /*
     * Class for music tracks
     */
    [System.Serializable]
    public class TrackInformation
    {
        [Header("Track Info")]
        public AudioClip Track; //
        public string Name;
        public string Artist;
        public int Bpm; // Track BPM to help with beat animation
        public float Pep; //Arbitrary value to blend motion intensity on

        [Header("Visualizer Tweaking")]
        public AnimationCurve frequencyModifier;
        public float curveMultiplier = 4.0f;
        public int freqDivisions = 12; // Number of divisions to divide the track into
        public int freqOffset = 0;

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
