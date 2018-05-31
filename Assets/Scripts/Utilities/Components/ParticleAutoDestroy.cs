using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestroy : MonoBehaviour
{
    [SerializeField] private new List<ParticleSystem> particleSystem;

    private void Update()
    {
        bool isPlaying = particleSystem.Exists(x => x.isPlaying);

        if(!isPlaying)
        {
            Destroy(gameObject);
        }
    }
}