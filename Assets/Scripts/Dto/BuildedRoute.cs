using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Dto
{
    public class BuildedRoute
    {
        public Route Route;
        public int CurrentPointIndex;
        public int TargetPointIndex;
        public Vector2 CustomTargetPoint;

        public BuildedRoute(Route route, int currentPointIndex, int targetPointIndex, Vector2 customPoint)
        {
            Route = route;
            this.CurrentPointIndex = currentPointIndex;
            this.TargetPointIndex = targetPointIndex;
            CustomTargetPoint = customPoint;
        }

        public float GetDistance()
        {
            float distance = 0f;
            if (TargetPointIndex != -1 && CurrentPointIndex != -1)
            {
                for (int i = CurrentPointIndex; i < TargetPointIndex; i++)
                {
                    Vector2 fromPoint = (Vector2)Route.Points[i].transform.position;
                    Vector2 toPoint = (Vector2)Route.Points[i + 1].transform.position;
                    distance += Vector2.Distance(fromPoint, toPoint);
                }
            }
            return distance;
        }
    }
}
