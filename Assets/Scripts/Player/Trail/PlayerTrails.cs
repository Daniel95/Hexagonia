using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the trails of the player, the trails are positioned at the target pixel coordinates that are within a certain color range indicated in the TargetPixelCoordinatesDataLibrary.
/// </summary>
public class PlayerTrails //: MonoBehaviour
{
    [SerializeField] private List<TrailPrefabAndLabelPair> trailPrefabByLabelDatas;

    private Dictionary<TargetPixelCoordinatesLabel, List<Transform>> trailsByLabel = new Dictionary<TargetPixelCoordinatesLabel, List<Transform>>();

    private void UpdateTrailPositions()
    {
        foreach (TrailPrefabAndLabelPair trailPrefabByLabelData in trailPrefabByLabelDatas)
        {
            List<Vector3> _positions = PlayerSpriteDetailPositions.Instance.PositionsByLabel[trailPrefabByLabelData.Label];
            List<Transform> _trails = trailsByLabel[trailPrefabByLabelData.Label];

            for (int i = 0; i < _positions.Count; i++)
            {
                _trails[i].position = _positions[i];
            }
        } 
    }

    private void InitiateTrailPositions()
    {
        foreach (TrailPrefabAndLabelPair trailPrefabByLabelData in trailPrefabByLabelDatas)
        {
            List<Vector3> _positions = PlayerSpriteDetailPositions.Instance.PositionsByLabel[trailPrefabByLabelData.Label];
            List<Transform> _trails = new List<Transform>();

            for (int i = 0; i < _positions.Count; i++)
            {
                Transform _trail = Instantiate(trailPrefabByLabelData.Prefab, transform).transform;
                _trail.position = _positions[i];
                _trail.name = trailPrefabByLabelData.Label + " Trail " + i;

                _trails.Add(_trail);
            }

            trailsByLabel.Add(trailPrefabByLabelData.Label, _trails);
        }
    }

    private void OnEnable()
    {
        PlayerSpriteDetailPositions.PositionsUpdatedEvent += UpdateTrailPositions;
        PlayerSpriteDetailPositions.PositionsInitiatedEvent += InitiateTrailPositions;
    }

    private void OnDisable()
    {
        PlayerSpriteDetailPositions.PositionsUpdatedEvent -= UpdateTrailPositions;
        PlayerSpriteDetailPositions.PositionsInitiatedEvent -= InitiateTrailPositions;
    }
}