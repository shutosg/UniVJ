using UnityEngine;

public static class MathUtility
{
    public static float Map(float value, float srcMin, float srcMax, float dstMin, float dstMax)
        => (value - srcMin) / (srcMax - srcMin) * (dstMax - dstMin) + dstMin;
}