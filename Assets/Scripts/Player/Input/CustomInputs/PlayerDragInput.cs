using UnityEngine;

public class PlayerDragInput : PlayerInputBase
{
    [SerializeField] private float dragSpeed = 1;
    [SerializeField] private JoyStickUI joyStickUI;

    public override void Activate()
    {
        TargetPosition = LookPositionOnPlane.Instance.transform.position;
        InputBase.DraggingInputEvent += DragInput;

        joyStickUI.Activate();
    }

    public override void Deactivate()
    {
        InputBase.DraggingInputEvent -= DragInput;
        joyStickUI.Deactivate();
    }

    private void DragInput(Vector2 _dragPosition, Vector2 _delta)
    {
        Vector2 _deltaFromStartTouchPosition = _dragPosition - InputBase.StartDownPosition;

        TargetPosition += ((Vector3)_deltaFromStartTouchPosition * dragSpeed) * Time.deltaTime;
        TargetPosition = LookPositionOnPlane.Instance.ClampToPlane(TargetPosition);

        if (TargetPositionUpdatedEvent != null)
        {
            TargetPositionUpdatedEvent(TargetPosition);
        }
    }
}
