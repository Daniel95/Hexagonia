using SRF;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] private GameObject eventSystemGameobject;

    private Transform defaultCameraHolderTransform;
    private GameObject mainCameraGameObject;
    private PostProcessingBehaviour postProcessingBehaviour;
    private Gyro gyro;

	private void Awake ()
	{
	    mainCameraGameObject = Camera.main.gameObject;

	    mainCameraGameObject.transform.ResetLocal();
        mainCameraGameObject.transform.parent.ResetLocal();
	    transform.ResetLocal();

        defaultCameraHolderTransform = Camera.main.transform.root;
	    defaultCameraHolderTransform.ResetLocal();

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
#if !UNITY_EDITOR
            GvrCardboardHelpers.Recenter();
#endif
            eventSystemGameobject.SetActive(false);
        }
	    VRSwitch.Instance.GvrReticlePointerGameObject.SetActive(false);
    }

    private void OnEnable()
    {
        SceneLoader.SceneSwitchStartedEvent += OnSceneSwitch;
    }

    private void OnDisable()
    {
        SceneLoader.SceneSwitchStartedEvent -= OnSceneSwitch;
    }

    private void OnSceneSwitch(Scenes? _oldScene, Scenes _newScene)
    {
        mainCameraGameObject.transform.ResetLocal();
        mainCameraGameObject.transform.parent.ResetLocal();

        mainCameraGameObject.transform.parent.parent = defaultCameraHolderTransform;
        gyro.enabled = false;

        mainCameraGameObject.transform.ResetLocal();
        mainCameraGameObject.transform.parent.ResetLocal();
        transform.ResetLocal();
        defaultCameraHolderTransform.ResetLocal();

        postProcessingBehaviour.enabled = false;

#if !UNITY_EDITOR
        if (VRSwitch.Instance.VrState)
        {
            GvrCardboardHelpers.Recenter();
        }
#endif
    }

    private void ResetTransforms()
    {
        mainCameraGameObject.transform.position = Vector3.zero;
        mainCameraGameObject.transform.rotation = Quaternion.identity;
        mainCameraGameObject.transform.parent.transform.position = Vector3.zero;

        transform.position = Vector3.zero;
        defaultCameraHolderTransform.position = Vector3.zero;
    }
}
