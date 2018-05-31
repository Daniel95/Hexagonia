using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pools the particle when done playing.
/// </summary>
public class ActivateParticle : MonoBehaviour {

	[SerializeField] private new List<ParticleSystem> particleSystem;

	private void Update()
	{
		bool isPlaying = particleSystem.Exists(x => x.isPlaying);

		if (!isPlaying)
		{
			ObjectPool.Instance.PoolObject(gameObject);
		}
	}

	private void OnEnable()
	{
		for (int i = 0; i < particleSystem.Count; i++)
		{
			particleSystem[i].Play();
		}
	}
}