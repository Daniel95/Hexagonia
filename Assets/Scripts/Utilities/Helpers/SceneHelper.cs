using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class SceneHelper
{

    public static List<Scenes> Scenes;

    static SceneHelper()
    {
        Array sceneArray = Enum.GetValues(typeof(Scenes));
        Scenes = sceneArray.Cast<Scenes>().ToList();
    }

    public static void LoadSceneOverTime(string sceneName, Action onLoaded = null)
    {
        CoroutineHelper.Start(LoadAdditive(sceneName, onLoaded));
    }

    public static void UnloadSceneOverTime(string sceneName, Action onLoaded = null)
    {
        CoroutineHelper.Start(UnloadAdditive(sceneName, onLoaded));
    }

    private static IEnumerator LoadAdditive(string sceneName, Action onLoaded = null)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);

        if (onLoaded != null)
        {
            onLoaded();
        }
    }

    private static IEnumerator UnloadAdditive(string sceneName, Action onDone = null)
    {
        yield return SceneManager.UnloadSceneAsync(sceneName);

        Scene scene = SceneManager.GetSceneByName(global::Scenes.Default.ToString());
        SceneManager.SetActiveScene(scene);

        GC.Collect();
        Resources.UnloadUnusedAssets();
        GC.Collect();

        if (onDone != null)
        {
            onDone();
        }
    }

}
