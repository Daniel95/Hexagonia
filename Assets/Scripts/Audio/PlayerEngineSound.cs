using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEngineSound : MonoBehaviour {
    
    [SerializeField] float maxVolume;

    [Space(5)]

    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;

    [SerializeField] AudioSource source;

    void Update ()
    {
        float _delta = SmoothPlaneMovement.Delta.sqrMagnitude;
        
        source.volume = (maxVolume * Mathf.InverseLerp(minSpeed, maxSpeed, _delta));

        //Debug.Log(Mathf.InverseLerp(minSpeed, maxSpeed, _delta));
    }
}
