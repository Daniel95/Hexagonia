using System;
using UnityEngine;

public static class MathHelper
{

    //Calculate the intersection point of two lines. Returns true if lines intersect, otherwise false.
    //Note that in 3d, two lines do not intersect most of the time. So if the two lines are not in the 
    //same plane, use ClosestPointsOnTwoLines() instead.
    public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parrallel
        if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }


    //Two non-parallel lines which may or may not touch each other have a point on each line which are closest
    //to each other. This function finds those two points. If the lines are not parallel, the function 
    //outputs true, otherwise false.
    public static bool ClosestPointsOnTwoLines(out Vector3 closestPointLine1, out Vector3 closestPointLine2, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {

        closestPointLine1 = Vector3.zero;
        closestPointLine2 = Vector3.zero;

        float a = Vector3.Dot(lineVec1, lineVec1);
        float b = Vector3.Dot(lineVec1, lineVec2);
        float e = Vector3.Dot(lineVec2, lineVec2);

        float d = a * e - b * b;

        //lines are not parallel
        if (d != 0.0f)
        {

            Vector3 r = linePoint1 - linePoint2;
            float c = Vector3.Dot(lineVec1, r);
            float f = Vector3.Dot(lineVec2, r);

            float s = (b * f - c * e) / d;
            float t = (a * f - c * b) / d;

            closestPointLine1 = linePoint1 + lineVec1 * s;
            closestPointLine2 = linePoint2 + lineVec2 * t;

            return true;
        }
        else
        {
            return false;
        }
    }

    public static int CeilToPowerOfTwo(float input)
    {
        int x = (int)input;
        x--;
        x |= (x >> 1);
        x |= (x >> 2);
        x |= (x >> 4);
        x |= (x >> 8);
        x |= (x >> 16);
        return x + 1;
    }

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

    public static float GetLowestNumber(float num1, float num2)
    {
        if (num1 <= num2)
        {
            return num1;
        }
        else
        {
            return num2;
        }
    }

    public static float GetHighestNumber(float num1, float num2)
    {
        if (num1 >= num2)
        {
            return num1;
        }
        else
        {
            return num2;
        }
    }

}
