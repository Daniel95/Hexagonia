using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneWindow : EditorWindow
{

    private const string BUILD_SCENE_PATH = "Assets/Scenes/";
    private const string SCENE_EXTENSION = ".unity";
    private const string AUTO_OPEN_KEY = "AutoOpenScene";

    private const int COLUMN_COUNT = 4;

    [MenuItem("Window/Scene Window")]
    private static void Init()
    {
        SceneWindow instance = GetWindow<SceneWindow>();
        instance.Show();
    }

    private void OnGUI()
    {
        int columnWidth = Mathf.FloorToInt((position.width - 30) / COLUMN_COUNT);

        List<Scenes> scenes = SceneHelper.Scenes;

        for (int l = 0; l < scenes.Count;)
        {
            GUILayout.BeginHorizontal();
            for (int i = 0; i < COLUMN_COUNT; i++)
            {
                Scenes scene = scenes[l];
                string sceneName = scene.ToString();

                bool isAutoOpened = PlayerPrefs.GetString(AUTO_OPEN_KEY, string.Empty) == sceneName;

                GUI.color = isAutoOpened ? Color.yellow : Color.white;

                if (GUILayout.Button(sceneName, GUILayout.Width(columnWidth)))
                {
                    PlayerPrefs.SetString(AUTO_OPEN_KEY, isAutoOpened ? string.Empty : sceneName);

                    if (Application.isPlaying)
                    {
                        SceneLoader.Instance.SwitchScene(scene);
                    }
                    else
                    {
                        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                        EditorSceneManager.OpenScene(BUILD_SCENE_PATH + sceneName + SCENE_EXTENSION);
                    }
                }
                l++;
                if (l >= scenes.Count) { break; }
            }
            GUILayout.EndHorizontal();
        }
    }
}