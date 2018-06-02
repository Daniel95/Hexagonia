using System.Collections;
using UnityEngine;

public class PlayerTiltInput : PlayerInputBase
{
    [SerializeField] private float tiltSpeed = 1;

    private float zRotation;
    private Coroutine tiltCoroutine;
    private bool planeHit;
    private Vector3 lookPositionOnPlane;

    public override void Activate()
    {
        Input.gyro.enabled = true;
        tiltCoroutine = StartCoroutine(TiltUpdate());
    }

    public override void Deactivate()
    {
        Input.gyro.enabled = false;
        StopCoroutine(tiltCoroutine);
    }

    private IEnumerator TiltUpdate()
    {
        while(true)
        {
            zRotation += Input.gyro.rotationRateUnbiased.z;
            TargetPosition.x += -zRotation * tiltSpeed;

            lookPositionOnPlane = LookPositionOnPlane.Instance.GetLookPosition(out planeHit);
            if (planeHit) {
                TargetPosition.y = lookPositionOnPlane.y;
                TargetPosition.z = lookPositionOnPlane.z;
            }

            TargetPosition = LookPositionOnPlane.Instance.ClampToPlane(TargetPosition);

            if (TargetPositionUpdatedEvent != null)
            {
                TargetPositionUpdatedEvent(TargetPosition);
            }
            yield return null;
        }
    }
}
