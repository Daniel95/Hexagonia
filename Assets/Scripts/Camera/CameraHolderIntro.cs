using UnityEngine;

/// <summary>
/// Adjust camera settings for the intro scene and start the intro video.
/// </summary>
public class CameraHolderIntro : CameraHolder
{
    [SerializeField] [Range(0, 90)] private float nonVRFOV = 60;

    private float previousFOV;

    protected override void EnterScene()
	{
	    base.EnterScene();
		
	    if (!VRSwitch.VRState)
	    {
	        previousFOV = Camera.main.fieldOfView;
	        Camera.main.fieldOfView = nonVRFOV;
	    }
    }

	protected override void ExitScene()
	{
		base.ExitScene();

	    if (!VRSwitch.VRState)
	    {
	        Camera.main.fieldOfView = previousFOV;
	    }
    }
}