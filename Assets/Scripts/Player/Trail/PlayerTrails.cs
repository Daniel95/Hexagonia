using System.Collections.Generic;
using UnityEngine;

public class PlayerTrails : MonoBehaviour
{

    [SerializeField] private List<TrailPrefabByLabelData> trailPrefabByLabelDatas;

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
        foreach (TrailPrefabByLabelData trailPrefabByLabelData in trailPrefabByLabelDatas)
        {
            List<Vector2> _pixelCoordinates = TargetPixelCoordinatesDataLibrary.Instance.GetTargetPixelCoordinates(spriteRenderer.sprite, trailPrefabByLabelData.Label);
            List<Transform> _trails = trailsByLabel[trailPrefabByLabelData.Label];

            for (int i = 0; i < _pixelCoordinates.Count; i++)
            {
                Vector2 _pixelCoordinate = _pixelCoordinates[i];
                Vector3 _worldPosition = spriteRenderer.PixelCoordinateToWorldPosition(_pixelCoordinate);
                _trails[i].position = _worldPosition;
            }
        }
    }

    private void InitiateTrailPositions()
    {
        foreach (TrailPrefabByLabelData trailPrefabByLabelData in trailPrefabByLabelDatas)
        {
            List<Vector2> _pixelCoordinates = TargetPixelCoordinatesDataLibrary.Instance.GetTargetPixelCoordinates(spriteRenderer.sprite, trailPrefabByLabelData.Label);
            List<Transform> _trails = trailsByLabel[trailPrefabByLabelData.Label];

            for (int i = 0; i < _pixelCoordinates.Count; i++)
            {
                Vector2 _pixelCoordinate = _pixelCoordinates[i];
                Vector3 _worldPosition = spriteRenderer.PixelCoordinateToWorldPosition(_pixelCoordinate);

                Transform trail = Instantiate(trailPrefabByLabelData.Prefab, transform).transform;

                trail.position = _worldPosition;

                _trails.Add(trail);
            }
        }
    }

}