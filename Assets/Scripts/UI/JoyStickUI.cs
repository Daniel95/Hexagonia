using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class JoyStickUI : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] private bool startDisabled;

    private LineRenderer lineRenderer;

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
        gameObject.SetActive(true);
    }

    private void OnDownInput(Vector2 _position)
    {
        gameObject.SetActive(true);
    }

    private void OnUpInput(Vector2 _position)
    {
        gameObject.SetActive(false);
    }

    private void UpdateJoyStick(Vector2 _position, Vector2 _delta)
    {
        start.position = InputBase.StartDownPosition;
        lineRenderer.SetPosition(0, Camera.main.ScreenToWorldPoint(new Vector3(start.position.x, start.position.y, 1)));
        end.position = _position;
        lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(new Vector3(end.position.x, end.position.y, 1)));
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        if (startDisabled)
        {
            gameObject.SetActive(false);
        }
    }
}
