using System.Collections;

public class PlayerLookInput : PlayerBaseInput
{
    protected override IEnumerator InputUpdate()
    {
        bool _hit;

        while (true)
        {
            TargetPosition = LookPositionOnPlane.Instance.GetLookPosition(out _hit);
            if (_hit && TargetPositionUpdatedEvent != null)
            {
                TargetPositionUpdatedEvent(TargetPosition);
            }
            yield return null;
        }
    }
}
