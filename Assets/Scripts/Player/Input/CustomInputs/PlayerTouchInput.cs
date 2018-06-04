using System.Collections;
using UnityEngine;

public class PlayerTouchInput : PlayerBaseInput
{
    [SerializeField] private Vector3 offset;

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
            Vector3 _touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(PlatformBaseInput.CurrentDownPosition.x, PlatformBaseInput.CurrentDownPosition.y, LookPositionOnPlane.Instance.transform.position.z));

            TargetPosition = _touchPosition + offset;
            TargetPosition = LookPositionOnPlane.Instance.ClampToPlane(TargetPosition);

            if (TargetPositionUpdatedEvent != null)
            {
                TargetPositionUpdatedEvent(TargetPosition);
            }

            yield return null;
        }
    }
}
