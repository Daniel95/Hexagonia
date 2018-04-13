using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Vector3 targetPosition;

    private void Update()
    {
        if (transform.position == targetPosition)
            return;

        transform.DOMove(targetPosition, 1).SetEase(Ease.OutElastic);
    }

    private void UpdateTargetPosition(Vector3 _targetPosition)
    {
        targetPosition = _targetPosition;
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
