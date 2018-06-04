using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestroy : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particleSystems;

    private void Update()
    {
        bool isPlaying = particleSystems.Exists(x => x.isPlaying);

        if(!isPlaying)
        {
            Destroy(gameObject);
        }
    }
}