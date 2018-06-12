using System;

/// <summary>
/// Used to indicate which menu section is to the right and to the left of this menu section
/// </summary>
[Serializable]
public class MainMenuSection
{
    public MainMenuSectionType MainMenuSectionType;
    public bool LeftExists;
    public MainMenuSectionType LeftMainMenuSectionType;
    public bool RightExists;
    public MainMenuSectionType RightMainMenuSectionType;
}