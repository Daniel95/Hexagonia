using UnityEngine;
using System;

/// <summary>
///	Converts raw GetSpectrumData to array's and sends it out
/// </summary>
public class AudioPeer : MonoBehaviour
{
    public static AudioPeer Instance { get { return GetInstance(); } }

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
    
    public static Action<AudioPeer> TransmitAudioData;

    public float[] Samples { get { return samples; } }
    public float[] FreqBand { get { return freqBand; } }
    public float[] BandBuffer { get { return bandBuffer; } }
    public float[] NormalisedFreqBand { get { return normalisedFreqBand; } }
	public float[] NormalisedFreqBandBuffer { get { return normalisedFreqBandBuffer; } }
    public float SingleBand { get { return singleBand; } }

	private const int LAST_BAND = 7;

	[SerializeField] private float decreaseSpeed = 0.005f;
	[SerializeField] private AudioSource audioSource;

	private float[] samples = new float[512];
	private float[] freqBand = new float[8];
    private float[] bandBuffer;
    private float[] normalisedFreqBandBuffer;
    private float[] normalisedFreqBand;
    private float[] bufferDecrease;
    private float[] freqBandHighest;
    private float singleBand;

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
        MakeSingleBand();

        if (TransmitAudioData != null)
            TransmitAudioData(instance);
    }

    private void MakeSingleBand()
    {
        float _average = 0;
        foreach (float _float in normalisedFreqBandBuffer)
        {
            _average += _float;
        }

        singleBand = _average / NormalisedFreqBandBuffer.Length;
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

    private void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    private void MakeFrequencyband()
    {
        int count = 0;

        for (int i = 0; i < freqBand.Length; i++)
        {
            float _average = 0;
            int _sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == LAST_BAND)
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