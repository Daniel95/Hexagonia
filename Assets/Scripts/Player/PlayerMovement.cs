using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float maxSpeed = 0.3f;

    private void UpdateTargetPosition(Vector3 _targetPosition)
    {
        if (transform.position == _targetPosition)
            return;

        transform.position = _targetPosition;



        //Vector3 offset = transform.position - pointOnPlane;
        //Vector3 direction = offset.normalized;
        /*
        Vector3 offset = _targetPosition - transform.position;

        Vector3 move = VectorHelper.Clamp(offset, -maxSpeed, maxSpeed);

        transform.Translate(move);

        //transform.DOMove(_targetPosition, 90).SetEase(Ease.InElastic);
        */
    }

    private void OnEnable()
    {
        MovementPlane.RaycastPointOnPlaneEvent += UpdateTargetPosition;
    }

    private void OnDisable()
    {
        MovementPlane.RaycastPointOnPlaneEvent -= UpdateTargetPosition;
    }

}
