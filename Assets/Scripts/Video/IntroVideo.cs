using System;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Plays the intro video and switched the scene when the video is done.
/// </summary>
[RequireComponent(typeof(VideoPlayer))]
public class IntroVideo : MonoBehaviour
{
    public static Action IntroVideoStartedEvent;
    public static Action IntroVideoCompletedEvent;

    [SerializeField] private Scenes sceneToLoadAfterVideo;

    private VideoPlayer videoPlayer;

    private void OnVideoFinished(VideoPlayer _videoPlayer)
    {
        SceneLoader.Instance.SwitchScene(sceneToLoadAfterVideo);

        if (IntroVideoCompletedEvent != null)
        {
            IntroVideoCompletedEvent();
        }
    }

    /*
    private IEnumerator WaitForVideoRead()
    {
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        videoPlayer.Play();

        MusicManager.Instance.StartMusic();

        if (IntroVideoStartedEvent != null)
        {
            IntroVideoStartedEvent();
        }
    }

    private void Start()
    {
        StartCoroutine(WaitForVideoRead());    
    }
    */

    private void Start()
    {
        videoPlayer.targetCamera = Camera.main;

        videoPlayer.Play();

        MusicManager.Instance.StartMusic();

        if (IntroVideoStartedEvent != null)
        {
            IntroVideoStartedEvent();
        }
    }

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    private void OnEnable()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnDisable()
    {
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}
