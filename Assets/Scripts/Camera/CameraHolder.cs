using SRF;
using UnityEngine;

/// <summary>
/// Handles and resets the Camera based on the scene index
/// </summary>

public class CameraHolder : MonoBehaviour
{
    protected Transform DefaultCameraHolderTransform;
    protected GameObject MainCameraGameObject;

	private void Awake()
	{
        EnterScene();
    }

    protected void OnSceneSwitch(Scenes _oldScene, Scenes _newScene)
    {
        ExitScene();
    }

    protected virtual void EnterScene()
    {
        MainCameraGameObject = Camera.main.gameObject;

	    MainCameraGameObject.transform.ResetLocal();
        MainCameraGameObject.transform.parent.ResetLocal();
	    transform.ResetLocal();

        DefaultCameraHolderTransform = Camera.main.transform.root;
	    DefaultCameraHolderTransform.ResetLocal();

        MainCameraGameObject.transform.parent.parent = transform;
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
    }

    private void ResetTransforms()
    {
        MainCameraGameObject.transform.position = Vector3.zero;
        MainCameraGameObject.transform.rotation = Quaternion.identity;
        MainCameraGameObject.transform.parent.transform.position = Vector3.zero;

        transform.position = Vector3.zero;
        DefaultCameraHolderTransform.position = Vector3.zero;
    }

    private void OnEnable()
    {
        SceneLoader.SceneSwitchStartedEvent += OnSceneSwitch;
    }

    private void OnDisable()
    {
        SceneLoader.SceneSwitchStartedEvent -= OnSceneSwitch;
    }
}
