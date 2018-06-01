using UnityEngine;

public class PlayerDragInput : PlayerInputBase
{
    [SerializeField] private float dragSpeed = 1;
    [SerializeField] private JoyStickUI joyStickUI;

    public override void Activate()
    {
        TargetPoint = LookPositionOnPlane.Instance.transform.position;
        InputBase.DraggingInputEvent += DragInput;
    }

    public override void Deactivate()
    {
        InputBase.DraggingInputEvent -= DragInput;
    }

    private void DragInput(Vector2 _dragPosition, Vector2 _delta)
    {
        Vector2 _deltaSinceTouched = _dragPosition - InputBase.StartDownPosition;

        TargetPoint += ((Vector3)_deltaSinceTouched * dragSpeed) * Time.deltaTime;
        TargetPoint = LookPositionOnPlane.Instance.ClampToPlane(TargetPoint);
    }
}
