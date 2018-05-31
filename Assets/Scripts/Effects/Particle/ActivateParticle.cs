using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the gameobject particlesystem active and deactive.
/// </summary>
public class ActivateParticle : MonoBehaviour {

	[SerializeField] private new List<ParticleSystem> particleSystem;
	//[SerializeField] private ParticleSystem _particleSystem;

	private void Update()
	{
		bool isPlaying = particleSystem.Exists(x => x.isPlaying);

		if (!isPlaying)
		{
			for (int i = 0; i < particleSystem.Count; i++)
			{
				particleSystem[i].gameObject.SetActive(false);
			}
			//gameObject.SetActive(false);
		}
		else if(isPlaying)
		{
			for (int i = 0; i < particleSystem.Count; i++)
			{
				particleSystem[i].gameObject.SetActive(true);
			}
			//gameObject.SetActive(true);
		}
	}
}
