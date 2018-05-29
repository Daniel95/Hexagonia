using System;
using UnityEngine;

/// <summary>
/// Loads and Unload scenes.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static Scenes CurrentScene
    {
        get { return (Scenes) currentScene; }
    }

    public static SceneLoader Instance
    {
        get { return GetInstance(); }
    }

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

    /// <summary>
    /// Parameters: Old Scene, New Scene
    /// </summary>
    public static Action<Scenes, Scenes> SceneSwitchCompletedEvent;

    public static Action<Scenes, Scenes> SceneSwitchStartedEvent;

    [SerializeField] private Scenes startScene;

    private static Scenes? currentScene;

    /// <summary>
    /// Unloads the current scene (except the Default scene), and loads the new scene.
    /// </summary>
    /// <param name="_newScene"></param>
    public void SwitchScene(Scenes _newScene)
    {
        if (startScene == Scenes.Default)
        {
            Debug.LogWarning("Cannot switch to the Default scene.");
            return;
        }

        Scenes? _previousScene = currentScene;
        currentScene = _newScene;

        if (SceneSwitchStartedEvent != null)
        {
            SceneSwitchStartedEvent((Scenes) _previousScene, _newScene);
        }

        if (_previousScene != null)
        {
            SceneHelper.UnloadSceneOverTime(_previousScene.ToString(), () => SceneHelper.LoadSceneOverTime(
                _newScene.ToString(), () =>
                {
                    if (SceneSwitchCompletedEvent != null)
                    {
                        SceneSwitchCompletedEvent((Scenes) _previousScene, _newScene);
                    }
                }));
        }
        else
        {
            SceneHelper.LoadSceneOverTime(_newScene.ToString(), () =>
            {
                Debug.Log(SceneSwitchCompletedEvent);
                if (SceneSwitchCompletedEvent != null)
                {
                    SceneSwitchCompletedEvent((Scenes) _previousScene, _newScene);
                }
            });
        }
    }

    private void Awake()
    {
        if (startScene == Scenes.Default)
        {
            Debug.LogWarning("Cannot load the Default scene as start scene.");
            return;
        }

        currentScene = startScene;
        SceneHelper.LoadSceneOverTime(startScene.ToString());
    }
}