using UnityEngine;
using System;

/// <summary>
/// Container for the audio clip and what priority it has. 
/// </summary>
[Serializable]
public class Song
{
    [HideInInspector] public int Priority = 0;
    public AudioClip Clip;
}