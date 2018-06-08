using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Couples the Sprite with its target pixel coordinates.
/// </summary>
[Serializable]
public class SpriteAndPixelCoordinatesPair
{
    public Sprite Sprite;
    public List<Vector2> PixelCoordinates;
}