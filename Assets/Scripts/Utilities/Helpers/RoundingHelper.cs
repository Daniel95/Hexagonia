using UnityEngine;

public class RoundingHelper
{

    /// <summary>
    /// Round the float to the highest, or lowest int, depending on if the float is negative or positive.
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static int InvertOnNegativeCeil(float f)
    {
        int i = 0;

        if (f > 0)
        {
            i = Mathf.CeilToInt(f);
        }
        else if (f < 0)
        {
            i = Mathf.FloorToInt(f);
        }

        return i;
    }

    public static int InvertOnNegativeCeilMax(float f, int max)
    {
        int i = 0;

        if (f > 0)
        {
            i = max;
        }
        else if (f < 0)
        {
            i = -max;
        }

        return i;
    }

    public static Vector2 InvertOnNegativeCeilMax(Vector2 vector, int max)
    {
        int normalizedToMaxVectorX = InvertOnNegativeCeilMax(vector.x, max);
        int normalizedToMaxVectorY = InvertOnNegativeCeilMax(vector.y, max);

        Vector2 normalizedToMaxVector = new Vector2(normalizedToMaxVectorX, normalizedToMaxVectorY);
        return normalizedToMaxVector;
    }

    public static int InvertOnNegativeFloor(float f)
    {
        int i = 0;

        if (f > 0)
        {
            i = Mathf.FloorToInt(f);
        }
        else if (f < 0)
        {
            i = Mathf.CeilToInt(f);
        }

        return i;
    }

    public static float RoundTo(float numberToRound, float numberToRoundTo)
    {
        float division = numberToRound / numberToRoundTo;
        float roundedDision = Mathf.Round(division);
        float roundedNumber = roundedDision * numberToRoundTo;
        return roundedNumber;
    }

    public static float CeilTo(float numberToCeil, float numberToCeilTo)
    {
        float division = numberToCeil / numberToCeilTo;
        float ceiledDivision = Mathf.Ceil(division);
        float ceiledNumber = ceiledDivision * numberToCeilTo;
        return ceiledNumber;
    }

    public static float FloorTo(float numberToFloor, float numberToFloorTo)
    {
        float division = numberToFloor / numberToFloorTo;
        float flooredDivision = Mathf.Floor(division);
        float flooredNumber = flooredDivision * numberToFloorTo;
        return flooredNumber;
    }

    public static float InvertOnNegativeCeilTo(float numberToRound, float numberToRoundTo)
    {
        float division = numberToRound / numberToRoundTo;
        float roundedDision = InvertOnNegativeCeil(division);
        float roundedNumber = roundedDision * numberToRoundTo;
        return roundedNumber;
    }

    public static float InvertOnNegativeFloorTo(float numberToRound, float numberToRoundTo)
    {
        float division = numberToRound / numberToRoundTo;
        float roundedDision = InvertOnNegativeFloor(division);
        float roundedNumber = roundedDision * numberToRoundTo;
        return roundedNumber;
    }

}
