using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button that can load a new scene.
/// </summary>
[RequireComponent(typeof(Button))]
public class SceneLoaderButton : GazeButton
{
    [SerializeField] private Scenes scene;

    /// <summary>
    /// Called when the button is activated.
    /// </summary>
    public void OnClick()
    {
        SceneLoader.Instance.SwitchScene(scene);
    }

    protected override void OnGazeFilled()
    {
        OnClick();
    }
}