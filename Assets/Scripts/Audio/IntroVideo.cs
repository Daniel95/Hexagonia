using System;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class IntroVideo : MonoBehaviour
{
    public static Action IntroVideoStartedEvent;
    public static Action IntroVideoCompletedEvent;

    [SerializeField] private Scenes scene;

    private VideoPlayer videoPlayer;

    private void OnVideoFinished(VideoPlayer _videoPlayer)
    {
        if (IntroVideoCompletedEvent != null)
        {
            IntroVideoCompletedEvent();
        }

        SceneLoader.Instance.SwitchScene(scene);
    }

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
