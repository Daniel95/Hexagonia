using System.Collections;
using UnityEngine;

public class PlayerTouchInput : PlayerBaseInput
{
    [SerializeField] private Vector3 offset;

    private Vector3 touchPosition;

    public override void Activate()
    {
        TargetPosition = LookPositionOnPlane.Instance.transform.position;

        if (TargetPositionUpdatedEvent != null)
        {
            TargetPositionUpdatedEvent(TargetPosition);
        }

        base.Activate();
    }

    protected override IEnumerator InputUpdate()
    {
        while (true)
        {
            if(PlatformBaseInput.Down)
            {
                touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(PlatformBaseInput.CurrentDownPosition.x, PlatformBaseInput.CurrentDownPosition.y, LookPositionOnPlane.Instance.transform.position.z));

                TargetPosition = touchPosition + offset;
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
