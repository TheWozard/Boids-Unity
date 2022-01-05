using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global
{
    public interface SizeChanger
    {
        void OnSizeChange(Vector3 min, Vector3 max);
    }

    public class State
    {
        public static float size = 20F;
        public static float height = size;
        public static Vector3 minPosition = new Vector3(-1 * size, 0, -1 * size);
        public static Vector3 maxPosition = new Vector3(size, height, size);

        // A list of all classes that should be called when the size changes
        public static List<SizeChanger> onSizeChangeSubscription = new List<SizeChanger>();

        public static void UpdateSize(float newSize, float newHeight)
        {
            size = newSize;
            height = newHeight;
            minPosition = new Vector3(-1 * size, 0, -1 * size);
            maxPosition = new Vector3(size, height, size);

            onSizeChangeSubscription.ForEach(changer =>
            {
                changer.OnSizeChange(minPosition, maxPosition);
            }
            );
        }

        public static (Vector3, float) ClosestEdgePointTo(Vector3 point)
        {
            (float, float) ClosestX = CloserTo(point.x, maxPosition.x, minPosition.x);
            (float, float) ClosestY = CloserTo(point.y, maxPosition.y, minPosition.y);
            (float, float) ClosestZ = CloserTo(point.z, maxPosition.z, minPosition.z);

            if (ClosestX.Item2 < ClosestY.Item2 & ClosestX.Item2 < ClosestZ.Item2)
            {
                return (new Vector3(ClosestX.Item1, point.y, point.z), ClosestX.Item2);
            }
            if (ClosestY.Item2 < ClosestX.Item2 & ClosestY.Item2 < ClosestZ.Item2)
            {
                return (new Vector3(point.x, ClosestY.Item1, point.z), ClosestY.Item2);
            }
            return (new Vector3(point.x, point.y, ClosestZ.Item1), ClosestZ.Item2);
        }

        private static (float, float) CloserTo(float target, float option1, float option2)
        {
            float distance1 = Mathf.Abs(target - option1);
            float distance2 = Mathf.Abs(target - option2);
            if (Mathf.Abs(target - option1) > Mathf.Abs(target - option2))
            {
                return (option2, distance2);
            }
            return (option1, distance1);
        }
    }
}
