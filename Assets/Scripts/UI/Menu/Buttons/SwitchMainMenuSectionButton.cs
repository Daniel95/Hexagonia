using UnityEngine;

public class SwitchMainMenuSectionButton : GazeButton
{
    [SerializeField] private MainMenuSectionType targetMainMenuSectionType;

    protected override void OnTrigger()
    {
        MainMenuRotator.Instance.RotateToSection(targetMainMenuSectionType);
    }
}
