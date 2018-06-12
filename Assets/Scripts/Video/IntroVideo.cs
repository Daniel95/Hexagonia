using System;
using System.Collections;
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

    private IEnumerator PlayVideoAfterLoad()
    {
        videoPlayer.Play();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

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

    private void Start()
    {
        StartCoroutine(PlayVideoAfterLoad());
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