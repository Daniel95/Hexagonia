using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the target pixel coordinates of the sprites that are within a color range. Also has a label so it can be identified what these pixel coordinates are used for.
/// </summary>
[Serializable]
public class TargetPixelCoordinatesData
{
    public List<Sprite> Sprites;
    public TargetPixelCoordinatesLabel Label;
    public Color MinColor;
    public Color MaxColor;
    public TargetPixelsCoordinatesType TargetPixelCoordinatesType;

    public List<SpriteAndPixelCoordinatesPair> spriteAndPixelCoordinatesPairs;

    /// <summary>
    /// Get the target pixel coordinates of a sprite.
    /// </summary>
    /// <param name="_sprite"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Update the target pixel coordinates of the sprites.
    /// </summary>
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
                Debug.LogError("Sprite " + _sprite.name + " with label " + Label + " does not have the same amount of pixels as the other sprites, has " + _pixelCoordinates.Count + " needs " + _pixelCount + ", color between " + MinColor + " and " + MaxColor);
                break;
            }
        }
    }

    private List<Vector2> GetPixelCoordinates(Sprite _sprite)
    {
        List<Vector2Int> _pixelCoordinatesWithTargetColor = _sprite.texture.GetPixelCoordinatesWithColor(MinColor, MaxColor);
        if (_pixelCoordinatesWithTargetColor.Count == 0)
        {
            Debug.LogError("Zero pixelcoordinates found in Sprite " + _sprite.name + " with label " + Label + ".");
            return null;
        }

        List<Vector2> _targetPixelCoordinates = new List<Vector2>();

        Debug.Log("Found " + _pixelCoordinatesWithTargetColor.Count + " pixelcoordinates in Sprite " + _sprite.name + " with label " + Label + ".");

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
            case TargetPixelsCoordinatesType.Corners:
                List<Vector2> _allPixelCoordinates = new List<Vector2>();
                foreach (Vector2 pixelCoordinate in _pixelCoordinatesWithTargetColor)
                {
                    _allPixelCoordinates.Add(pixelCoordinate);
                }

                Vector2 _size = new Vector2(_sprite.texture.width, _sprite.texture.height);
                Vector2 _sizeQuater = _size / 4;

                Vector2 _centerOfTopRight = new Vector2(_sizeQuater.x * 3, _sizeQuater.y);
                Vector2 _centerOfTopLeft = new Vector2(_sizeQuater.x, _sizeQuater.y);
                Vector2 _centerOfBottomRight = new Vector2(_sizeQuater.x * 3, _sizeQuater.y * 3);
                Vector2 _centerOfBottomLeft = new Vector2(_sizeQuater.x, _sizeQuater.y * 3);

                Vector2 _topRight = GetClosestInList(_centerOfTopRight, _allPixelCoordinates);
                Vector2 _topLeft = GetClosestInList(_centerOfTopLeft, _allPixelCoordinates);
                Vector2 _bottomRight = GetClosestInList(_centerOfBottomRight, _allPixelCoordinates);
                Vector2 _bottomLeft = GetClosestInList(_centerOfBottomLeft, _allPixelCoordinates);

                _targetPixelCoordinates.Add(_topRight);
                _targetPixelCoordinates.Add(_topLeft);
                _targetPixelCoordinates.Add(_bottomRight);
                _targetPixelCoordinates.Add(_bottomLeft);

                break;
        }

        return _targetPixelCoordinates;
    }

    private Vector2 GetClosestInList(Vector2 _targetPoint, List<Vector2> _points)
    {
        Vector2 _closestPoint = new Vector2();
        float _closestDistance = float.MaxValue;

        foreach (Vector2 _point in _points)
        {
            float _distance = Vector2.Distance(_point, _targetPoint); 
            if(_distance >= _closestDistance) { continue; }
            _closestDistance = _distance;
            _closestPoint = _point;
        }
        
        return _closestPoint;
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

        Vector2 _center = new Vector2(_sprite.texture.height, _sprite.texture.height) / 2;

        foreach (Vector2Int _position in _pixelCoordinates)
        {
            bool _top = _position.y > _center.y;
            bool _right = _position.x > _center.x;

            if (_top && _right)
            {
                _combinedTopRight += _position;
                _topRightCount++;
            }
            else if (_top && !_right)
            {
                _combinedTopLeft += _position;
                _topLeftCount++;
            }
            else if (!_top && _right)
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
            Debug.LogError("topRight pixelcoordinates not found in Sprite " + _sprite.name + " with label " + Label + ".");
        }

        if (_topLeftCount != 0)
        {
            _averageTopLeft = _combinedTopLeft / _topLeftCount;
        }
        else
        {
            _averageTopLeft = Vector2.zero;
            Debug.LogError("topLeft pixelcoordinates not found in Sprite " + _sprite.name + " with label " + Label + ".");
        }

        if (_bottomRightCount != 0)
        {
            _averageBottomRight = _combinedBottomRight / _bottomRightCount;
        }
        else
        {
            _averageBottomRight = Vector2.zero;
            Debug.LogError("bottomRight pixelcoordinates not found in Sprite " + _sprite.name + " with label " + Label + ".");
        }

        if (_bottomLeftCount != 0)
        {
            _averageBottomLeft = _combinedBottomLeft / _bottomLeftCount;
        }
        else
        {
            _averageBottomLeft = Vector2.zero;
            Debug.LogError("bottomLeft pixelcoordinates not found in Sprite " + _sprite.name + " with label " + Label + ".");
        }
    }
}