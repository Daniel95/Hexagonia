using System.Collections;
using UnityEngine;

public class PlayerDragInput : PlayerBaseInput
{
    [SerializeField] private float dragSpeed = 1;
    [SerializeField] private JoyStickUI joyStickUI;

    private Vector3 startDownWorldPosition;
    private Vector3 currentDownPositionWorldPosition;
    private Vector3 deltaFromStartTouchPosition;
    private Vector3 deltaWithSpeed;

    public override void Activate()
    {
        TargetPosition = Player.Instance.transform.position;
        if (TargetPositionUpdatedEvent != null)
        {
            TargetPositionUpdatedEvent(TargetPosition);
        }

        joyStickUI.Activate();
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
            if (PlatformBaseInput.Down) {
                startDownWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(PlatformBaseInput.StartDownPosition.x, PlatformBaseInput.StartDownPosition.y, LookPositionOnPlane.Instance.transform.position.z));
                currentDownPositionWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(PlatformBaseInput.CurrentDownPosition.x, PlatformBaseInput.CurrentDownPosition.y, LookPositionOnPlane.Instance.transform.position.z));
                deltaFromStartTouchPosition = currentDownPositionWorldPosition - startDownWorldPosition;

                Vector3 _deltaWithSpeed = deltaFromStartTouchPosition * dragSpeed;

                TargetPosition = Player.Instance.transform.position + _deltaWithSpeed;
                TargetPosition = LookPositionOnPlane.Instance.ClampToPlane(TargetPosition);
            }

            if (TargetPositionUpdatedEvent != null)
            {
                TargetPositionUpdatedEvent(TargetPosition);
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
