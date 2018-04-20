﻿using UnityEngine;

public class CrossHairMovement : MonoBehaviour
{

    [SerializeField] private Vector3 offset;

    private void UpdateTargetPosition(Vector3 _targetPosition)
    {
        transform.position = _targetPosition;
    }

    private void OnEnable()
    {
        PlaneMovement.MovePointOnPlaneEvent += UpdateTargetPosition;
    }

    private void OnDisable()
    {
        PlaneMovement.MovePointOnPlaneEvent -= UpdateTargetPosition;
    }

}
