using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the gameobject particlesystem active and deactive.
/// </summary>
public class ActivateParticle : MonoBehaviour {

	[SerializeField] private new List<ParticleSystem> particleSystem;

	private void Update()
	{
		bool isPlaying = particleSystem.Exists(x => x.isPlaying);

		if (!isPlaying)
		{
			for (int i = 0; i < particleSystem.Count; i++)
			{
				ObjectPool.Instance.PoolObject(particleSystem[i].gameObject);
				Debug.Log("Pooled: " + particleSystem[i].gameObject);
				//particleSystem[i].gameObject.SetActive(false);
			}
		}
	}
}
