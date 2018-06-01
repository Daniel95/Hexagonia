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
            eventSystemGameobject.SetActive(false);
        }
        VRSwitch.Instance.GVRReticlePointerGameObject.SetActive(false);
    }

    protected override void ExitScene()
    {
        base.ExitScene();

        gyro.enabled = false;
        postProcessingBehaviour.enabled = false;
    }
}