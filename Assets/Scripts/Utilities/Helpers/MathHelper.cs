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

    public static Vector2 GetPointOfIntersection(Vector2 p1, Vector2 p2, Vector2 n1, Vector2 n2) {
        Vector2 p1End = p1 + n1; // another point in line p1->n1
        Vector2 p2End = p2 + n2; // another point in line p2->n2

        float m1 = (p1End.y - p1.y) / (p1End.x - p1.x); // slope of line p1->n1
        float m2 = (p2End.y - p2.y) / (p2End.x - p2.x); // slope of line p2->n2

        float b1 = p1.y - m1 * p1.x; // y-intercept of line p1->n1
        float b2 = p2.y - m2 * p2.x; // y-intercept of line p2->n2

        float px = (b2 - b1) / (m1 - m2); // collision x
        float py = m1 * px + b1; // collision y

        return new Vector2(px, py); // return statement
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
}
