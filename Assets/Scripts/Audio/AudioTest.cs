using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    private void Start()
    {
        AudioProcessor.onBeat += OnBeat;
    }

    private void OnBeat()
    {
        Debug.Log("Hit that beat");
    }
}