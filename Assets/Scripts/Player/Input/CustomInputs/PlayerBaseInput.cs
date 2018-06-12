using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Base inputclass that provides an input update loop.
/// </summary>
public abstract class PlayerBaseInput //: MonoBehaviour
{
    public Action<Vector3> TargetPositionUpdatedEvent; 
    public PlayerInputType PlayerInputType { get { return playerInputType; } }

    protected Vector3 TargetPosition;

    [SerializeField] private PlayerInputType playerInputType;

    private Coroutine inputUpdateCoroutine;

    /// <summary>
    /// Activates the input loop
    /// </summary>
    public virtual void Activate()
    {
        inputUpdateCoroutine = StartCoroutine(InputUpdate());
    }

    /// <summary>
    /// Deactivates the input loop
    /// </summary>
    public virtual void Deactivate()
    {
        StopCoroutine(inputUpdateCoroutine);
    }

    protected abstract IEnumerator InputUpdate();
}
