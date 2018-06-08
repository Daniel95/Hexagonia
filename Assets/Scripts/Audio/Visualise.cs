using UnityEngine;
/// <summary>
/// Changes the lightsource with the frequence of the AudioPeer.cs
/// </summary>
public class Visualise : MonoBehaviour
{
#pragma warning disable CS0649, 
    [SerializeField] private int min, max;
#pragma warning restore CS0649,

    [SerializeField] private Light lightSource;
 
    private void Update()
    {
        lightSource.range = min + (max * AudioPeer.SingleBand);
    }
}