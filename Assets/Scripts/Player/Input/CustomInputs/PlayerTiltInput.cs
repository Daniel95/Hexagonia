using System.Collections;
using UnityEngine;

public class PlayerTiltInput : PlayerInputBase
{
    [SerializeField] private float tiltSpeed = 1;

    private float zRotation;
    private Coroutine tiltCoroutine;

    public override void Activate()
    {
        Input.gyro.enabled = true;
        tiltCoroutine = StartCoroutine(TiltUpdate());
        LookPositionOnPlane.LookPositionUpdatedEvent += TiltInput;
    }

    public override void Deactivate()
    {
        Input.gyro.enabled = false;
        StopCoroutine(tiltCoroutine);
        tiltCoroutine = null;
        LookPositionOnPlane.LookPositionUpdatedEvent -= TiltInput;
    }

    private void TiltInput(Vector3 _lookPosition)
    {
        TargetPosition.y = _lookPosition.y;
        TargetPosition.z = _lookPosition.z;

        TargetPosition = LookPositionOnPlane.Instance.ClampToPlane(TargetPosition);

        if(TargetPositionUpdatedEvent != null)
        {
            TargetPositionUpdatedEvent(TargetPosition);
        }
    }

    private IEnumerator TiltUpdate()
    {
        while(true)
        {
            zRotation += Input.gyro.rotationRateUnbiased.z;
            TargetPosition.x += -zRotation * tiltSpeed;

            TargetPosition = LookPositionOnPlane.Instance.ClampToPlane(TargetPosition);

            if (TargetPositionUpdatedEvent != null)
            {
                TargetPositionUpdatedEvent(TargetPosition);
            }
            yield return null;
        }
    }
}
