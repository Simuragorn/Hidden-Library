using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class DragListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private Collider2D draggingCollider;
    [SerializeField] protected bool isDragging;
    Vector2 mouseOffset;
    protected Transform targetTransform;

    public bool IsDragging => isDragging;

    private void Awake()
    {
        draggingCollider = GetComponent<Collider2D>();
    }

    public void SetDraggableTarget(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 mousePosition = GetMousePosition();
        mouseOffset = (Vector2)targetTransform.position - mousePosition;
        Vector3 point = new Vector3(mousePosition.x, mousePosition.y, draggingCollider.bounds.center.z);
        if (draggingCollider.bounds.Contains(point))
        {
            isDragging = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (draggingCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(draggingCollider.bounds.center, 0.1f);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(GetMousePosition(), 0.1f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
    public Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public Vector2 GetNewPosition()
    {
        return GetMousePosition() + mouseOffset;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
    }
}
