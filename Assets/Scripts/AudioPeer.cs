using UnityEngine;
using System;

/// <summary>
/// Converts raw GetSpectrumData to array's and sends it out
/// </summary>
public class AudioPeer : MonoBehaviour
{
    public static AudioPeer Instance
    {
        get
        {
            return GetInstance();
        }
    }
    private static AudioPeer instance;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private float decreaseSpeed = 0.005f;

    public Action<AudioPeer> TransmitAudioData;

    private float[] samples = new float[512];
    public float[] Samples
    {
        get
        {
            return samples;
        }
    }

    private float[] freqBand = new float[8];
    public float[] FreqBand
    {
        get
        {
            return freqBand;
        }
    }

    private float[] bandBuffer;
    public float[] BandBuffer
    {
        get
        {
            return bandBuffer;
        }
    }

    private float[] bufferDecrease;

    private void Awake ()
    {
        bandBuffer = new float[freqBand.Length];
        bufferDecrease = new float[freqBand.Length];

        if (audioSource == null)
        {
            UnityEditor.EditorUtility.DisplayDialog("Error", "Cannot visualise audio", "OK");
        }
    }
    
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyband();
        MakeFreqBandBuffer();

        if (TransmitAudioData != null)
            TransmitAudioData(instance);
    }

    /// <summary>
    /// Reads the audio for SpectrumData and sets the samples array t that data
    /// </summary>
    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    /// <summary>
    /// Takes the audio Frequency and reduces it to 8 averages
    /// </summary>
    void MakeFrequencyband()
    {
        int count = 0;

        for (int i = 0; i < freqBand.Length; i++)
        {
            float _average = 0;
            int _sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                _sampleCount += 2;
            }

            for (int j = 0; j < _sampleCount; j++)
            {
                _average += samples[count] * (count + 1);
                count += 1;
            }

            _average /= count;

            freqBand[i] = _average;
        }
    }

    /// <summary>
    /// Buffers the audio FrequencyBand to fall down slow instead if instantly
    /// </summary>
    void MakeFreqBandBuffer()
    {
        for (int i = 0; i < bandBuffer.Length; i++)
        {
            if (freqBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = freqBand[i];
            }
            if (freqBand[i] < bandBuffer[i])
            {
                bufferDecrease[i] = (bandBuffer[i] - freqBand[i]) * decreaseSpeed;
                bandBuffer[i] -= bufferDecrease[i];
            }
        }
    }

    private static AudioPeer GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<AudioPeer>();
        }
        return instance;
    }
}
