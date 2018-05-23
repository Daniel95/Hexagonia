using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraHolder : MonoBehaviour
{
    private Transform defaultCameraHolderTransform;
    private GameObject mainCameraGameObject;
    private PostProcessingBehaviour postProcessingBehaviour;
    private Gyro gyro;

	void Awake ()
	{
	    mainCameraGameObject = Camera.main.gameObject;
	    defaultCameraHolderTransform = Camera.main.transform.root;
        mainCameraGameObject.transform.parent.parent = transform;

	    postProcessingBehaviour = mainCameraGameObject.GetComponent<PostProcessingBehaviour>();
	    gyro = mainCameraGameObject.GetComponent<Gyro>();

	    postProcessingBehaviour.enabled = true;

	    if (!VRSwitch.Instance.VrState)
	    {
	        gyro.enabled = true;
        }
	    else
	    {
	        VRSwitch.Instance.GvrReticlePointerGameObject.SetActive(false);
        }
	}
	
	private void Update () {
		
	}

    private void OnEnable()
    {
        SceneLoader.SceneSwitchStartedEvent += OnSceneSwitch;
    }

    private void OnDisable()
    {
        SceneLoader.SceneSwitchStartedEvent -= OnSceneSwitch;
    }

    private void OnSceneSwitch()
    {
        mainCameraGameObject.transform.parent.parent = defaultCameraHolderTransform;

        mainCameraGameObject.transform.position = Vector3.zero;
        mainCameraGameObject.transform.parent.transform.position = Vector3.zero;

        postProcessingBehaviour.enabled = false;
        gyro.enabled = false;
    }
}
