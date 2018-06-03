using System;
using System.Collections;
using UnityEngine;

public abstract class PlayerBaseInput : MonoBehaviour
{
    public Action<Vector3> TargetPositionUpdatedEvent; 
    public PlayerInputType PlayerInputType { get { return playerInputType; } }

    [SerializeField] private PlayerInputType playerInputType;

    protected Vector3 TargetPosition;

    private Coroutine inputUpdateCoroutine;

    public virtual void Activate()
    {
        inputUpdateCoroutine = StartCoroutine(InputUpdate());
    }

    public virtual void Deactivate()
    {
        StopCoroutine(inputUpdateCoroutine);
    }

    protected abstract IEnumerator InputUpdate();
}
