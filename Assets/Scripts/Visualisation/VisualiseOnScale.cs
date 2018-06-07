using UnityEngine;

public class VisualiseOnScale : MonoBehaviour
{
    [SerializeField] private Angle angle;
	[SerializeField] private float min;
	[SerializeField] private float max;

    private void Update()
    {
        Vector3 _newScale = transform.localScale;

        switch (angle)
        {
            case Angle.x:
                _newScale.x = min + ((max - min) * AudioPeer.SingleBand);
                break;

            case Angle.y:
                _newScale.y = min + ((max - min) * AudioPeer.SingleBand);
                break;

            case Angle.z:
                _newScale.z = min + ((max - min) * AudioPeer.SingleBand);
                break;
        }

        transform.localScale = _newScale;
    }
}

internal enum Angle
{
    x,
    y,
    z,
}