using System.Collections;
using UnityEngine;

public class PlayerTiltInput : PlayerBaseInput
{
    [SerializeField] private float maxCameraRotation = -90;
    [SerializeField] private float minCameraRotation = 90;
    [SerializeField] private float tiltSpeed = 1;
    [SerializeField] [Range(-90, 90)] private float testRotation = 20;

    private float zRotation;
    private Coroutine tiltCoroutine;
    private bool planeHit;
    private Vector3 lookPositionOnPlane;
    private Transform cameraTransform;

    public override void Activate()
    {
        Input.gyro.enabled = true;
        cameraTransform = Camera.main.gameObject.transform;
        base.Activate();
    }

    public override void Deactivate()
    {
        Input.gyro.enabled = false;
        base.Deactivate();
    }

    protected override IEnumerator InputUpdate()
    {
        while(true)
        {
            //float _cameraZRotation = testRotation;
            float _cameraZRotation = cameraTransform.rotation.eulerAngles.z;
            float _rotationRange = maxCameraRotation - minCameraRotation;
            float _progress = (_cameraZRotation - minCameraRotation) / _rotationRange;
            float _tilt = Mathf.Lerp(-tiltSpeed, tiltSpeed, _progress);

            TargetPosition.x = Player.Instance.transform.position.x + _tilt;

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
