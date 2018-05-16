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
            Debug.LogError("Zero pixelcoordinates found in Sprite " + Sprite.name + " with color " + TargetColor + ".");
            return;
        }

        Debug.Log("Found " + _pixelCoordinatesWithTargetColor.Count + " pixelcoordinates in Sprite " + Sprite.name + " with color " + TargetColor + ".");

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

                if(_averageTopRight != Vector2.zero)
                {
                    TargetPixelCoordinates.Add(_averageTopRight);
                }
                if (_averageTopLeft != Vector2.zero)
                {
                    TargetPixelCoordinates.Add(_averageTopLeft);
                }
                if (_averageBottomRight != Vector2.zero)
                {
                    TargetPixelCoordinates.Add(_averageBottomRight);
                }
                if (_averageBottomLeft != Vector2.zero)
                {
                    TargetPixelCoordinates.Add(_averageBottomLeft);
                }

                break;
        }
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

        if (_topRightCount != 0)
        {
            _averageTopRight = _combinedTopRight / _topRightCount;
        }
        else
        {
            _averageTopRight = Vector2.zero;
            Debug.LogError("topRight pixelcoordinates not found in Sprite " + Sprite.name + " with color " + TargetColor + ".");
        }

        if (_topLeftCount != 0)
        {
            _averageTopLeft = _combinedTopLeft / _topLeftCount;
        }
        else
        {
            _averageTopLeft = Vector2.zero;
            Debug.LogError("topLeft pixelcoordinates not found in Sprite " + Sprite.name + " with color " + TargetColor + ".");
        }

        if (_bottomRightCount != 0)
        {
            _averageBottomRight = _combinedBottomRight / _bottomRightCount;
        }
        else
        {
            _averageBottomRight = Vector2.zero;
            Debug.LogError("bottomRight pixelcoordinates not found in Sprite " + Sprite.name + " with color " + TargetColor + ".");
        }

        if (_bottomLeftCount != 0)
        {
            _averageBottomLeft = _combinedBottomLeft / _bottomLeftCount;
        }
        else
        {
            _averageBottomLeft = Vector2.zero;
            Debug.LogError("bottomLeft pixelcoordinates not found in Sprite " + Sprite.name + " with color " + TargetColor + ".");
        }

    }

}