using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class HyperSpaceEffect : MonoBehaviour
{
    [SerializeField] private float minSpeed = 1;
    [SerializeField] private float maxSpeed = 1;
    [SerializeField] private float minSpawnRate = 1;
    [SerializeField] private float maxSpawnRate = 1;

    private float speedRange;
    private float spawnRateRange;
    private ParticleSystem.MainModule particleSystemMainModule;
    private ParticleSystem.EmissionModule particleSystemEmissionModule;
    private Coroutine hyperSpaceUpdateCoroutine;

    private IEnumerator HyperSpaceUpdate()
    {
        while(true)
        {
            float _progress = ChunkMover.Instance.SpeedProgress;
            particleSystemEmissionModule.rateOverTime = minSpawnRate + (spawnRateRange * _progress);
            particleSystemMainModule.simulationSpeed = minSpeed + (speedRange * _progress);
            yield return new WaitForSeconds(1);
        }
    }

    private void StopHyperSpaceUpdate()
    {
        StopCoroutine(hyperSpaceUpdateCoroutine);
        particleSystemEmissionModule.rateOverTime = 0;
    }

    private void Awake()
    {
        ParticleSystem _particleSystem = GetComponent<ParticleSystem>();
        particleSystemMainModule = _particleSystem.main;
        particleSystemEmissionModule = _particleSystem.emission;

        speedRange = maxSpeed - minSpeed; 
        spawnRateRange = maxSpawnRate - minSpawnRate;

        hyperSpaceUpdateCoroutine = StartCoroutine(HyperSpaceUpdate());
    }

    private void OnEnable()
    {
        PlayerCollisions.DiedEvent += StopHyperSpaceUpdate;
    }

    private void OnDisable()
    {
        PlayerCollisions.DiedEvent -= StopHyperSpaceUpdate;
    }
}