using UnityEngine;
using System.Collections;

public class PlayerLookInput : PlayerInputBase
{
    public override void Activate()
    {
        LookPositionOnPlane.LookPositionUpdatedEvent += LookInput;
    }

    public override void Deactivate()
    {
        LookPositionOnPlane.LookPositionUpdatedEvent -= LookInput;
    }

    private void LookInput(Vector3 _lookPosition)
    {
        TargetPoint = _lookPosition;
    }
}
