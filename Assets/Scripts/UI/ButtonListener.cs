using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ButtonListenerView : MonoBehaviour
{

    private Button button;

    protected abstract void OnButtonClick();

    private void OnEnable()
    {
        GetButton();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }

    private Button GetButton()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
        return button;
    }

}
