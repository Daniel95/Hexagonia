using System;
using UnityEngine;

/// <summary>
///  UpdateTargetPosition gets updated everytime the method gets called with the event: LookPositionUpdateEvent in LookPositionPlane.
///  It Smoothly moves the player when moving around in the game.
/// </summary>
public class PlayerMovement : SmoothPlaneMovement
{
    public static Action<Vector3> OnMoved;

    protected override void UpdateTargetPosition(Vector3 _targetPosition)
    {
        base.UpdateTargetPosition(_targetPosition);

        if (OnMoved != null)
        {
            OnMoved(transform.position);
        }
    }
}