using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    private void OnEnable()
    {
        Player.PlayerDiedEvent += GameOver;
    }

    private void OnDisable()
    {
        Player.PlayerDiedEvent -= GameOver;
    }

    private void GameOver()
    {
        menu.SetActive(true);   
    }
}
