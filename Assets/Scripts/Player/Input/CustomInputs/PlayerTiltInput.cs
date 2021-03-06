﻿using System.Collections;
using UnityEngine;

/// <summary>
/// Contains the logic for tilt controls for the player.
/// </summary>
public class PlayerTiltInput : PlayerBaseInput
{
    [SerializeField] private float maxCameraRotation = -90;
    [SerializeField] private float minCameraRotation = 90;
    [SerializeField] private float tiltSpeed = 1;

    private bool planeHit;
    private Vector3 lookPositionOnPlane;
    private float gvrZRotation;
    private float rotationRange;
    private float currentRange;
    private float progress;
    private float tilt;

    protected override IEnumerator InputUpdate()
    {
        while (true)
        {
            gvrZRotation = Camera.main.transform.rotation.eulerAngles.z;
            rotationRange = Mathf.Abs(Mathf.DeltaAngle(minCameraRotation, maxCameraRotation));
            currentRange = Mathf.Abs(Mathf.DeltaAngle(minCameraRotation, gvrZRotation));
            progress = Mathf.InverseLerp(0, rotationRange, currentRange);
            tilt = Mathf.Lerp(-tiltSpeed, tiltSpeed, progress);

            TargetPosition.x = PlayerMovement.Position.x + tilt;

            lookPositionOnPlane = LookPositionOnPlane.Instance.GetLookPosition(out planeHit);
            if (planeHit)
            {
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