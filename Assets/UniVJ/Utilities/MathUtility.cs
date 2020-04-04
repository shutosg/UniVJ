using UnityEngine;

namespace UniVJ.Utility
{
    public static class MathUtility
    {
        public static float Map(float value, float srcMin, float srcMax, float dstMin, float dstMax)
            => (value - srcMin) / (srcMax - srcMin) * (dstMax - dstMin) + dstMin;

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => a + (b - a) * t;

        public static bool NearlyEquals(float a, float b) => Mathf.Abs(a - b) <= 0.00001f;
    }
}