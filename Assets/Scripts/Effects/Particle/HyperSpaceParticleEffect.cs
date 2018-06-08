using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class HyperSpaceParticleEffect : MonoBehaviour
{
    [SerializeField] private float speed = 1;

    private new ParticleSystem particleSystem;

    private void Update()
    {
        var particleSystemMainModule = particleSystem.main;
        particleSystemMainModule.simulationSpeed = ChunkMover.Instance.Speed * speed;
    }

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }
}