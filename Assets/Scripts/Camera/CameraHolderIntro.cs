using UnityEngine;
using UnityEngine.Video;

public class CameraHolderIntro : CameraHolder
{
	[SerializeField] private VideoPlayer videoPlayer;
	[SerializeField] private Scenes scene;


	protected override void EnterScene()
	{
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