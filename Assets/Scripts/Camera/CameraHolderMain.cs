using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraHolderMain : CameraHolder
{
    [SerializeField] private GameObject eventSystemGameobject;

    private PostProcessingBehaviour postProcessingBehaviour;

    protected override void EnterScene()
    {
        base.EnterScene();

        postProcessingBehaviour = MainCameraGameObject.GetComponent<PostProcessingBehaviour>();
        postProcessingBehaviour.enabled = true;

        if (VRSwitch.VRState)
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

        postProcessingBehaviour.enabled = false;

#if !UNITY_EDITOR
        if (VRSwitch.VRState)
        {
            GvrCardboardHelpers.Recenter();
        }
#endif
    }
}