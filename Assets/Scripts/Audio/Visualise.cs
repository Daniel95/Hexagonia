using UnityEngine;

public class Visualise : MonoBehaviour
{

    [Range(0,7)]
    [SerializeField] private int band;
    [SerializeField] private Light lightSource;
    
    private void Update()
    {
        lightSource.intensity = AudioPeer.Instance.NormalisedFreqBand[band];
    }
}
