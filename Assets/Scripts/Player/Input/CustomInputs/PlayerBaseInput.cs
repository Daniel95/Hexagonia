using System;
using System.Collections;
using UnityEngine;

public abstract class PlayerBaseInput : MonoBehaviour
{
    public Action<Vector3> TargetPositionUpdatedEvent; 
    public PlayerInputType PlayerInputType { get { return playerInputType; } }

    protected Vector3 TargetPosition;

    [SerializeField] private PlayerInputType playerInputType;

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
