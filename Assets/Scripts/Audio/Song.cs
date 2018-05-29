using UnityEngine;
using System;

[Serializable]
public class Song
{
    [HideInInspector] public int priority = 0;
    public AudioClip clip;
}