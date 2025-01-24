using System;

public static class GGJMathUtils
{
    public static float ConvertInRange(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        // Ensure oldMin is less than oldMax
        if (oldMin > oldMax) throw new ArgumentException("oldMin must be less than oldMax");

        // Calculate the range
        var oldRange = oldMax - oldMin;
        if (oldRange == 0) return newMin; // Avoid division by zero

        // Normalize the value to a 0-1 range, then scale to the new range
        var normalizedValue = (value - oldMin) / oldRange;
        var newRange = newMax - newMin;
        return normalizedValue * newRange + newMin;
    }
}