using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button that can load a new scene.
/// </summary>
//[RequireComponent(typeof(Button))]
public class SceneLoaderButton : GazeButton
{
    [SerializeField] private Scenes scene;

    /// <summary>
    /// Called when the button is activated.
    /// </summary>
    protected override void OnTrigger()
    {
        SceneLoader.Instance.SwitchScene(scene);
    }
}