using UnityEngine;

public class SceneLoader : MonoBehaviour {

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

    public void SwitchScene(Scenes newScene)
    {
        if (startScene == Scenes.Default)
        {
            Debug.LogWarning("Cannot switch to the Default scene.");
            return;
        }

        Scenes? previousScene = currentScene;
        currentScene = newScene;

        if (previousScene != null)
        {
            SceneHelper.UnloadSceneOverTime(previousScene.ToString(), () => SceneHelper.LoadSceneOverTime(newScene.ToString()));
        }
        else
        {
            SceneHelper.LoadSceneOverTime(newScene.ToString());
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
