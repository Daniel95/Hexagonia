using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    private void OnEnable()
    {
        Player.PlayerDiedEvent += Activate;
    }

    private void OnDisable()
    {
        Player.PlayerDiedEvent -= Activate;
    }

    private void Activate()
    {
        menu.SetActive(true);   
    }

}
