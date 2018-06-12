using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pools the particle when done playing.
/// </summary>
public class ActivateParticle //: MonoBehaviour {

	[SerializeField] private List<ParticleSystem> particleSystems;

	private void Update()
	{
		bool _isPlaying = particleSystems.Exists(x => x.isPlaying);

		if (!_isPlaying)
		{
			ObjectPool.Instance.PoolObject(gameObject);
		}
	}

	private void OnEnable()
	{
		for (int i = 0; i < particleSystems.Count; i++)
		{
			particleSystems[i].Play();
		}
	}
}