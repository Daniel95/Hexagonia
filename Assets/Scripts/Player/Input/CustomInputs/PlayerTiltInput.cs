using UnityEngine;
using System.Collections;

public class PlayerTiltInput : PlayerInputBase
{
    [SerializeField] private float tiltSpeed = 1;

    private float zRotation;

    public override void Activate()
    {
        Input.gyro.enabled = true;
        LookPositionOnPlane.LookPositionUpdatedEvent += TiltInput;
    }

    public override void Deactivate()
    {
        Input.gyro.enabled = false;
        LookPositionOnPlane.LookPositionUpdatedEvent -= TiltInput;
    }

    private void TiltInput(Vector3 _lookPosition)
    {
        zRotation += Input.gyro.rotationRate.z;

        TargetPoint.x += -zRotation * tiltSpeed;
        TargetPoint.y = _lookPosition.y;
        TargetPoint.z = _lookPosition.z;

        TargetPoint = LookPositionOnPlane.Instance.ClampToPlane(TargetPoint);
    }
}
