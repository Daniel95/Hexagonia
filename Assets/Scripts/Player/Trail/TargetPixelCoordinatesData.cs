using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TargetPixelCoordinatesData
{
    public Sprite Sprite;
    public TargetPixelCoordinatesLabel Label;
    public Color TargetColor;
    public TargetPixelsCoordinatesAverageType AverageType;

    [HideInInspector] public List<Vector2> TargetPixelCoordinates;

    public void UpdateTargetPixelCoordinates()
    {
        TargetPixelCoordinates.Clear();

        List<Vector2Int> _pixelCoordinatesWithTargetColor = Sprite.texture.GetPixelCoordinatesWithColor(TargetColor);
        if (_pixelCoordinatesWithTargetColor.Count == 0)
        {
            Debug.LogWarning("Zero pixelcoordinates found in Sprite " + Sprite.name + " with color " + TargetColor + ".");
            return;
        }

        switch (AverageType)
        {
            case TargetPixelsCoordinatesAverageType.Center:
                Vector2 _combinedPixelCoordinates = _pixelCoordinatesWithTargetColor.CombineVectors();
                Vector2 _centerAverage = _combinedPixelCoordinates / _pixelCoordinatesWithTargetColor.Count;

                TargetPixelCoordinates.Add(_centerAverage);

                break;
            case TargetPixelsCoordinatesAverageType.Corners:

                Vector2 _averageTopRight;
                Vector2 _averageTopLeft;
                Vector2 _averageBottomRight;
                Vector2 _averageBottomLeft;

                GetCornerAverages(_pixelCoordinatesWithTargetColor, out _averageTopRight, out _averageTopLeft, out _averageBottomRight, out _averageBottomLeft);

                TargetPixelCoordinates.Add(_averageTopRight);
                TargetPixelCoordinates.Add(_averageTopLeft);
                TargetPixelCoordinates.Add(_averageBottomRight);
                TargetPixelCoordinates.Add(_averageBottomLeft);

                break;
        }

        Debug.Log(TargetPixelCoordinates.Count + " pixelcoordinates found in Sprite " + Sprite.name + " with color " + TargetColor + ".");
    }

    private void GetCornerAverages(List<Vector2Int> _pixelCoordinates, out Vector2 _averageTopRight, out Vector2 _averageTopLeft, out Vector2 _averageBottomRight, out Vector2 _averageBottomLeft)
    {
        Vector2 _combinedTopRight = new Vector2();
        int _topRightCount = 0;

        Vector2 _combinedTopLeft = new Vector2();
        int _topLeftCount = 0;

        Vector2 _combinedBottomRight = new Vector2();
        int _bottomRightCount = 0;

        Vector2 _combinedBottomLeft = new Vector2();
        int _bottomLeftCount = 0;

        Vector2 center = new Vector2(Sprite.texture.height, Sprite.texture.height) / 2;

        foreach (Vector2Int _position in _pixelCoordinates)
        {
            bool top = _position.y > center.y;
            bool right = _position.x > center.x;

            if (top && right)
            {
                _combinedTopRight += _position;
                _topRightCount++;
            }
            else if (top && !right)
            {
                _combinedTopLeft += _position;
                _topLeftCount++;
            }
            else if (!top && right)
            {
                _combinedBottomRight += _position;
                _bottomRightCount++;
            }
            else
            {
                _combinedBottomLeft += _position;
                _bottomLeftCount++;
            }
        }

        _averageTopRight = _combinedTopRight / _topRightCount;
        _averageTopLeft = _combinedTopLeft / _topLeftCount;
        _averageBottomRight = _combinedBottomRight / _bottomRightCount;
        _averageBottomLeft = _combinedBottomLeft / _bottomLeftCount;
    }

}