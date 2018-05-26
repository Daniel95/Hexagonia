using System;
using UnityEngine;

[RequireComponent(typeof(ScriptedAnimationController))]
public class DefaultSceneUI : MonoBehaviour
{

    public static DefaultSceneUI Instance
    {
        get
        {
            return GetInstance();
        }
    }

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

    public ScriptedAnimationController ScriptedAnimationController { get { return scriptedAnimationController; } }

    private ScriptedAnimationController scriptedAnimationController;

    public void FadeSceneOut(Action _fadeSceneOutCompleted)
    {
        scriptedAnimationController.CancelAnimation(ScriptedAnimationType.Out);
        scriptedAnimationController.StartAnimation(ScriptedAnimationType.In, _fadeSceneOutCompleted);
    }

    public void FadeSceneIn(Action _fadeSceneInCompleted)
    {
        scriptedAnimationController.CancelAnimation(ScriptedAnimationType.In);
        scriptedAnimationController.StartAnimation(ScriptedAnimationType.Out, _fadeSceneInCompleted);
    }

    private void Awake()
    {
        scriptedAnimationController = GetComponent<ScriptedAnimationController>();
    }

}
