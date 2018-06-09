using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSpriteDetailPositions : MonoBehaviour
{
    public static Action PositionsInitiatedEvent;
    public static Action PositionsUpdatedEvent;

    public Dictionary<TargetPixelCoordinatesLabel, List<Vector3>> PositionsByLabel { get { return positionsByLabel; } }
    public Vector3 TopLeftWingPosition { get { return positionsByLabel[TargetPixelCoordinatesLabel.Wings][3]; } }
    public Vector3 TopRightWingPosition { get { return positionsByLabel[TargetPixelCoordinatesLabel.Wings][2]; } }
    public Vector3 BottomLeftWingPosition { get { return positionsByLabel[TargetPixelCoordinatesLabel.Wings][1]; } }
    public Vector3 BottomRightWingPosition { get { return positionsByLabel[TargetPixelCoordinatesLabel.Wings][0]; } }
    public Vector3 EnginePosition { get { return positionsByLabel[TargetPixelCoordinatesLabel.Engine][0]; } }
    public static PlayerSpriteDetailPositions Instance { get { return GetInstance(); } }

    #region Singleton
    private static PlayerSpriteDetailPositions instance;

    private static PlayerSpriteDetailPositions GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<PlayerSpriteDetailPositions>();
        }
        return instance;
    }
    #endregion

    private Dictionary<TargetPixelCoordinatesLabel, List<Vector3>> positionsByLabel = new Dictionary<TargetPixelCoordinatesLabel, List<Vector3>>();
    private List<TargetPixelCoordinatesLabel> targetPixelCoordinatesLabels;

    private SpriteRenderer spriteRenderer;
    private Sprite previousSprite;

    private void UpdatePositions()
    {
        foreach (TargetPixelCoordinatesLabel targetPixelCoordinatesLabel in targetPixelCoordinatesLabels)
        {
            List<Vector2> _pixelCoordinates = TargetPixelCoordinatesDataLibrary.Instance.GetTargetPixelCoordinates(spriteRenderer.sprite, targetPixelCoordinatesLabel);
            for (int i = 0; i < _pixelCoordinates.Count; i++)
            {
                Vector2 _pixelCoordinate = _pixelCoordinates[i];
                positionsByLabel[targetPixelCoordinatesLabel][i] = spriteRenderer.PixelCoordinateToWorldPosition(_pixelCoordinate);
            }
        }

        if(PositionsUpdatedEvent != null)
        {
            PositionsUpdatedEvent();
        }
    }

    private void InitPositionsByLabel()
    {
        targetPixelCoordinatesLabels = EnumHelper.GetValues<TargetPixelCoordinatesLabel>();

        foreach (TargetPixelCoordinatesLabel targetPixelCoordinatesLabel in targetPixelCoordinatesLabels)
        {
            List<Vector3> _positions = new List<Vector3>();

            List<Vector2> _pixelCoordinates = TargetPixelCoordinatesDataLibrary.Instance.GetTargetPixelCoordinates(spriteRenderer.sprite, targetPixelCoordinatesLabel);
            foreach (Vector2 _pixelCoordinate in _pixelCoordinates)
            {
                Vector3 _worldPosition = spriteRenderer.PixelCoordinateToWorldPosition(_pixelCoordinate);
                _positions.Add(_worldPosition);
            }

            positionsByLabel.Add(targetPixelCoordinatesLabel, _positions);
        }

        if (PositionsInitiatedEvent != null)
        {
            PositionsInitiatedEvent();
        }
    }

    private void Update()
    {
        if (spriteRenderer.sprite == previousSprite) { return; }
        previousSprite = spriteRenderer.sprite;

        UpdatePositions();
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitPositionsByLabel();
    }
}
