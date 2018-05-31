using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestroy : MonoBehaviour
{

    [SerializeField] private new List<ParticleSystem> particleSystem; //misschien geen list

	//folder Effects -> 2 folders particle en scriptedAnimations

    private void Update() //veranderen naar enable/disable 
    {
        bool isPlaying = particleSystem.Exists(x => x.isPlaying);

        if(!isPlaying)
        {
            Destroy(gameObject);
        }
    }
}