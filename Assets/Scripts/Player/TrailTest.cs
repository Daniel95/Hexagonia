using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailTest : MonoBehaviour {

    [SerializeField] private Color targetColor;
    [SerializeField] private GameObject prefab;

    private Dictionary<string, Transform> trailsByName = new Dictionary<string, Transform>();
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject topRightTrail = Instantiate(prefab, transform);
        trailsByName.Add("topRightTrail", topRightTrail.transform);

        GameObject topLeftTrail = Instantiate(prefab, transform);
        trailsByName.Add("topLeftTrail", topLeftTrail.transform);

        GameObject bottomRightTrail = Instantiate(prefab, transform);
        trailsByName.Add("bottomRightTrail", bottomRightTrail.transform);

        GameObject bottomLeftTrail = Instantiate(prefab, transform);
        trailsByName.Add("bottomLeftTrail", bottomLeftTrail.transform);

        UpdateTrailPositions();
    }

    private void UpdateTrailPositions()
    {
        List<Vector2Int> pixelCoordinates = spriteRenderer.sprite.texture.GetPixelCoordinatesWithColor(targetColor);
        if (pixelCoordinates.Count == 0)
        {
            Debug.Log("zero");
            return;
        }

        Vector2 _combinedTopRight = new Vector2();
        int _topRightCount = 0;

        Vector2 _combinedTopLeft = new Vector2();
        int _topLeftCount = 0;

        Vector2 _combinedBottomRight = new Vector2();
        int _bottomRightCount = 0;

        Vector2 _combinedBottomLeft = new Vector2();
        int _bottomLeftCount = 0;

        Vector2 center = new Vector2(spriteRenderer.sprite.texture.height, spriteRenderer.sprite.texture.height) / 2;

        foreach (Vector2Int _position in pixelCoordinates)
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
            Vector2 averageTopRight = _combinedTopRight / _topRightCount;
            Vector3 topRightWorldPosition = spriteRenderer.PixelCoordinateToWorldPosition(averageTopRight);
            trailsByName["topRightTrail"].transform.position = topRightWorldPosition;
        }

        if (_topLeftCount != 0)
        {
            Vector2 averageTopLeft = _combinedTopLeft / _topLeftCount;
            Vector3 topLeftWorldPosition = spriteRenderer.PixelCoordinateToWorldPosition(averageTopLeft);
            trailsByName["topLeftTrail"].transform.position = topLeftWorldPosition;
        }

        if (_bottomRightCount != 0)
        {
            Vector2 averageBottomRight = _combinedBottomRight / _bottomRightCount;
            Vector3 bottomRightWorldPosition = spriteRenderer.PixelCoordinateToWorldPosition(averageBottomRight);
            trailsByName["bottomRightTrail"].transform.position = bottomRightWorldPosition;
        }

        if (_bottomLeftCount != 0)
        {
            Vector2 averageBottomLeft = _combinedBottomLeft / _bottomLeftCount;
            Vector3 bottomLeftWorldPosition = spriteRenderer.PixelCoordinateToWorldPosition(averageBottomLeft);
            trailsByName["bottomLeftTrail"].transform.position = bottomLeftWorldPosition;
        }
    }

}
