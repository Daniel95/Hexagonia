using UnityEngine;

/// <summary>
/// An music visualizer effect that moves a tileable texture and scales an gameobject based on the music.
/// </summary>
public class ScaleVisualizer : MonoBehaviour
{
    [SerializeField] private DimensionType dimension;
	[SerializeField] private float min;
	[SerializeField] private float max;

    private float range;

    private void Update()
    {
        Vector3 _newScale = transform.localScale;

        switch (dimension)
        {
            case DimensionType.X:
                _newScale.x = min + (range * AudioPeer.SingleBand);
                break;

            case DimensionType.Y:
                _newScale.y = min + (range * AudioPeer.SingleBand);
                break;

            case DimensionType.Z:
                _newScale.z = min + (range * AudioPeer.SingleBand);
                break;
        }

        transform.localScale = _newScale;
    }

    private void Start()
    {
        range = max - min;
    }
}