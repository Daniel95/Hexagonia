using UnityEngine;

/// <summary>
/// Controls game over UI.
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    private void OnEnable()
    {
        DyingPlayer.AnimationEnd += Activate;
    }

    private void OnDisable()
    {
        DyingPlayer.AnimationEnd -= Activate;
    }

    private void Activate()
    {
        menu.SetActive(true);   
    }
}
