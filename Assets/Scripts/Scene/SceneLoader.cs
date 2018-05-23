using System;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    //Parameters: Old scene, New Scene
    public static Action<Scenes, Scenes> SceneSwitchCompletedEvent;
    public static Action<Scenes, Scenes> SceneSwitchStartedEvent;

    public static SceneLoader Instance { get { return GetInstance(); } }

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
        Debug.Log("Calling switchscene");
        if (startScene == Scenes.Default)
        {
            Debug.LogWarning("Cannot switch to the Default scene.");
            return;
        }

        Scenes? _previousScene = currentScene;
        currentScene = _newScene;

        if (SceneSwitchStartedEvent != null)
        {
            SceneSwitchStartedEvent((Scenes)_previousScene, _newScene);
        }

        if (_previousScene != null)
        {
            SceneHelper.UnloadSceneOverTime(_previousScene.ToString(), () => SceneHelper.LoadSceneOverTime(_newScene.ToString()));
        }
        else
        {
            SceneHelper.LoadSceneOverTime(_newScene.ToString(), () =>
            {
                if (SceneSwitchCompletedEvent != null)
                {
                    SceneSwitchCompletedEvent((Scenes)_previousScene, _newScene);
                }
            });
        }
    }

    private void Awake()
    {
        if(startScene == Scenes.Default)
        {
            Debug.LogWarning("Cannot load the Default scene as start scene.");
            return;
        }

        currentScene = startScene;
        SceneHelper.LoadSceneOverTime(startScene.ToString());
    }

}
