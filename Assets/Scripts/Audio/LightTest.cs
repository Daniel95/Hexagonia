using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTest : MonoBehaviour {

    [Range(0, 7)]
    [SerializeField] private int band; 

    private AudioPeer audioPeer;

    [SerializeField] private Light lightSource;

    [Space(5)]

    private int min = 1;
    private int max = 10;
	
	// Update is called once per frame
	void Update ()
    {
        if (audioPeer != null)
            UpdateLight();
	}

    private void UpdateLight()
    {
        lightSource.range = min + (max - min) * audioPeer.NormalisedFreqBandBuffer[band];
    }

    private void GetAudioPeer(AudioPeer _audioPeer)
    {
        audioPeer = _audioPeer;
    }

    private void OnEnable()
    {
        AudioPeer.Instance.TransmitAudioData += GetAudioPeer;
    }
    private void OnDisable()
    {
        AudioPeer.Instance.TransmitAudioData -= GetAudioPeer;
    }
}
