using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumCubes : MonoBehaviour
{
    [SerializeField] GameObject sampleCube;

    [Space(5)]

    [SerializeField] float audioScale = 100;
    [SerializeField] float defaultScale = 2;

    GameObject[] sampleCubes = new GameObject[512];
    
	void Awake ()
    {
        for (int i = 0; i < sampleCubes.Length; i++)
        {
            GameObject instantiatedCube = Instantiate(sampleCube, transform.position, Quaternion.identity, transform);
            instantiatedCube.name = "SampleCube " + i;

            float angle = 360F / sampleCubes.Length;
            transform.eulerAngles = new Vector3(0,angle * i,0);
            instantiatedCube.transform.position = Vector3.forward * 100;

            sampleCubes[i] = instantiatedCube;
        }
	}
	
	void Update ()
    {
        UpdateSampleScale();
    }

    void UpdateSampleScale()
    {
        for (int i = 0; i < sampleCubes.Length; i++)
        {
            if (sampleCubes[i] != null)
            {
                sampleCubes[i].transform.localScale = new Vector3(1, (AudioPeer.Instance.Samples[i] * audioScale) + defaultScale, 1);
            }
        }
    }
}
