using System;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get { return GetInstance(); } }

    /// <summary>
    /// Old scene, New Scene
    /// </summary>
    public static Action<Scenes?, Scenes> SceneSwitchStartedEvent;
    public static Action<Scenes?, Scenes> SceneSwitchCompletedEvent;

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

    [SerializeField] private Scenes startScene;

    private Scenes? currentScene;

    public void SwitchScene(Scenes _newScene)
    {
        if (startScene == Scenes.Default)
        {
            Debug.LogWarning("Cannot switch to the Default scene.");
            return;
        }

        Scenes? _previousScene = currentScene;
        currentScene = _newScene;

        if (_previousScene != null)
        {
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

                        DefaultSceneUI.Instance.FadeSceneIn();
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
                DefaultSceneUI.Instance.FadeSceneIn();
            });
        }
    }

    private void Start()
    {
        if(startScene == Scenes.Default)
        {
            Debug.LogWarning("Cannot load the Default scene as start scene.");
            return;
        }

        SwitchScene(startScene);
    }

}
