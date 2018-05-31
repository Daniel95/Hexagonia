using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        float[] _audioData = new float[/*audioSource.clip.samples * audioSource.clip.channels*/ 1000];

        audioSource.clip.GetData(_audioData, 0);

        for (int i = 0; i < _audioData.Length; i++)
        {
            //Debug.Log(_audioData[i]);
        }
    }
}