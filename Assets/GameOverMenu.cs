using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    private void OnEnable()
    {
        PlayerCollision.PlayerDiedEvent += GameOver;
    }
    private void OnDisable()
    {
        PlayerCollision.PlayerDiedEvent -= GameOver;
    }

    private void GameOver()
    {
        menu.SetActive(true);   
    }
}
