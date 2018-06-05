using UnityEngine.PostProcessing;

public class CameraHolderMain : CameraHolder
{
    private PostProcessingBehaviour postProcessingBehaviour;

    protected override void EnterScene()
    {
        base.EnterScene();

        postProcessingBehaviour = MainCameraGameObject.GetComponent<PostProcessingBehaviour>();
        postProcessingBehaviour.enabled = true;

        VRSwitch.Instance.GVRReticlePointerGameObject.SetActive(false);
    }

    protected override void ExitScene()
    {
        base.ExitScene();
        postProcessingBehaviour.enabled = false;
    }
}