using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneLoaderButton : GazeButton
{
    [SerializeField] private Scenes scene;

    protected override void OnGazeFilled()
    {
        OnClick();
    }

    public void OnClick()
    {
        SceneLoader.Instance.SwitchScene(scene);
    }
}