using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global
{
    public class Distance
    {
        // Calculates an exponential falloff effect on a distance based on a factor. Any value of distance >= max will return 0.
        public static float CalcDistanceFactor(float distance, float max, float factor)
        {
            if (distance > max)
            {
                distance = max;
            }
            return Mathf.Pow(((max - distance) / max) * factor, 2);
        }
    }
}
