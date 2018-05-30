using System;
using UnityEngine;

/// <summary>
/// Couples the Trail prefab with the TargetPixelCoordinatesLabel.
/// </summary>
[Serializable]
public class TrailPrefabAndLabelPair
{
    public GameObject Prefab;
    public TargetPixelCoordinatesLabel Label;
}