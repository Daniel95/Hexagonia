using UnityEngine;

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
        TargetPosition = _lookPosition;
        if(TargetPositionUpdatedEvent != null)
        {
            TargetPositionUpdatedEvent(TargetPosition);
        }
    }
}
