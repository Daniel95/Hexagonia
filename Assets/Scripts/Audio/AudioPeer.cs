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
    #region SingleTon
    private static AudioPeer instance;
    private static AudioPeer GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<AudioPeer>();
        }
        return instance;
    }
    #endregion
    
    public Action<AudioPeer> TransmitAudioData;

    public float[] Samples
    {
        get
        {
            return samples;
        }
    }
    public float[] FreqBand
    {
        get
        {
            return freqBand;
        }
    }

    public float[] BandBuffer
    {
        get
        {
            return bandBuffer;
        }
    }
    
    public float[] NormalisedFreqBand
    {
        get
        {
            return normalisedFreqBand;
        }
    }
    public float[] NormalisedFreqBandBuffer
    {
        get
        {
            return normalisedFreqBandBuffer;
        }
    }

    private float[] samples = new float[512];
    private float[] freqBand = new float[8];

    private float[] bandBuffer;
    
    private float[] normalisedFreqBandBuffer;
    private float[] normalisedFreqBand;

    private float[] bufferDecrease;
    private float[] freqBandHighest;

    [SerializeField] private float decreaseSpeed = 0.005f;

    [SerializeField] private AudioSource audioSource;

    private void Awake ()
    {
        bandBuffer = new float[freqBand.Length];
        bufferDecrease = new float[freqBand.Length];
        freqBandHighest = new float[freqBand.Length];
        normalisedFreqBand = new float[freqBand.Length];
        normalisedFreqBandBuffer = new float[freqBand.Length];
    }
    
    private void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyband();
        MakeFreqBandBuffer();
        NormaliseFreqBand();

        if (TransmitAudioData != null)
            TransmitAudioData(instance);

        for (int i = 0; i < NormalisedFreqBandBuffer.Length; i++)
        {
            Debug.Log(NormalisedFreqBandBuffer[i]);
        }
    }
    
    private void NormaliseFreqBand()
    {
        for (int i = 0; i < NormalisedFreqBand.Length; i++)
        {
            if (freqBand[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = freqBand[i];
            }
            NormalisedFreqBand[i] = (freqBand[i] / freqBandHighest[i]);
            normalisedFreqBandBuffer[i] = (bandBuffer[i] / freqBandHighest[i]);
        }
    }

    /// <summary>
    /// Reads the audio for SpectrumData and sets the samples array t that data
    /// </summary>
    private void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    /// <summary>
    /// Takes the audio Frequency and reduces it to 8 averages
    /// </summary>
    private void MakeFrequencyband()
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
    private void MakeFreqBandBuffer()
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

}
