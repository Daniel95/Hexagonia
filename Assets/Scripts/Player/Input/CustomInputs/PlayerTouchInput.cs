using System.Collections;
using UnityEngine;

public class PlayerTouchInput : PlayerBaseInput
{
    public override void Activate()
    {
        TargetPosition = LookPositionOnPlane.Instance.transform.position;

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
        while (true)
        {
            TargetPosition = Camera.main.ScreenToWorldPoint(new Vector3(PlatformBaseInput.CurrentDownPosition.x, PlatformBaseInput.CurrentDownPosition.y, LookPositionOnPlane.Instance.transform.position.z));
            TargetPosition = LookPositionOnPlane.Instance.ClampToPlane(TargetPosition);

            if (TargetPositionUpdatedEvent != null)
            {
                TargetPositionUpdatedEvent(TargetPosition);
            }

            yield return null;
        }
    }
}
