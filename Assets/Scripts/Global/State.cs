using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global
{
    public interface SizeChanger
    {
        void OnSizeChange();
    }

    public class AxisInfo
    {
        public readonly float length;
        public readonly float max;
        public readonly float min;
        public readonly float midpoint;
        public AxisInfo(float min, float max)
        {
            this.length = max - min;
            this.max = max;
            this.min = min;
            this.midpoint = min + (this.length / 2);
        }
        public AxisInfo WithPadding(float padding)
        {
            return new AxisInfo(min - padding, max + padding);
        }
    }

    public class State
    {
        public static float size;
        public static float height;
        public static float ratio;
        public static Vector3 minPosition;
        public static Vector3 maxPosition;
        public static AxisInfo axisX;
        public static AxisInfo axisY;
        public static AxisInfo axisZ;

        public static bool DebugDisplay = false;

        // A list of all classes that should be called when the size changes
        public static List<SizeChanger> onSizeChangeSubscription = new List<SizeChanger>();

        public static void SetSize(float size, float height, float ratio)
        {
            State.size = size;
            State.height = height;
            State.ratio = ratio;
            State.minPosition = new Vector3(ratio * -size, 0, -size);
            State.maxPosition = new Vector3(ratio * size, height, size);
            State.axisX = new AxisInfo(State.minPosition.x, State.maxPosition.x);
            State.axisY = new AxisInfo(State.minPosition.y, State.maxPosition.y);
            State.axisZ = new AxisInfo(State.minPosition.z, State.maxPosition.z);

            onSizeChangeSubscription.ForEach(changer =>
            {
                changer.OnSizeChange();
            }
            );
        }
    }
}
