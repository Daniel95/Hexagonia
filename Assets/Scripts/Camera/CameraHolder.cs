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
    [SerializeField] private GameObject eventSystemGameobject;

    void Start()
    {
        mainCameraGameObject.transform.position = Vector3.zero;
        mainCameraGameObject.transform.rotation = Quaternion.identity;
        mainCameraGameObject.transform.parent.transform.position = Vector3.zero;
        transform.position = Vector3.zero;
        if (VRSwitch.Instance.VrState)
        {
            GvrCardboardHelpers.Recenter();
        }

    }
    
	void Awake ()
	{
	    mainCameraGameObject = Camera.main.gameObject;
	    mainCameraGameObject.transform.position = Vector3.zero;
	    mainCameraGameObject.transform.rotation = Quaternion.identity;
	    mainCameraGameObject.transform.parent.transform.position = Vector3.zero;
        defaultCameraHolderTransform = Camera.main.transform.root;
	    defaultCameraHolderTransform.position = Vector3.zero;
        mainCameraGameObject.transform.parent.parent = transform;

	    postProcessingBehaviour = mainCameraGameObject.GetComponent<PostProcessingBehaviour>();
	    gyro = mainCameraGameObject.GetComponent<Gyro>();

	    postProcessingBehaviour.enabled = true;

	    mainCameraGameObject.transform.position = Vector3.zero;
	    mainCameraGameObject.transform.rotation = Quaternion.identity;
	    mainCameraGameObject.transform.parent.transform.position = Vector3.zero;
	    transform.position = Vector3.zero;

        if (!VRSwitch.Instance.VrState)
	    {
	        gyro.enabled = true;

        }
        else
        {
            GvrCardboardHelpers.Recenter();
            eventSystemGameobject.SetActive(false);
        }
	    VRSwitch.Instance.GvrReticlePointerGameObject.SetActive(false);
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

    private void OnSceneSwitch(Scenes _oldScene, Scenes _newScene)
    {
        mainCameraGameObject.transform.position = Vector3.zero;
        mainCameraGameObject.transform.rotation = Quaternion.identity;
        mainCameraGameObject.transform.parent.transform.position = Vector3.zero;

        mainCameraGameObject.transform.parent.parent = defaultCameraHolderTransform;
        gyro.enabled = false;

        mainCameraGameObject.transform.position = Vector3.zero;
        mainCameraGameObject.transform.rotation = Quaternion.identity;
        mainCameraGameObject.transform.parent.transform.position = Vector3.zero;
        transform.position = Vector3.zero;
        defaultCameraHolderTransform.position = Vector3.zero;

        postProcessingBehaviour.enabled = false;

        if (VRSwitch.Instance.VrState)
        {
            GvrCardboardHelpers.Recenter();
        }

    }
}
