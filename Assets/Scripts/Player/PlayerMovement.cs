using System;
using UnityEngine;

/// <summary>
/// Handles the playermovement based on the SmoothPlaneMovement parent.
/// </summary>
public class PlayerMovement : SmoothPlaneMovement
{
    /// <summary>
    /// The current position of the player.
    /// </summary>
    public static Vector3 Position { get { return GetInstance().transform.position; } }

    /// <summary>
    /// Parameters: Position, Delta
    /// </summary>
    public static Action<Vector3, Vector3> OnMoved;

    #region Singleton
    private static PlayerMovement instance;

    private static PlayerMovement GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<PlayerMovement>();
        }
        return instance;
    }
    #endregion

    protected override void MoveToTargetPosition(Vector3 _targetPosition)
    {
        base.MoveToTargetPosition(_targetPosition);

        if (OnMoved != null)
        {
            OnMoved(transform.position, Delta);
        }
    }
}