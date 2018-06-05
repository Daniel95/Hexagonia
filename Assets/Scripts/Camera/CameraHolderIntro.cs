using UnityEngine;
using UnityEngine.Video;

public class CameraHolderIntro : CameraHolder
{
	[SerializeField] private VideoPlayer videoPlayer;
	[SerializeField] private Scenes scene;
	private GameObject gvrPointer;

	protected override void EnterScene()
	{
		//gvrPointer = GameObject.Find("GvrReticlePointer");
	    gvrPointer = Resources.FindObjectsOfTypeAll<GvrReticlePointer>()[0].transform.gameObject;

        gvrPointer.SetActive(false);
		videoPlayer.targetCamera = Camera.main;
		videoPlayer.Play();
		videoPlayer.loopPointReached += CheckVideoPlayer;
		base.EnterScene();
	}

	private void CheckVideoPlayer(VideoPlayer vp)
	{ 
		SceneLoader.Instance.SwitchScene(scene);
		videoPlayer.loopPointReached -= CheckVideoPlayer;
	}

	protected override void ExitScene()
	{
		base.ExitScene();
	}
}