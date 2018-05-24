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

    public static List<Vector2Int> GetPixelCoordinatesWithColor(this Texture2D texture, Color minColor, Color maxColor) {
        int width = texture.width;
        int height = texture.height;
        Color[] colors = texture.GetPixels();

        List<Vector2Int> pixelCoordinates = new List<Vector2Int>();

        for (int i = 0; i < colors.Length; i++) {
            if (colors[i].WithinColorRange(minColor, maxColor)) {
                Vector2Int pixelCoordinate = new Vector2Int();
                pixelCoordinate.x = i % width;
                pixelCoordinate.y = (i - pixelCoordinate.x) / width;

                pixelCoordinates.Add(pixelCoordinate);
            }
        }

        return pixelCoordinates;
    }

    public static List<Vector3> GetWorldPositionsOfColor(this SpriteRenderer spriteRenderer, Color minColor, Color maxColor) {
        Transform transform = spriteRenderer.transform;
        Texture2D texture = spriteRenderer.sprite.texture;

        List<Vector2Int> pixelCoordinates = texture.GetPixelCoordinatesWithColor(minColor, maxColor);

        List<Vector3> worldPositions = new List<Vector3>();
        float pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;

        foreach (Vector2Int pixelCoordinate in pixelCoordinates) {
            Vector3 localPosition = (Vector2)pixelCoordinate / pixelsPerUnit;
            Vector3 worldPosition = transform.TransformPoint(localPosition);

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

    private static bool WithinColorRange(this Color color, Color minColor, Color maxColor)
    {
        bool redMatch = color.r >= Mathf.Min(minColor.r, maxColor.r) && color.r <= Mathf.Max(minColor.r, maxColor.r);
        bool greenMatch = color.g >= Mathf.Min(minColor.g, maxColor.g) && color.g <= Mathf.Max(minColor.g, maxColor.g);
        bool blueMatch = color.b >= Mathf.Min(minColor.b, maxColor.b) && color.b <= Mathf.Max(minColor.b, maxColor.b);
        bool alphaMatch = color.a >= Mathf.Min(minColor.a, maxColor.a) && color.a <= Mathf.Max(minColor.a, maxColor.a);

        return redMatch && greenMatch && blueMatch && alphaMatch;
    }

}
