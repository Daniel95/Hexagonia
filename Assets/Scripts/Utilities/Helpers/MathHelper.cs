using System;
using UnityEngine;

public static class MathHelper
{

    public static float Normalize(float input, float minValue, float maxValue)
    {
        float range = maxValue - minValue;
        float normalizedInput = (input - minValue) / range;
        return normalizedInput;
    }

    public static float Scale(float normalizedInput, float newMin, float newMax)
    {
        float scaledRange = newMax - newMin;
        float scaledValue = newMin + (normalizedInput * scaledRange);
        return scaledValue;
    }

    public static bool HasSameSign(float num1, float num2)
    {
        return Math.Sign(num1) == Math.Sign(num2);
    }

    public static float GetClosestNumber(float numberToCheck, float num1, float num2)
    {
        float num1Difference = Mathf.Abs(numberToCheck - num1);
        float num2Difference = Mathf.Abs(numberToCheck - num2);

        return num1Difference <= num2Difference ? num1 : num2;
    }

}