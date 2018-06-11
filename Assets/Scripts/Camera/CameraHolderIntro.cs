using System;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Adjust camera settings in for the intro scene and start the intro video.
/// </summary>
public class CameraHolderIntro : CameraHolder
{
    public static Action OnCompletedMobileIntoCardboardEvent;
    public static Action OnCompletedIntroVideoEvent;

    [SerializeField] [Range(0, 90)] private float nonVRFOV = 60;
    [SerializeField] private VideoPlayer videoPlayer;
	[SerializeField] private Scenes scene;

    [SerializeField] private GameObject mobileIntoCardboardImageGameObject;
    [SerializeField] private GameObject skipIntroButtonGameObject;

    [SerializeField] private ScriptedAnimationController scriptedAnimationController;

    private GameObject gvrGameObject;
    private GameObject gvrReticlePointerGameObject;

    private const string VR_MODE = "VRMode";

    private float previousFOV;

    protected override void EnterScene()
	{
	    base.EnterScene();

	    gvrGameObject = FindObjectOfType<GvrHeadset>().transform.parent.gameObject;
	    gvrReticlePointerGameObject = FindObjectOfType<GvrReticlePointer>().gameObject;

	    //CoroutineHelper.DelayFrames(60, DisableGVR);

        if (Convert.ToBoolean(PlayerPrefs.GetInt(VR_MODE)))
        {
            skipIntroButtonGameObject.SetActive(false);

            CoroutineHelper.DelayTime(4f, StartMobileIntoCardboardAnimation);
        }
	    else
	    {
	        mobileIntoCardboardImageGameObject.SetActive(false);
	        skipIntroButtonGameObject.SetActive(false);

            previousFOV = Camera.main.fieldOfView;
	        Camera.main.fieldOfView = nonVRFOV;

	        StartVideo();
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

    private void StartMobileIntoCardboardAnimation()
    {
        scriptedAnimationController.StartAnimation(ScriptedAnimationType.Out, StartVideo);
    }

    private void StartVideo()
    {
        skipIntroButtonGameObject.SetActive(true);

        OnCompletedMobileIntoCardboardEvent();

        videoPlayer.targetCamera = Camera.main;
        videoPlayer.Play();
    }

    private void DisableGVR()
    {
        gvrGameObject.SetActive(false);
        gvrReticlePointerGameObject.SetActive(false);
    }

    private void CheckVideoPlayer(VideoPlayer _videoPlayer)
    {
        SceneLoader.Instance.SwitchScene(scene);

        if (OnCompletedIntroVideoEvent != null)
        {
            OnCompletedIntroVideoEvent();
        }

        videoPlayer.loopPointReached -= CheckVideoPlayer;
    }
}