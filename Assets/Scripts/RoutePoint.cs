using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutePoint : MonoBehaviour
{
    public Route Route { get; private set; }

    public void Init(Route currentRoute)
    {
        Route = currentRoute;
    }
}
