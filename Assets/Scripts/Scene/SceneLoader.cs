using System;
using UnityEngine;

/// <summary>
/// Can switch from scene to scene using async loading. Also plays an scen fade in/out animation when switching.
/// The default scene should always be active so that scene is never loaded/unloaded.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get { return GetInstance(); } }
    public static Scenes CurrentScene { get { return (Scenes)currentScene; } }

    /// <summary>
    /// Old scene, New Scene
    /// </summary>
    public static Action<Scenes?, Scenes> SceneSwitchStartedEvent;
    public static Action<Scenes?, Scenes> SceneSwitchCompletedEvent;
    public static Action FadeSceneOutStartedEvent;
    public static Action FadeSceneInStartedEvent;
    public static Action FadeSceneInCompletedEvent;

    #region Singleton
    private static SceneLoader instance;

    private static SceneLoader GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<SceneLoader>();
        }
        return instance;
    }
    #endregion

    private static Scenes? currentScene;

    [SerializeField] private Scenes startScene;

    /// <summary>
    /// Load the scene indicated by the startScene assigned in the editor.
    /// </summary>
    /// <param name="_newScene"></param>
    public void LoadStartScene()
    {
        if (startScene == Scenes.Default)
        {
            Debug.LogWarning("Cannot load the Default scene as start scene.");
            return;
        }

        SwitchScene(startScene);
    }

    /// <summary>
    /// Switch to the new scene.
    /// </summary>
    /// <param name="_newScene"></param>
    public void SwitchScene(Scenes _newScene)
    {
        if (_newScene == Scenes.Default)
        {
            Debug.LogWarning("Cannot switch to the Default scene.");
            return;
        }

        Scenes? _previousScene = currentScene;
        currentScene = _newScene;

        if (_previousScene != null)
        {
            if (FadeSceneOutStartedEvent != null)
            {
                FadeSceneOutStartedEvent();
            }
            DefaultSceneUI.Instance.FadeSceneOut(() =>
            {
                if (SceneSwitchStartedEvent != null)
                {
                    SceneSwitchStartedEvent((Scenes)_previousScene, _newScene);
                }

                SceneHelper.UnloadSceneOverTime(_previousScene.ToString(), () => 
                {
                    SceneHelper.LoadSceneOverTime(_newScene.ToString(), () =>
                    {
                        if (SceneSwitchCompletedEvent != null)
                        {
                            SceneSwitchCompletedEvent(_previousScene, _newScene);
                        }

                        DefaultSceneUI.Instance.FadeSceneIn(() =>
                        {
                            if (FadeSceneInCompletedEvent != null)
                            {
                                FadeSceneInCompletedEvent();
                            }
                        });
                    });
                });
            });
        }
        else
        {
            SceneHelper.LoadSceneOverTime(_newScene.ToString(), () =>
            {
                if (SceneSwitchCompletedEvent != null)
                {
                    SceneSwitchCompletedEvent(_previousScene, _newScene);
                }

                if (FadeSceneInStartedEvent != null)
                {
                    FadeSceneInStartedEvent();
                }

                DefaultSceneUI.Instance.FadeSceneIn(() =>
                {
                    if (FadeSceneInCompletedEvent != null)
                    {
                        FadeSceneInCompletedEvent();
                    }
                });
            });
        }
    }
}