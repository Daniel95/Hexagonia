using System;
using UnityEngine;
using UnityEngine.Video;

public class CameraHolderIntro : CameraHolder
{
    public static Action OnCompletedIntroVideoEvent;

    [SerializeField] [Range(0, 90)] private float nonVRFOV = 60;
    [SerializeField] private VideoPlayer videoPlayer;
	[SerializeField] private Scenes scene;

    private float previousFOV;

    protected override void EnterScene()
	{
	    base.EnterScene();

		videoPlayer.targetCamera = Camera.main;
		videoPlayer.Play();
		

	    if (!VRSwitch.VRState)
	    {
	        previousFOV = Camera.main.fieldOfView;
	        Camera.main.fieldOfView = nonVRFOV;
	    }
    }

	private void CheckVideoPlayer(VideoPlayer _vp)
	{ 
		SceneLoader.Instance.SwitchScene(scene);
	    OnCompletedIntroVideoEvent();
	    videoPlayer.loopPointReached -= CheckVideoPlayer;
    }

	protected override void ExitScene()
	{
		base.ExitScene();

	    if (!VRSwitch.VRState)
	    {
	        Camera.main.fieldOfView = previousFOV;
	    }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        videoPlayer.loopPointReached += CheckVideoPlayer;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ExitScene();
        videoPlayer.loopPointReached -= CheckVideoPlayer;
    }
}