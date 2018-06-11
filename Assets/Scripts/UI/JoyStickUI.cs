using UnityEngine;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(UILineRenderer))]
public class JoyStickUI : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] private bool startDisabled;

    private UILineRenderer uiLineRenderer;

    public void Activate()
    {
        PlatformBaseInput.InputEvent += UpdateJoyStick;
        PlatformBaseInput.DownInputEvent += OnDownInput;
        PlatformBaseInput.UpInputEvent += OnUpInput;
    }

    public void Deactivate()
    {
        PlatformBaseInput.InputEvent -= UpdateJoyStick;
        PlatformBaseInput.DownInputEvent -= OnDownInput;
        PlatformBaseInput.UpInputEvent -= OnUpInput;
        gameObject.SetActive(false);
    }

    private void OnDownInput(Vector2 _position)
    {
        gameObject.SetActive(true);
    }

    private void OnUpInput(Vector2 _position)
    {
        gameObject.SetActive(false);
    }

    private void UpdateJoyStick(Vector2 _position)
    {
        start.position = Camera.main.ViewportToScreenPoint(PlatformBaseInput.StartDownPosition);
        uiLineRenderer.Points[0] = start.localPosition;
        end.position = Camera.main.ViewportToScreenPoint(_position);
        uiLineRenderer.Points[1] = end.localPosition;
        uiLineRenderer.LineThickness = uiLineRenderer.LineThickness;
    }

    private void Awake()
    {
        uiLineRenderer = GetComponent<UILineRenderer>();
        uiLineRenderer.Points = new Vector2[2];
        if (startDisabled)
        {
            gameObject.SetActive(false);
        }
    }
}
