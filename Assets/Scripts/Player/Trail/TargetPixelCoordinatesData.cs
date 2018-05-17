using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TargetPixelCoordinatesData
{

    public List<Sprite> Sprites;
    public TargetPixelCoordinatesLabel Label;
    public Color TargetColor;
    public TargetPixelsCoordinatesType TargetPixelCoordinatesType;
    [SerializeField] [HideInInspector] public List<SpriteAndPixelCoordinatesPair> spriteAndPixelCoordinatesPairs;

    public List<Vector2> GetTargetPixelCoordinates(Sprite _sprite)
    {
        SpriteAndPixelCoordinatesPair _spriteAndPixelCoordinatesPair = spriteAndPixelCoordinatesPairs.Find(x => x.Sprite == _sprite);

        if (_spriteAndPixelCoordinatesPair == null)
        {
            Debug.LogError("SpriteAndPixelCoordinatesPair with sprite " + _sprite.name + " and label " + Label + " does not exist");
            return null;
        }

        return _spriteAndPixelCoordinatesPair.PixelCoordinates;
    }

    public void UpdateTargetPixelCoordinates()
    {
        spriteAndPixelCoordinatesPairs.Clear();
        int? _pixelCount = null;

        foreach (Sprite _sprite in Sprites)
        {
            SpriteAndPixelCoordinatesPair _spriteAndPixelCoordinatesPair = new SpriteAndPixelCoordinatesPair()
            {
                Sprite = _sprite,
            };

            List<Vector2> _pixelCoordinates = GetPixelCoordinates(_sprite);
            _spriteAndPixelCoordinatesPair.PixelCoordinates = _pixelCoordinates;

            if(_pixelCount == null)
            {
                _pixelCount = _pixelCoordinates.Count;
                spriteAndPixelCoordinatesPairs.Add(_spriteAndPixelCoordinatesPair);
            }
            else if(_pixelCount == _pixelCoordinates.Count)
            {
                spriteAndPixelCoordinatesPairs.Add(_spriteAndPixelCoordinatesPair);
            }
            else
            {
                Debug.LogError("Sprite " + _sprite.name + " with label " + Label + " does not have the same amount of pixels as the other sprites, has " + _pixelCoordinates.Count + " needs " + _pixelCount + ", color " + TargetColor + "");
                break;
            }
        }
    }

    private List<Vector2> GetPixelCoordinates(Sprite _sprite)
    {
        List<Vector2Int> _pixelCoordinatesWithTargetColor = _sprite.texture.GetPixelCoordinatesWithColor(TargetColor);
        if (_pixelCoordinatesWithTargetColor.Count == 0)
        {
            Debug.LogError("Zero pixelcoordinates found in Sprite " + _sprite.name + " with color " + TargetColor + ".");
            return null;
        }

        List<Vector2> _targetPixelCoordinates = new List<Vector2>();

        Debug.Log("Found " + _pixelCoordinatesWithTargetColor.Count + " pixelcoordinates in Sprite " + _sprite.name + " with color " + TargetColor + ".");

        switch (TargetPixelCoordinatesType)
        {
            case TargetPixelsCoordinatesType.All:
                foreach (Vector2Int pixelCoordinate in _pixelCoordinatesWithTargetColor)
                {
                    _targetPixelCoordinates.Add(pixelCoordinate);
                }

                break;

            case TargetPixelsCoordinatesType.Average:
                Vector2 _combinedPixelCoordinates = _pixelCoordinatesWithTargetColor.CombineVectors();
                Vector2 _centerAverage = _combinedPixelCoordinates / _pixelCoordinatesWithTargetColor.Count;

                _targetPixelCoordinates.Add(_centerAverage);

                break;
            case TargetPixelsCoordinatesType.AverageInCorners:

                Vector2 _averageTopRight;
                Vector2 _averageTopLeft;
                Vector2 _averageBottomRight;
                Vector2 _averageBottomLeft;

                GetCornerAverages(_sprite, _pixelCoordinatesWithTargetColor, out _averageTopRight, out _averageTopLeft, out _averageBottomRight, out _averageBottomLeft);

                if(_averageTopRight != Vector2.zero)
                {
                    _targetPixelCoordinates.Add(_averageTopRight);
                }
                if (_averageTopLeft != Vector2.zero)
                {
                    _targetPixelCoordinates.Add(_averageTopLeft);
                }
                if (_averageBottomRight != Vector2.zero)
                {
                    _targetPixelCoordinates.Add(_averageBottomRight);
                }
                if (_averageBottomLeft != Vector2.zero)
                {
                    _targetPixelCoordinates.Add(_averageBottomLeft);
                }

                break;
        }

        return _targetPixelCoordinates;
    }

    private void GetCornerAverages(Sprite _sprite, List<Vector2Int> _pixelCoordinates, out Vector2 _averageTopRight, out Vector2 _averageTopLeft, out Vector2 _averageBottomRight, out Vector2 _averageBottomLeft)
    {
        Vector2 _combinedTopRight = new Vector2();
        int _topRightCount = 0;

        Vector2 _combinedTopLeft = new Vector2();
        int _topLeftCount = 0;

        Vector2 _combinedBottomRight = new Vector2();
        int _bottomRightCount = 0;

        Vector2 _combinedBottomLeft = new Vector2();
        int _bottomLeftCount = 0;

        Vector2 center = new Vector2(_sprite.texture.height, _sprite.texture.height) / 2;

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
            Debug.LogError("topRight pixelcoordinates not found in Sprite " + _sprite.name + " with color " + TargetColor + ".");
        }

        if (_topLeftCount != 0)
        {
            _averageTopLeft = _combinedTopLeft / _topLeftCount;
        }
        else
        {
            _averageTopLeft = Vector2.zero;
            Debug.LogError("topLeft pixelcoordinates not found in Sprite " + _sprite.name + " with color " + TargetColor + ".");
        }

        if (_bottomRightCount != 0)
        {
            _averageBottomRight = _combinedBottomRight / _bottomRightCount;
        }
        else
        {
            _averageBottomRight = Vector2.zero;
            Debug.LogError("bottomRight pixelcoordinates not found in Sprite " + _sprite.name + " with color " + TargetColor + ".");
        }

        if (_bottomLeftCount != 0)
        {
            _averageBottomLeft = _combinedBottomLeft / _bottomLeftCount;
        }
        else
        {
            _averageBottomLeft = Vector2.zero;
            Debug.LogError("bottomLeft pixelcoordinates not found in Sprite " + _sprite.name + " with color " + TargetColor + ".");
        }
    }

}