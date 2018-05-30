using System;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Data")]
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

                _targetPixelCoordinates.Sort((vector, compareVector) => {

                    float _priority = vector.x + vector.y / 2;
                    float _comparePriority = compareVector.x + compareVector.y / 2;

                    if(_priority < _comparePriority)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                });

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

    private void SortTest()
    {
        List<int> numbers = new List<int>();
        numbers.Add(4);
        numbers.Add(0);
        numbers.Add(10);
        numbers.Add(50);
        numbers.Add(1000);
        numbers.Add(40);

        // ... Sort the numbers by their first digit.
        //     We use ToString on each number.
        //     We access the first character of the string and compare that.
        //     This uses a lambda expression.
        numbers.Sort((a, b) => (a.ToString()[0].CompareTo(b.ToString()[0])));

        List<Vector2> test = new List<Vector2>();

        test.Sort((a, b) => {
            int condition = a.x < b.x ? 0 : 1;
            return condition;
        });
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