using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    [SerializeField] private List<RoutePoint> points;

    public IReadOnlyList<RoutePoint> Points => points;

    public Vector2 GetPointByIndex(int index)
    {
        return points[index].transform.position;
    }

    public int GetIndexByPoint(RoutePoint point)
    {
        return points.IndexOf(point);
    }

    private void Start()
    {
        foreach (var point in points)
        {
            point.Init(this);
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < Points.Count - 1; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Points[i].transform.position, Points[i + 1].transform.position);
        }
    }
}
