using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultSceneUI : MonoBehaviour
{
    public static DefaultSceneUI Instance { get { return GetInstance(); } }

    #region Singleton
    private static DefaultSceneUI instance;

    private static DefaultSceneUI GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<DefaultSceneUI>();
        }
        return instance;
    }
    #endregion

    public ScriptedAnimationController SceneFadeScriptedAnimationController { get { return sceneFadeScriptedAnimationController; } }

    [SerializeField] private ScriptedAnimationController sceneFadeScriptedAnimationController;
    [SerializeField] private ScriptedAnimationController vrWarningSceneFadeScriptedAnimationController;
    [SerializeField] private ScriptedAnimationController vrWarningFadeScriptedAnimationController;

    [SerializeField] private Image sceneFadeImage;
    [SerializeField] private List<Image> vrWarningImages;

    public void FadeSceneOut(Action _fadeSceneOutCompleted = null)
    {
        sceneFadeScriptedAnimationController.CancelAnimation(ScriptedAnimationType.Out);
        sceneFadeScriptedAnimationController.StartAnimation(ScriptedAnimationType.In, _fadeSceneOutCompleted);
    }

    public void FadeSceneIn(Action _fadeSceneInCompleted = null)
    {
        sceneFadeScriptedAnimationController.CancelAnimation(ScriptedAnimationType.In);
        sceneFadeScriptedAnimationController.StartAnimation(ScriptedAnimationType.Out, _fadeSceneInCompleted);
    }

    private void Initialize()
    {
        if (VRSwitch.VRState)
        {
            Color _color = sceneFadeImage.color;
            _color.a = 0;
            sceneFadeImage.color = _color;

            CoroutineHelper.DelayTime(3, () =>
            {
                vrWarningFadeScriptedAnimationController.StartAnimation(ScriptedAnimationType.Out, SwitchSceneFromVRWarning);
            });
        }
        else
        {
            foreach (Image _image in vrWarningImages)
            {
                Color _color = _image.color;
                _color.a = 0;
                _image.color = _color;
            }
            SceneLoader.Instance.LoadStartScene();
        }

        VRSwitch.SwitchedEvent -= Initialize;
    }

    private void SwitchSceneFromVRWarning()
    {
        SceneLoader.Instance.LoadStartScene();
        vrWarningSceneFadeScriptedAnimationController.StartAnimation(ScriptedAnimationType.Out);
    }

    private void OnEnable()
    {
        VRSwitch.SwitchedEvent += Initialize;
    }

    private void OnDisable()
    {
        VRSwitch.SwitchedEvent -= Initialize;
    }
}
