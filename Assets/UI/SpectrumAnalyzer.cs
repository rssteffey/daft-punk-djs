using UnityEngine;

public class SpectrumAnalyzer : MonoBehaviour
{
    public AudioSource jukebox;

    public HexFreq hexVisualizer;

    public float SpectrumRefreshTime;

    public float[] freq_normalizers; // Swap these to be overrideable per song

    public AnimationCurve freq_normalizer;
    public float curve_modifier = 5.0f;

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
                    hexVals[index] += spectrum[i] * freq_normalizer.Evaluate(i / 256.0f);
                }
            }

            for(int side = 0; side < 6; side++)
            {
                float strength = Mathf.Clamp(hexVals[side] * curve_modifier, 0.0f, 1.0f);
                hexVisualizer.setSize(side, strength);
                hexVals[side] = 0.0f;
            }
            
            lastUpdate = Time.time;
        }
    }

    void hexFreqs()
    {

    }
}
