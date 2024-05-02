using Assets.Scripts.Consts;
using Assets.Scripts.Dto;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    [SerializeField] private List<Route> routes;

    public BuildedRoute BuildRoute(Vector2 from, Vector2 to)
    {
        if (IsDirectMovementAvailable(from, to))
        {
            return new BuildedRoute(null, default, default, to);
        }
        List<BuildedRoute> buildedRoutes = new List<BuildedRoute>();
        foreach (var route in routes)
        {
            List<RoutePoint> points = route.Points.ToList();
            RoutePoint closestToPoint = points.Where(p =>
        IsDirectMovementAvailable(p.transform.position, to)).OrderBy(p => Vector2.Distance(p.transform.position, to)).FirstOrDefault();
            if (closestToPoint == null)
            {
                continue;
            }
            RoutePoint closestFromPoint = points.Where(p =>
        IsDirectMovementAvailable(p.transform.position, from)).OrderBy(p =>
        Mathf.Abs(route.GetIndexByPoint(closestToPoint) - route.GetIndexByPoint(p))).FirstOrDefault();

            if (closestFromPoint == null)
            {
                continue;
            }
            var buildedRoute = new BuildedRoute(route, points.IndexOf(closestFromPoint), points.IndexOf(closestToPoint), new Vector2(to.x, closestToPoint.transform.position.y));
            buildedRoutes.Add(buildedRoute);
        }

        return buildedRoutes.OrderBy(r => r.GetDistance() + Vector2.Distance(r.Route.GetPointByIndex(r.TargetPointIndex), r.CustomTargetPoint)).FirstOrDefault();
    }

    public bool IsDirectMovementAvailable(Vector2 from, Vector2 to)
    {
        int obstacleLayer = LayerMask.GetMask(LayerNameConsts.Obstacle);
        Vector2 direction = (to - from).normalized;
        float distance = (to - from).magnitude + CalculationConsts.DistanceOffset;
        RaycastHit2D hit = Physics2D.Raycast(from, direction, distance, obstacleLayer);
        return hit.collider == null;
    }
}
