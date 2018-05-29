using System.Collections.Generic;
using UnityEngine;

public class PlayerTrails : MonoBehaviour
{

    [SerializeField] private List<TrailPrefabAndLabelPair> trailPrefabByLabelDatas;
    [SerializeField] private Vector3 offset;

    private Dictionary<TargetPixelCoordinatesLabel, List<Transform>> trailsByLabel = new Dictionary<TargetPixelCoordinatesLabel, List<Transform>>();
    private SpriteRenderer spriteRenderer;
    private Sprite previousSprite;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        InitiateTrailPositions();
    }

    private void Update()
    {
        if(spriteRenderer.sprite == previousSprite) { return; }
        previousSprite = spriteRenderer.sprite;

        UpdateTrailPositions();
    }

    private void UpdateTrailPositions()
    {
        foreach (TrailPrefabAndLabelPair trailPrefabByLabelData in trailPrefabByLabelDatas)
        {
            List<Vector2> _pixelCoordinates = TargetPixelCoordinatesDataLibrary.Instance.GetTargetPixelCoordinates(spriteRenderer.sprite, trailPrefabByLabelData.Label);

            List<Transform> _trails = trailsByLabel[trailPrefabByLabelData.Label];

            for (int i = 0; i < _pixelCoordinates.Count; i++)
            {
                Vector2 _pixelCoordinate = _pixelCoordinates[i];
                Vector3 _worldPosition = spriteRenderer.PixelCoordinateToWorldPosition(_pixelCoordinate);

                _trails[i].position = _worldPosition + offset;
            }
        }
    }

    private void InitiateTrailPositions()
    {
        foreach (TrailPrefabAndLabelPair trailPrefabByLabelData in trailPrefabByLabelDatas)
        {
            List<Vector2> _pixelCoordinates = TargetPixelCoordinatesDataLibrary.Instance.GetTargetPixelCoordinates(spriteRenderer.sprite, trailPrefabByLabelData.Label);

            List<Transform> _trails = new List<Transform>();

            for (int i = 0; i < _pixelCoordinates.Count; i++)
            {
                Vector2 _pixelCoordinate = _pixelCoordinates[i];
                Vector3 _worldPosition = spriteRenderer.PixelCoordinateToWorldPosition(_pixelCoordinate);

                Transform trail = Instantiate(trailPrefabByLabelData.Prefab, transform).transform;

                trail.name = trailPrefabByLabelData.Label + " Trail " + i;

                trail.position = _worldPosition;

                _trails.Add(trail);
            }

            trailsByLabel.Add(trailPrefabByLabelData.Label, _trails);
        }
    }

}