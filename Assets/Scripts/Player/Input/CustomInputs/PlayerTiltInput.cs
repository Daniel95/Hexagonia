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

    public override void Activate()
    {
        Input.gyro.enabled = true;
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
            Debug.Log(Input.acceleration.x);

            float _acceleration = Input.acceleration.x * tiltSpeed;
            TargetPosition.x = Player.Instance.transform.position.x + _acceleration;

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

    /*
    private void Update()
    {
        Input.gyro.enabled = true;
        Quaternion referenceRotation = Quaternion.identity;
        Quaternion deviceRotation = GyroHelper.Get();
        Quaternion eliminationOfXY = Quaternion.Inverse(
            Quaternion.FromToRotation(referenceRotation * Vector3.forward,
                                      deviceRotation * Vector3.forward)
        );
        Quaternion rotationZ = eliminationOfXY * deviceRotation;
        float roll = rotationZ.eulerAngles.z;
        Debug.Log("________________");
        Debug.Log("gyro roll " + roll);
        Debug.Log("acceleration " + Input.acceleration.x);
        Debug.Log("Camera rotation " + Camera.main.transform.localRotation.eulerAngles);
    }
    */
}
