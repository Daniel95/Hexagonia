﻿using UnityEngine;
using System;

/// <summary>
/// AudioEffect.cs Adds different audio effects for certain objects in the game. This is a generic script that is communicating with AudioManager.cs
/// </summary>

[Serializable]
public class AudioEffect
{
    public AudioClip Clip;
    public AudioEffectType Effect;
}

public enum AudioEffectType
{
    Coin,
	Obstacle,
}