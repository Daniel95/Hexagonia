using UnityEngine;

/// <summary>
/// Handles and resets the Camera based on the scene index.
/// </summary>
public class CameraHolder : MonoBehaviour
{
    protected Transform DefaultCameraHolderTransform;
    protected GameObject MainCameraGameObject;

    protected virtual void EnterScene()
    {
        MainCameraGameObject = Camera.main.gameObject;

	    MainCameraGameObject.transform.ResetLocal();
        MainCameraGameObject.transform.parent.ResetLocal();
	    transform.ResetLocal();

        DefaultCameraHolderTransform = Camera.main.transform.root;
	    DefaultCameraHolderTransform.ResetLocal();

        MainCameraGameObject.transform.parent.parent = transform;

#if !UNITY_EDITOR
        if (VRSwitch.VRState)
        {
            GvrCardboardHelpers.Recenter();
        }
#endif
    }

    protected virtual void ExitScene()
    {
        MainCameraGameObject.transform.ResetLocal();
        MainCameraGameObject.transform.parent.ResetLocal();

        MainCameraGameObject.transform.parent.parent = DefaultCameraHolderTransform;

        MainCameraGameObject.transform.ResetLocal();
        MainCameraGameObject.transform.parent.ResetLocal();
        transform.ResetLocal();
        DefaultCameraHolderTransform.ResetLocal();

#if !UNITY_EDITOR
        if (VRSwitch.VRState)
        {
            GvrCardboardHelpers.Recenter();
        }
#endif
    }

    protected virtual void OnEnable()
    {
        SceneLoader.SceneSwitchStartedEvent += OnSceneSwitch;
    }

    protected virtual void OnDisable()
    {
        SceneLoader.SceneSwitchStartedEvent -= OnSceneSwitch;
    }

    private void OnSceneSwitch(Scenes? _oldScene, Scenes _newScene)
    {
        ExitScene();
    }

    private void ResetTransforms()
    {
        MainCameraGameObject.transform.position = Vector3.zero;
        MainCameraGameObject.transform.rotation = Quaternion.identity;
        MainCameraGameObject.transform.parent.transform.position = Vector3.zero;

        transform.position = Vector3.zero;
        DefaultCameraHolderTransform.position = Vector3.zero;
    }

    private void Awake()
	{
        EnterScene();
    }
}
