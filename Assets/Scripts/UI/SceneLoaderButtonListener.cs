using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneLoaderButtonListener : MonoBehaviour
{

    [SerializeField] private Scenes scene;

    private string levelName;

    private Button button;

    private void OnClick()
    {
        SceneLoader.Instance.SwitchScene(scene);
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }

    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnClick);
    }

}
