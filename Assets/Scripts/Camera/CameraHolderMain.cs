using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraHolderMain : CameraHolder
{
    [SerializeField] [Range(0, 90)] private float nonVRFOV = 60;

    private PostProcessingBehaviour postProcessingBehaviour;

    private float previousFOV;

    protected override void EnterScene()
    {
        base.EnterScene();

        if(!VRSwitch.VRState)
        {
            previousFOV = Camera.main.fieldOfView;
            Camera.main.fieldOfView = nonVRFOV;
        }

        postProcessingBehaviour = MainCameraGameObject.GetComponent<PostProcessingBehaviour>();
        postProcessingBehaviour.enabled = true;

        VRSwitch.Instance.GVRReticlePointerGameObject.SetActive(false);
    }

    protected override void ExitScene()
    {
        base.ExitScene();

        if (!VRSwitch.VRState)
        {
            Camera.main.fieldOfView = previousFOV;
        }
        postProcessingBehaviour.enabled = false;
    }
}