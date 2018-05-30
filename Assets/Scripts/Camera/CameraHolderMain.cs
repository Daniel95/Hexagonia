using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraHolderMain : CameraHolder
{
    [SerializeField] private GameObject eventSystemGameobject;

    private PostProcessingBehaviour postProcessingBehaviour;
    private Gyro gyro;

    protected override void EnterScene()
    {
        base.EnterScene();

        postProcessingBehaviour = MainCameraGameObject.GetComponent<PostProcessingBehaviour>();
        gyro = MainCameraGameObject.GetComponent<Gyro>();

        postProcessingBehaviour.enabled = true;

        if (!VRSwitch.Instance.VRState)
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
        VRSwitch.Instance.GVRReticlePointerGameObject.SetActive(false);
    }

    protected override void ExitScene()
    {
        base.ExitScene();

        gyro.enabled = false;
        postProcessingBehaviour.enabled = false;

#if !UNITY_EDITOR
        if (VRSwitch.Instance.VRState)
        {
            GvrCardboardHelpers.Recenter();
        }
#endif
    }
}