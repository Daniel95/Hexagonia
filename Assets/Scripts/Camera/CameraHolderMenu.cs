using UnityEngine.PostProcessing;

/// <summary>
/// Handles and resets the Camera based on the scene index.
/// </summary>
public class CameraHolderMenu : CameraHolder
{
    private PostProcessingBehaviour postProcessingBehaviour;

    protected override void EnterScene()
    {
        base.EnterScene();
        postProcessingBehaviour = MainCameraGameObject.GetComponent<PostProcessingBehaviour>();
        postProcessingBehaviour.enabled = true;
    }

    protected override void ExitScene()
    {
        base.ExitScene();
        postProcessingBehaviour.enabled = false;
    }
}
