using Assets.Scripts.Consts;
using Assets.Scripts.Dto;
using Spine.Unity;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Player player;
    public Vector2 MovementDirection { get; private set; }
    private Vector2? targetPoint;
    private BuildedRoute currentRoute;
    private Vector2 oldPosition;

    private void Update()
    {
        HandleRoute();
        HandleMovement();
        HandleAnimation(transform.position, oldPosition);
    }

    public void Init(Player currentPlayer)
    {
        player = currentPlayer;
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
        if (Vector2.Distance(transform.position, currentRoute.CustomTargetPoint) < CalculationConsts.DistanceOffset)
        {
            ResetTarget();
            return;
        }
        if (Vector2.Distance(transform.position, targetPoint.Value) < CalculationConsts.DistanceOffset)
        {
            RoutePointReached();
        }
    }

    private void RoutePointReached()
    {
        transform.position = targetPoint.Value;
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
        oldPosition = (Vector2)transform.position;
        MovementDirection = Vector2.zero;
        if (targetPoint == null)
        {
            return;
        }
        bool isReachedPoint = CheckIsReachedPoint();
        if (isReachedPoint)
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
        MovementDirection = direction;
        transform.position = newPosition;
    }

    private bool CheckIsReachedPoint()
    {
        bool isReachedPoint = Vector2.Distance(transform.position, targetPoint.Value) < CalculationConsts.DistanceOffset;
        if (currentRoute == null)
        {
            targetPoint = new Vector2(targetPoint.Value.x, transform.position.y);
            if (!isReachedPoint)
            {
                isReachedPoint = Mathf.Abs(targetPoint.Value.x - transform.position.x) < CalculationConsts.DistanceOffset;
            }
        }

        return isReachedPoint;
    }

    private void HandleAnimation(Vector2 newPosition, Vector2 oldPosition)
    {
        bool playerMoved = Vector2.Distance(oldPosition, newPosition) > CalculationConsts.MathOffset;
        if (playerMoved)
        {
           player.TrySetWalkingAnimation();
        }
        else if (!playerMoved)
        {
           player.TrySetIdleAnimation();
        }
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
