using UnityEngine;

public class SpectrumAnalyzer : MonoBehaviour
{
    public AudioSource jukebox;

    public HexFreq hexVisualizer;

    public float SpectrumRefreshTime;

    public float[] freq_normalizers; // Swap these to be overrideable per song
    public int spectrum_divider;

    private float lastUpdate = 0;
    private float[] spectrum = new float[256];
    private float[] hexVals = new float[6];

    void Start()
    {
     
    }

    void Update()
    {
        if (Time.time - lastUpdate > SpectrumRefreshTime)
        {
            int index = 0;
            jukebox.GetSpectrumData(spectrum, 0, FFTWindow.Triangle);
            for (int i = 0; i < spectrum.Length; i++)
            {
                //We only care about 6 buckets
                index = Mathf.FloorToInt(i / (spectrum.Length / spectrum_divider));
                if (index < 6) {
                    hexVals[index] += spectrum[i];
                }
            }

            for(int side = 0; side < 6; side++)
            {
                hexVisualizer.setSize(side, Mathf.Clamp(hexVals[side] * freq_normalizers[side], 0.0f, 1.0f));
                hexVals[side] = 0.0f;
            }
            
            lastUpdate = Time.time;
        }
    }

    void hexFreqs()
    {

    }
}
