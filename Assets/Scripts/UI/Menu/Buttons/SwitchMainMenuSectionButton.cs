using UnityEngine;

/// <summary>
/// Calls the MainMenuRotator to rotate to the targetMainMenuSectionType.
/// </summary>
public class SwitchMainMenuSectionButton : GazeButton
{
    [SerializeField] private MainMenuSectionType targetMainMenuSectionType;

    protected override void OnTrigger()
    {
        MainMenuRotator.Instance.RotateToSection(targetMainMenuSectionType);
    }
}