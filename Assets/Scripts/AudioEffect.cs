using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AudioEffect
{
    public AudioClip clip;
    public AudioEffectType Effect;
}

public enum AudioEffectType
{
    Coin,
}