using UnityEngine;

public static class VectorHelper
{

    public static Vector2 Increment(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x + v2.x, v1.y + v2.y);
    }

    public static Vector3 Increment(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    }

    public static Vector2 Decrement(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x - v2.x, v1.y - v2.y);
    }

    public static Vector3 Decrement(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
    }

    public static Vector2 Multiply(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x * v2.x, v1.y * v2.y);
    }

    public static Vector2 Multiply(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    public static Vector2 InvertedMultiply(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x * -v2.x, v1.y * -v2.y);
    }

    public static Vector2 InvertedMultiply(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x * -v2.x, v1.y * -v2.y, v1.z * -v2.z);
    }

    public static Vector2 Divide(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x / v2.x, v1.y / v2.y);
    }

    public static Vector3 Divide(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
    }

    public static Vector2 Modulo(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x % v2.x, v1.y % v1.y);
    }

    public static Vector2 CeilTo(Vector2 v1, Vector2 v2)
    {
        return new Vector2(RoundingHelper.CeilTo(v1.x, v2.x), RoundingHelper.CeilTo(v1.y, v2.y));
    }

    public static Vector2 FloorTo(Vector2 v1, Vector2 v2)
    {
        return new Vector2(RoundingHelper.FloorTo(v1.x, v2.x), RoundingHelper.FloorTo(v1.y, v2.y));
    }

    public static Vector2 InvertOnNegativeCeilTo(Vector2 v1, Vector2 v2)
    {
        return new Vector2(RoundingHelper.InvertOnNegativeCeilTo(v1.x, v2.x), RoundingHelper.InvertOnNegativeFloorTo(v1.y, v2.y));
    }

    public static Vector2 InvertOnNegativeFloorTo(Vector2 v1, Vector2 v2)
    {
        return new Vector2(RoundingHelper.InvertOnNegativeFloorTo(v1.x, v2.x), RoundingHelper.InvertOnNegativeCeilTo(v1.y, v2.y));
    }

    public static Vector2 RoundTo(Vector2 v1, Vector2 v2)
    {
        return new Vector2(RoundingHelper.RoundTo(v1.x, v2.x), RoundingHelper.RoundTo(v1.y, v2.y));
    }

    public static Vector3 RoundTo(Vector3 v1, Vector3 v2)
    {
        return new Vector3(RoundingHelper.RoundTo(v1.x, v2.x), RoundingHelper.RoundTo(v1.y, v2.y), RoundingHelper.RoundTo(v1.z, v2.z));
    }

    public static Vector2 Round(Vector2 v1)
    {
        return new Vector2(Mathf.Round(v1.x), Mathf.Round(v1.y));
    }

    public static Vector3 Round(Vector3 v1)
    {
        return new Vector3(Mathf.Round(v1.x), Mathf.Round(v1.y), Mathf.Round(v1.z));
    }

    public static Vector2 Floor(Vector2 v1)
    {
        return new Vector2(Mathf.Floor(v1.x), Mathf.Floor(v1.y));
    }

    public static Vector3 Floor(Vector3 v1)
    {
        return new Vector3(Mathf.Floor(v1.x), Mathf.Floor(v1.y), Mathf.Floor(v1.z));
    }

    public static Vector2 Ceil(Vector2 v1)
    {
        return new Vector2(Mathf.Ceil(v1.x), Mathf.Ceil(v1.y));
    }

    public static Vector3 Ceil(Vector3 v1)
    {
        return new Vector3(Mathf.Ceil(v1.x), Mathf.Ceil(v1.y), Mathf.Ceil(v1.z));
    }

    public static Vector2 InvertOnNegativeCeil(Vector2 vector)
    {
        return new Vector2(RoundingHelper.InvertOnNegativeCeil(vector.x), RoundingHelper.InvertOnNegativeCeil(vector.y));
    }

    public static Vector2 InvertOnNegativeFloor(Vector2 vector)
    {
        return new Vector2(RoundingHelper.InvertOnNegativeFloor(vector.x), RoundingHelper.InvertOnNegativeFloor(vector.y));
    }

    public static Vector2 Clamp(Vector2 v1, float min, float max)
    {
        return new Vector2(Mathf.Clamp(v1.x, min, max), Mathf.Clamp(v1.y, min, max));
    }

    public static Vector3 Clamp(Vector3 v1, float min, float max)
    {
        return new Vector3(Mathf.Clamp(v1.x, min, max), Mathf.Clamp(v1.y, min, max), Mathf.Clamp(v1.z, min, max));
    }

    public static Vector2 Clamp(Vector2 v1, Vector2 min, Vector2 max) {
        return new Vector2(Mathf.Clamp(v1.x, min.x, max.x), Mathf.Clamp(v1.y, min.y, max.y));
    }

    public static Vector3 Clamp(Vector3 v1, Vector3 min, Vector3 max) {
        return new Vector3(Mathf.Clamp(v1.x, min.x, max.x), Mathf.Clamp(v1.y, min.y, max.y), Mathf.Clamp(v1.z, min.z, max.z));
    }

    public static Vector2 Abs(Vector2 vector)
    {
        return new Vector2(Mathf.Abs(vector.x), Mathf.Abs(vector.y));
    }

    public static Vector3 Abs(Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    public static bool AbsSizeIsSmallerThen(this Vector2 vector, Vector2 compareSize)
    {
        Vector2 absSize = Abs(vector);
        bool isSmaller = absSize.x < compareSize.x && absSize.y < compareSize.y;
        return isSmaller;
    }

    public static Vector2 GetRandomizedVector2(float min, float max)
    {
        return new Vector2()
        {
            x = Random.Range(min, max),
            y = Random.Range(min, max)
        };
    }

    public static Vector3 GetRandomizedVector3(float min, float max)
    {
        return new Vector3()
        {
            x = Random.Range(min, max),
            y = Random.Range(min, max),
            z = Random.Range(min, max)
        };
    }

    public static bool VectorsAxisesHaveSameSigns(Vector2 vector1, Vector2 vector2)
    {
        bool axisesHaveSameSign = MathHelper.HasSameSign(vector1.x, vector2.x) && MathHelper.HasSameSign(vector1.y, vector2.y);
        return axisesHaveSameSign;
    }

    public static bool VectorsAxisHasSameSign(Vector2 vector1, Vector2 vector2)
    {
        bool axisesHaveSameSign = MathHelper.HasSameSign(vector1.x, vector2.x) || MathHelper.HasSameSign(vector1.y, vector2.y);
        return axisesHaveSameSign;
    }

    //??
    //returns positive when to the right (local) and negative when to the left
    public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 right = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(right, up);

        return dir;
    }
    //??

    public static float DirectionToAngle(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        return angle;
    }

    public static bool DirectionIsToTheRight(Vector2 myRight, Vector2 direction)
    {
        return Vector2.Dot(myRight, direction) > 0;
    }

    public static bool IsBiggerOrEquals(this Vector2 vector, Vector2 compareVector)
    {
        bool isBiggerOrEquals = vector.x >= compareVector.x && vector.y >= compareVector.y;
        return isBiggerOrEquals;
    }

    public static bool IsSmallerOrEquals(this Vector2 vector, Vector2 compareVector)
    {
        bool isBiggerOrEquals = vector.x <= compareVector.x && vector.y <= compareVector.y;
        return isBiggerOrEquals;
    }

    public static bool IsBetweenVectors(this Vector2 vector, Vector2 compareVector1, Vector2 compareVector2)
    {
        float lowestX = MathHelper.GetLowestNumber(compareVector1.x, compareVector2.x);
        if (vector.x < lowestX) { return false; }

        float highestX = MathHelper.GetHighestNumber(compareVector1.x, compareVector2.x);
        if (vector.x > highestX) { return false; }

        float lowestY = MathHelper.GetLowestNumber(compareVector1.y, compareVector2.y);
        if (vector.y < lowestY) { return false; }

        float highestY = MathHelper.GetHighestNumber(compareVector1.y, compareVector2.y);
        if (vector.y > highestY) { return false; }

        return true;
    }

    public static bool IsBetweenVectors(this Vector3 vector, Vector3 compareVector1, Vector3 compareVector2)
    {
        float lowestX = MathHelper.GetLowestNumber(compareVector1.x, compareVector2.x);
        if (vector.x < lowestX) { return false; }

        float highestX = MathHelper.GetHighestNumber(compareVector1.x, compareVector2.x);
        if (vector.x > highestX) { return false; }

        float lowestY = MathHelper.GetLowestNumber(compareVector1.y, compareVector2.y);
        if (vector.y < lowestY) { return false; }

        float highestY = MathHelper.GetHighestNumber(compareVector1.y, compareVector2.y);
        if (vector.y > highestY) { return false; }

        return true;
    }

}
