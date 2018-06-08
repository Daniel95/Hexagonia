using UnityEngine;

public class InstanceData
{
    public Transform Parent;
    public Vector3 LocalPosition;
    public Vector3 Scale;
    public Quaternion Rotation;

    public Matrix4x4 Matrix { get { return Matrix4x4.TRS(WorldPosition, Rotation, Scale); } }

    public InstanceData(Vector3 localPosition, Vector3 scale, Quaternion rotation, Transform parent = null)
    {
        LocalPosition = localPosition;
        Scale = scale;
        Rotation = rotation;
        Parent = parent;
    }

    private Vector3 WorldPosition
    {
        get
        {
            Vector3 _position = LocalPosition;
            if (Parent != null)
            {
                _position = Parent.TransformPoint(_position);
            }
            return _position;
        }
    }
}