using System.Collections;
using UnityEngine;

public class PlayerDragInput : PlayerBaseInput
{
    [SerializeField] private float dragSpeed = 1;
    [SerializeField] private JoyStickUI joyStickUI;

    private Vector2 startInputPosition;

    public override void Activate()
    {
        TargetPosition = LookPositionOnPlane.Instance.transform.position;

        if (TargetPositionUpdatedEvent != null)
        {
            TargetPositionUpdatedEvent(TargetPosition);
        }

        joyStickUI.Activate();

        Application.targetFrameRate = 60;

        base.Activate();
    }

    public override void Deactivate()
    {
        joyStickUI.Deactivate();
        base.Deactivate();
    }

    protected override IEnumerator InputUpdate()
    {
        while(true)
        {
            //Vector2 _currentInputPosition = Camera.main.ScreenToViewportPoint(InputBase.CurrentDownPosition);
            //Vector2 _startInputPosition = Camera.main.ScreenToViewportPoint(InputBase.StartDownPosition);

            Vector2 _deltaFromStartTouchPosition = PlatformBaseInput.CurrentDownPosition - PlatformBaseInput.StartDownPosition;

            TargetPosition += ((Vector3)_deltaFromStartTouchPosition * dragSpeed) * Time.deltaTime;

            TargetPosition = LookPositionOnPlane.Instance.ClampToPlane(TargetPosition);

            if (TargetPositionUpdatedEvent != null)
            {
                TargetPositionUpdatedEvent(TargetPosition);
            }

            yield return null;
        }
    }
}
