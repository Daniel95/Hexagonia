using UnityEngine;

/// <summary>
/// Allows the user to quit the application.
/// </summary>
public class BackHandler : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void Awake()
    {
        Input.backButtonLeavesApp = true;
    }
}