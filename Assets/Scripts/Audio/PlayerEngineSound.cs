using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEngineSound : MonoBehaviour {
    
    [SerializeField] float maxVolume;

    [Space(5)]

    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;

    [Space(5)]

    [SerializeField] float buffer = 0.1f;

    [SerializeField] AudioSource source;

    void Update ()
    {
        float _delta = SmoothPlaneMovement.Delta.sqrMagnitude;
        float _volumeDelta = (maxVolume * Mathf.InverseLerp(minSpeed, maxSpeed, _delta));

        if (_volumeDelta >= source.volume)
        {
            source.volume = (maxVolume * Mathf.InverseLerp(minSpeed, maxSpeed, _delta));
        }
        else
        {
            source.volume -= buffer * Time.deltaTime;
        }

        //Debug.Log(Mathf.InverseLerp(minSpeed, maxSpeed, _delta));
    }
}
