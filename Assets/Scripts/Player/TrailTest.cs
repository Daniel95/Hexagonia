using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailTest : MonoBehaviour {

    [SerializeField] private Color targetColor;

    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        List<Vector2Int> pixelCoordinates = spriteRenderer.sprite.texture.GetPixelCoordinatesWithColor(targetColor);
        if(pixelCoordinates.Count == 0) { return; }

        Vector2 combinedPixelCoordinates = new Vector2();

        foreach (Vector2Int pixelCoordinate in pixelCoordinates)
        {
            combinedPixelCoordinates += pixelCoordinate;
        }

        float pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;
        Vector2 averagePixelPosition = combinedPixelCoordinates / pixelCoordinates.Count;

        Vector3 worldPosition = spriteRenderer.PixelCoordinateToWorldPosition(averagePixelPosition);

        DebugHelper.SetDebugPosition(worldPosition, "worldPosition");

        /*
        Vector2 _combinedTopRight = new Vector2();
        int _topRightCount = 0;

        Vector2 _combinedTopLeft = new Vector2();
        int _topLeftCount = 0;

        Vector2 _combinedBottomRight = new Vector2();
        int _bottomRightCount = 0;

        Vector2 _combinedBottomLeft = new Vector2();
        int _bottomLeftCount = 0;

        Vector2 _tranformPosition = transform.position;

        foreach (Vector2Int _position in pixelCoordinates)
        {
            bool top = _position.y > _tranformPosition.y;
            bool right = _position.x > _tranformPosition.x;

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

        Vector2 averageTopRight = _combinedTopRight / _topRightCount;
        Vector2 averageTopLeft = _combinedTopLeft / _topLeftCount;
        Vector2 averageBottomRight = _combinedBottomRight / _bottomRightCount;
        Vector2 averageBottomLeft = _combinedBottomLeft / _bottomLeftCount;

        DebugHelper.SetDebugPosition(averageTopRight, "averageTopRight");
        DebugHelper.SetDebugPosition(averageTopLeft, "averageTopLeft");
        DebugHelper.SetDebugPosition(averageBottomRight, "averageBottomRight");
        DebugHelper.SetDebugPosition(averageBottomLeft, "averageBottomLeft");
        */


        //List<Vector3> _worldPositionsOfColor = spriteRenderer.GetWorldPositionsOfColor(targetColor);

        //Vector3 average = _worldPositionsOfColor.Average();
        //Debug.Log(average);

        //DebugHelper.SetDebugPosition(averageTopRight, "averageTopRight");
        //DebugHelper.SetDebugPosition(averageTopLeft, "averageTopLeft");
        //DebugHelper.SetDebugPosition(averageBottomRight, "averageBottomRight");
        //DebugHelper.SetDebugPosition(averageBottomLeft, "averageBottomLeft");

        //Debug.Break();
    }

}
