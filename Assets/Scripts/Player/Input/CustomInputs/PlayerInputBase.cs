using System;
using UnityEngine;

public abstract class PlayerInputBase : MonoBehaviour
{
    public Action<Vector3> TargetPositionUpdatedEvent; 
    public PlayerInputType PlayerInputType { get { return playerInputType; } }

    [SerializeField] private PlayerInputType playerInputType;

    protected Vector3 TargetPosition;

    public abstract void Activate();
    public abstract void Deactivate();
}
