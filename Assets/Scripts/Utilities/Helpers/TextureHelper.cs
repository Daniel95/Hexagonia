using System.Collections.Generic;
using UnityEngine;

public static class TextureHelper {

    public static List<int> GetColorIndexes(this Texture2D texture, Color targetColor) {
        int width = texture.width;
        int height = texture.height;
        Color[] colors = texture.GetPixels();

        List<int> indexes = new List<int>();

        for (int i = 0; i < colors.Length; i++) {
            if (colors[i] == targetColor) {
                indexes.Add(i);
            }
        }

        return indexes;
    }

    public static List<Vector2Int> GetPixelCoordinatesWithColor(this Texture2D texture, Color targetColor) {
        int width = texture.width;
        int height = texture.height;
        Color[] colors = texture.GetPixels();

        List<Vector2Int> pixelCoordinates = new List<Vector2Int>();

        for (int i = 0; i < colors.Length; i++) {
            if (colors[i] == targetColor) {
                Vector2Int pixelCoordinate = new Vector2Int();
                pixelCoordinate.x = i % width;
                pixelCoordinate.y = (i - pixelCoordinate.x) / width;

                pixelCoordinates.Add(pixelCoordinate);
            }
        }

        return pixelCoordinates;
    }

    public static List<Vector3> GetWorldPositionsOfColor(this SpriteRenderer spriteRenderer, Color targetColor) {
        Transform transform = spriteRenderer.transform;
        Texture2D texture = spriteRenderer.sprite.texture;

        List<Vector2Int> pixelCoordinates = texture.GetPixelCoordinatesWithColor(targetColor);

        List<Vector3> worldPositions = new List<Vector3>();
        float pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;

        foreach (Vector2Int pixelCoordinate in pixelCoordinates) {
            Vector3 localPosition = (Vector2)pixelCoordinate / pixelsPerUnit;
            //Debug.Log("localPosition = " + localPosition);
            Vector3 worldPosition = transform.TransformPoint(localPosition);
            //Debug.Log("worldPosition = " + worldPosition);

            worldPositions.Add(worldPosition);
        }

        return worldPositions;
    }

    public static Vector3 PixelCoordinateToWorldPosition(this SpriteRenderer spriteRenderer, Vector2 pixelCoordinate)
    {
        Vector2 textureSize = new Vector2(spriteRenderer.sprite.texture.width, spriteRenderer.sprite.texture.height);
        Vector3 localPosition = (pixelCoordinate - (textureSize / 2)) / spriteRenderer.sprite.pixelsPerUnit;
        Vector3 worldPosition = spriteRenderer.transform.TransformPoint(localPosition);

        return worldPosition;
    }

}
