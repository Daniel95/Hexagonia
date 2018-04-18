using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays the current song's audio spectrum average as scaled cubes
/// </summary>
public class SpectrumCubes : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab;

    [Space(5)]

    [SerializeField] float audioScale = 100;
    [SerializeField] float defaultScale = 2;

    GameObject[] sampleCubes = new GameObject[512];
    
	void Awake ()
    {
        InstantiateCubes();
    }

    /// <summary>
    /// Instantiates all the cubes needed for the audio visualisation.
    /// </summary>
    private void InstantiateCubes()
    {
        for (int i = 0; i < sampleCubes.Length; i++)
        {
            float _angle = 360F / sampleCubes.Length;

            sampleCubes[i] = Instantiate(cubePrefab, transform.position, Quaternion.identity, transform);
            sampleCubes[i].name = "SampleCube " + i;

            transform.eulerAngles = new Vector3(0, _angle * i, 0);
            sampleCubes[i].transform.position = Vector3.forward * 100;
        }
    }
    
    /// <summary>
    /// Updates the cubes scale based on the frequency.
    /// </summary>
    void UpdateSampleScale(AudioPeer _audioPeer)
    {
        for (int i = 0; i < sampleCubes.Length; i++)
        {
            if (sampleCubes[i] != null)
            {
                sampleCubes[i].transform.localScale = new Vector3(1, (_audioPeer.Samples[i] * audioScale) + defaultScale, 1);
            }
        }
    }

    private void OnEnable()
    {
        AudioPeer.Instance.TransmitAudioData += UpdateSampleScale;
    }
    private void DisEnable()
    {
        AudioPeer.Instance.TransmitAudioData -= UpdateSampleScale;
    }
}
