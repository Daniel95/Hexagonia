using System.Collections;
using UnityEngine;

public class PlayerDragInput : PlayerBaseInput
{
    [SerializeField] private float dragSpeed = 1;
    [SerializeField] private JoyStickUI joyStickUI;

    private Vector2 screenCenter;

    public override void Activate()
    {
        PlatformBaseInput.UpInputEvent += OnUpInput;

        TargetPosition = LookPositionOnPlane.Instance.transform.position;

        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        if (TargetPositionUpdatedEvent != null)
        {
            TargetPositionUpdatedEvent(TargetPosition);
        }

        joyStickUI.Activate();
        base.Activate();
    }

    public override void Deactivate()
    {
        PlatformBaseInput.UpInputEvent -= OnUpInput;
        joyStickUI.Deactivate();
        base.Deactivate();
    }

    protected override IEnumerator InputUpdate()
    {
        while(true)
        {
            Vector2 _deltaFromStartTouchPosition = PlatformBaseInput.CurrentDownPosition - PlatformBaseInput.StartDownPosition * dragSpeed;
            Vector3 _targetScreenPosition = screenCenter + _deltaFromStartTouchPosition;
            TargetPosition = Camera.main.ScreenToWorldPoint(new Vector3(_targetScreenPosition.x, _targetScreenPosition.y, LookPositionOnPlane.Instance.transform.position.z));

            TargetPosition = LookPositionOnPlane.Instance.ClampToPlane(TargetPosition);

            if (TargetPositionUpdatedEvent != null)
            {
                TargetPositionUpdatedEvent(TargetPosition);
            }

            yield return null;
        }
    }

    private void OnUpInput(Vector2 _inputPosition)
    {
        TargetPosition = screenCenter;
    }
}
