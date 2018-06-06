using UnityEngine;
using UnityEngine.Video;

public class CameraHolderIntro : CameraHolder
{
    [SerializeField] [Range(0, 90)] private float nonVRFOV = 60;
    [SerializeField] private VideoPlayer videoPlayer;
	[SerializeField] private Scenes scene;

	private GameObject gvrPointer;
    private float previousFOV;

    protected override void EnterScene()
	{
	    base.EnterScene();

        gvrPointer = Resources.FindObjectsOfTypeAll<GvrReticlePointer>()[0].transform.gameObject;
        //gvrPointer.SetActive(false);

		videoPlayer.targetCamera = Camera.main;
		videoPlayer.Play();
		videoPlayer.loopPointReached += CheckVideoPlayer;

	    if (!VRSwitch.VRState)
	    {
	        previousFOV = Camera.main.fieldOfView;
	        //Camera.main.fieldOfView = nonVRFOV;
	    }
    }

	private void CheckVideoPlayer(VideoPlayer _vp)
	{ 
		SceneLoader.Instance.SwitchScene(scene);
		videoPlayer.loopPointReached -= CheckVideoPlayer;
	}

	protected override void ExitScene()
	{
		base.ExitScene();

	    if (!VRSwitch.VRState)
	    {
	        //Camera.main.fieldOfView = previousFOV;
	    }
    }
}