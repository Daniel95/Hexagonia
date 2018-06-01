using UnityEngine;

public abstract class PlayerInputBase : MonoBehaviour
{
    public Vector3 TargetPosition { get { return TargetPoint; } }
    public PlayerInputType PlayerInputType { get { return playerInputType; } }

    [SerializeField] private PlayerInputType playerInputType;

    protected Vector3 TargetPoint;

    public abstract void Activate();
    public abstract void Deactivate();
}
