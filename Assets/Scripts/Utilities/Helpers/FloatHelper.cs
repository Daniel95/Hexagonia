public static class FloatHelper
{

    public static float Scale(float normalizedInput, float newMin, float newMax)
    {
        float scaledRange = newMax - newMin;
        float scaledValue = newMin + (normalizedInput * scaledRange);
        return scaledValue;
    }

    public static float Normalize(float input, float minValue, float maxValue)
    {
        float range = maxValue - minValue;
        float normalizedValue = (input - minValue) / range;
        return normalizedValue;
    }

}
