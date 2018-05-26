using System;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    //Parameters: Old scene, New Scene
    public static Action<Scenes?, Scenes> SceneSwitchStartedEvent;
    public static Action<Scenes?, Scenes> SceneSwitchCompletedEvent;

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
            DefaultSceneUI.Instance.FadeSceneOut(() =>
            {
                Debug.Log("FadeSceneOut completed");
                SceneHelper.UnloadSceneOverTime(_previousScene.ToString(), () => 
                {
                    SceneHelper.LoadSceneOverTime(_newScene.ToString(), () =>
                    {
                        Debug.Log("loaded " + _newScene.ToString());

                        DefaultSceneUI.Instance.FadeSceneIn(() =>
                        {
                            Debug.Log("FadeSceneIn completed");
                            if (SceneSwitchCompletedEvent != null)
                            {
                                SceneSwitchCompletedEvent(_previousScene, _newScene);
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
                Debug.Log("loaded " + _newScene.ToString());

                DefaultSceneUI.Instance.FadeSceneIn(() =>
                {
                    Debug.Log("FadeSceneIn completed");

                    if (SceneSwitchCompletedEvent != null)
                    {
                        SceneSwitchCompletedEvent(_previousScene, _newScene);
                    }
                });
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
