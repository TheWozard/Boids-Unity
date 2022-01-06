using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global
{
    public struct EdgeInformation
    {
        public readonly bool isOutside;
        public readonly float distance;
        public readonly Vector3 closestPoint;
        public readonly Vector3 closestAxis;
        public EdgeInformation(bool isOutside, float distance, Vector3 closestPoint, Vector3 closestAxis)
        {
            this.isOutside = isOutside;
            this.distance = distance;
            this.closestPoint = closestPoint;
            this.closestAxis = closestAxis;
        }
    }

    class ClosestPoint
    {
        public readonly float distance = 0;
        public readonly float point = 0;
        public ClosestPoint(float distance, float point)
        {
            this.distance = distance;
            this.point = point;
        }
        public bool IsCloser(ClosestPoint point)
        {
            return this.distance < point.distance;
        }
    }

    class ClosestInfo
    {
        public readonly ClosestPoint point;
        public readonly bool isOutside;
        public ClosestInfo(ClosestPoint point, bool isOutside)
        {
            this.point = point;
            this.isOutside = isOutside;
        }
        public bool IsClosest(params ClosestInfo[] infos)
        {
            if (this.isOutside)
            {
                return true;
            }
            foreach (ClosestInfo info in infos)
            {
                if (info.isOutside)
                {
                    return false;
                }
            }
            foreach (ClosestInfo info in infos)
            {
                if (info.point.IsCloser(point))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class Edges
    {
        public static EdgeInformation GetEdgeInformation(Vector3 point)
        {

            ClosestInfo ClosestX = CloserTo(point.x, State.maxPosition.x, State.minPosition.x);
            ClosestInfo ClosestY = CloserTo(point.y, State.maxPosition.y, State.minPosition.y);
            ClosestInfo ClosestZ = CloserTo(point.z, State.maxPosition.z, State.minPosition.z);
            if (ClosestX.IsClosest(ClosestY, ClosestZ))
            {
                return new EdgeInformation(
                    ClosestX.isOutside, ClosestX.point.distance,
                    new Vector3(ClosestX.point.point, point.y, point.z),
                    new Vector3(1, 0, 0)
                );
            }
            if (ClosestY.IsClosest(ClosestX, ClosestZ))
            {
                return new EdgeInformation(
                    ClosestY.isOutside, ClosestY.point.distance,
                    new Vector3(point.x, ClosestY.point.point, point.z),
                    new Vector3(0, 1, 0)
                );
            }
            return new EdgeInformation(
                    ClosestZ.isOutside, ClosestZ.point.distance,
                    new Vector3(point.x, point.y, ClosestZ.point.point),
                    new Vector3(0, 0, 1)
                );
        }

        private static ClosestInfo CloserTo(float point, float target, params float[] additional)
        {
            float min = target;
            float max = target;
            ClosestPoint closest = DistanceInfo(point, target);
            foreach (float add in additional)
            {
                min = Mathf.Min(min, add);
                max = Mathf.Max(max, add);
                ClosestPoint temp = DistanceInfo(point, add);
                if (temp.distance < closest.distance)
                {
                    closest = temp;
                }
            }
            return new ClosestInfo(closest, point < min | point > max);
        }

        private static ClosestPoint DistanceInfo(float point, float target)
        {
            return new ClosestPoint(Mathf.Abs(point - target), target);
        }
    }
}
