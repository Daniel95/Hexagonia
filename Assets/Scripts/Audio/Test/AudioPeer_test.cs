using UnityEngine;

public class AudioPeer_test : MonoBehaviour
{
	//I Did not follow the conventions
	private AudioSource audiosource;
	private float[] samples = new float[512];
	private float[] frequencyBand = new float[8];
	private float[] bandBuffer = new float[8];
	private float[] bufferDecrease = new float[8];

	private float[] frequencyBandHighest = new float[8];
	public static float[] audioBand = new float[8];
	public static float[] audioBandBuffer = new float[8];
	private float decreaseAmount = 1f;

	private const int LAST_BAND = 7;

	private void Start()
	{
		audiosource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		GetSpectrumAudioSource();
		MakeFrequencyband();
		BandBuffer();
		CreateAudioBands();
	}

	private void CreateAudioBands()
	{
		for (int i = 0; i < 8; i++)
		{
			if(frequencyBand[i] > frequencyBandHighest[i])
			{
				frequencyBandHighest[i] = frequencyBand[i];
			}
			audioBand[i] = (frequencyBand[i] / frequencyBandHighest[i]);
			audioBandBuffer[i] = (bandBuffer[i] / frequencyBandHighest[i]);
		}
	}

	private void GetSpectrumAudioSource()
	{
		audiosource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
	}

	private void BandBuffer()
	{
		for (int g = 0; g < 8; ++g)
		{
			if(frequencyBand[g] > bandBuffer[g])
			{
				bandBuffer[g] = frequencyBand[g];
				bufferDecrease[g] = 0.005f; //Magic Number
			}
			if(frequencyBand[g] < bandBuffer[g])
			{
				bandBuffer[g] -= bufferDecrease[g];
				bufferDecrease[g] *= 1.2f; //Magic Number
			}
		}
	}

	private void MakeFrequencyband()
	{
		int count = 0;

		for (int i = 0; i < frequencyBand.Length; i++)
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

			frequencyBand[i] = _average;
		}
	}
}