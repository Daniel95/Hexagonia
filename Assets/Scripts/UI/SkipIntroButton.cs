using System;
using UnityEngine;

/// <summary>
/// Button that skips the Intro cutscene when pressed
/// </summary>
public class SkipIntroButton : GazeButton
{
    [SerializeField] private Scenes scene;
    [SerializeField] private const string NOT_FIRST_TIME_LAUNCHED = "FirstTimeLaunched";

    protected override void OnTrigger()
    {
        if (Convert.ToBoolean(PlayerPrefs.GetInt(NOT_FIRST_TIME_LAUNCHED)))
        {
            SceneLoader.Instance.SwitchScene(scene);
            PlatformBaseInput.DownInputEvent -= OnInput;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PlatformBaseInput.DownInputEvent += OnInput;
        CameraHolderIntro.OnCompletedIntroVideoEvent += SetFirstTimeLaunchedPrefs;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlatformBaseInput.DownInputEvent -= OnInput;
        CameraHolderIntro.OnCompletedIntroVideoEvent -= SetFirstTimeLaunchedPrefs;
    }

    private void SetFirstTimeLaunchedPrefs()
    {
        PlayerPrefs.SetInt(NOT_FIRST_TIME_LAUNCHED, Convert.ToInt32(true));
    }

    private void OnInput(Vector2 _position)
    {
        TryToTrigger();
    }
}