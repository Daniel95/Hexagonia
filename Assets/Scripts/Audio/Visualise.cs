using UnityEngine;

public class Visualise : MonoBehaviour
{
    [Range(0, 7)] [SerializeField] private int band;
    [SerializeField] private int min, max;
    
    [Space(5)]

    [SerializeField] private Light lightSource;

    
    private void Update()
    {
        //lightSource.range = min + (max * AudioPeer.Instance.NormalisedFreqBandBuffer[band]);
        lightSource.range = min + (max * AudioPeer.Instance.SingleBand);
    }
}