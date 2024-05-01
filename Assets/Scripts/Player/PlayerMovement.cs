using Assets.Scripts.Consts;
using Assets.Scripts.Dto;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2? targetPoint;
    private BuildedRoute currentRoute;
    private void Update()
    {
        HandleRoute();
        HandleMovement();
    }

    public void ResetTarget()
    {
        targetPoint = null;
        currentRoute = null;
    }

    public void SetDirectTarget(Vector2 newTarget)
    {
        targetPoint = newTarget;
        currentRoute = null;
    }

    public void SetBuildedRoute(BuildedRoute buildedRoute)
    {
        currentRoute = buildedRoute;
        targetPoint = currentRoute.Route.GetPointByIndex(currentRoute.CurrentPointIndex);
    }

    private void HandleRoute()
    {
        if (currentRoute == null)
        {
            return;
        }
        if (Vector2.Distance(transform.position, currentRoute.CustomTargetPoint) < DistanceConsts.MinDistance)
        {
            ResetTarget();
            return;
        }
        if (Vector2.Distance(transform.position, targetPoint.Value) < DistanceConsts.MinDistance)
        {
            RoutePointReached();
        }
    }

    private void RoutePointReached()
    {
        if (currentRoute.TargetPointIndex == currentRoute.CurrentPointIndex)
        {
            targetPoint = currentRoute.CustomTargetPoint;
            currentRoute = null;
            return;
        }
        int offset = (int)Mathf.Sign(currentRoute.TargetPointIndex - currentRoute.CurrentPointIndex);
        currentRoute.CurrentPointIndex += offset;
        targetPoint = currentRoute.Route.GetPointByIndex(currentRoute.CurrentPointIndex);
    }

    private void HandleMovement()
    {
        if (targetPoint == null)
        {
            return;
        }
        if (Vector2.Distance(transform.position, targetPoint.Value) < DistanceConsts.MinDistance)
        {
            targetPoint = null;
            return;
        }
        Vector2 direction = (targetPoint.Value - (Vector2)transform.position).normalized;
        Vector2 newPosition = (Vector2)transform.position + speed * Time.deltaTime * direction;
        if (currentRoute == null)
        {
            newPosition.y = transform.position.y;
        }
        transform.position = newPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (targetPoint.HasValue)
        {
            Gizmos.DrawLine(transform.position, targetPoint.Value);
        }
    }
}
