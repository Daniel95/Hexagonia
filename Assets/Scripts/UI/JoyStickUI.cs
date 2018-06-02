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
        InputBase.DraggingInputEvent += UpdateJoyStick;
        InputBase.DownInputEvent += OnDownInput;
        InputBase.UpInputEvent += OnUpInput;
    }

    public void Deactivate()
    {
        InputBase.DraggingInputEvent -= UpdateJoyStick;
        InputBase.DownInputEvent -= OnDownInput;
        InputBase.UpInputEvent -= OnUpInput;
        gameObject.SetActive(false);
    }

    private void OnDownInput(Vector2 _position)
    {
        gameObject.SetActive(true);
    }

    private void OnUpInput(Vector2 _position)
    {
        //gameObject.SetActive(false);
    }

    private void UpdateJoyStick(Vector2 _position, Vector2 _delta)
    {
        start.position = InputBase.StartDownPosition;
        uiLineRenderer.Points[0] = start.position;
        end.position = _position;
        uiLineRenderer.Points[1] = end.position;
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
