using UnityEngine;

/// <summary>
/// Controls game over UI.
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    private void OnEnable()
    {
        DyingPlayer.AnimationEndEvent += Activate;
        Player.DiedEvent += RecenterUI;
    }

    private void OnDisable()
    {
        DyingPlayer.AnimationEndEvent -= Activate;
        Player.DiedEvent -= RecenterUI;
    }

    private void Activate()
    {
        menu.SetActive(true);
    }

    private void RecenterUI()
    {
        menu.transform.position = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y, menu.transform.position.z);
    }
}
