using UnityEngine;

public class Visualise : MonoBehaviour
{
#pragma warning disable CS0649, 
    [SerializeField] private int min, max;
#pragma warning restore CS0649,

    [Space(5)]

    [SerializeField] private Light lightSource;

    
    private void Update()
    {
        lightSource.range = min + (max * AudioPeer.Instance.SingleBand);
    }
}