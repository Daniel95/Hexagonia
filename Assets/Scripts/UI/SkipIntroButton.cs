using UnityEngine;

/// <summary>
/// Button that skips the Intro cutscene when pressed
/// </summary>
public class SkipIntroButton : GazeButton {

    [SerializeField] private Scenes scene;

    protected override void OnTrigger()
    {
        SceneLoader.Instance.SwitchScene(scene);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PlatformBaseInput.DownInputEvent += OnInput;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlatformBaseInput.DownInputEvent -= OnInput;
    }

    private void OnInput(Vector2 _position)
    {
        TryToTrigger();
    }
}