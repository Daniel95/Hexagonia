using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class HyperSpaceParticleEffect : MonoBehaviour
{
    [SerializeField] private float speed = 1;

    private ParticleSystem.MainModule particleSystemMainModule;

    private void Update()
    {
        particleSystemMainModule.simulationSpeed = ChunkMover.Instance.Speed * speed;
    }

    private void Awake()
    {
        particleSystemMainModule = GetComponent<ParticleSystem>().main;
    }
}