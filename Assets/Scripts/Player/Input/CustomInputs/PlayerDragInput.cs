using System.Collections;
using UnityEngine;

public class PlayerDragInput : PlayerBaseInput
{
    [SerializeField] private float dragSpeed = 1;
    [SerializeField] private JoyStickUI joyStickUI;

    private Vector3 startDownPosition;
    private Vector3 currentDownPosition;
    private Vector3 deltaFromStartTouchPosition;
    private Vector3 deltaWithSpeed;
    private Coroutine dragUpdate;

    public override void Activate()
    {
        TargetPosition = Player.Instance.transform.position;
        TargetPosition = LookPositionOnPlane.Instance.ClampToPlane(TargetPosition);

        if (TargetPositionUpdatedEvent != null)
        {
            TargetPositionUpdatedEvent(TargetPosition);
        }

        base.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    protected override IEnumerator InputUpdate()
    {
        while(true)
        {
            if (PlatformBaseInput.Down) {
                startDownPosition = new Vector3(PlatformBaseInput.StartDownPosition.x, PlatformBaseInput.StartDownPosition.y, LookPositionOnPlane.Instance.transform.position.z);
                currentDownPosition = new Vector3(PlatformBaseInput.CurrentDownPosition.x, PlatformBaseInput.CurrentDownPosition.y, LookPositionOnPlane.Instance.transform.position.z);
                deltaFromStartTouchPosition = currentDownPosition - startDownPosition;

                Vector3 _deltaWithSpeed = deltaFromStartTouchPosition * dragSpeed;

                TargetPosition = Player.Instance.transform.position + _deltaWithSpeed;
                TargetPosition = LookPositionOnPlane.Instance.ClampToPlane(TargetPosition);
            }

            if (TargetPositionUpdatedEvent != null)
            {
                TargetPositionUpdatedEvent(TargetPosition);
            }

            yield return null;
        }
    }
}
