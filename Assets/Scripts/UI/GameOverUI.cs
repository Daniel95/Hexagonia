using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    private void OnEnable()
    {
        Player.DiedEvent += Activate;
    }

    private void OnDisable()
    {
        Player.DiedEvent -= Activate;
    }

    private void Activate()
    {
        menu.SetActive(true);   
    }

}
