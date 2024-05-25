using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class DragListener : MonoBehaviour
{
    private Collider2D draggingCollider;
    protected bool isDragging;
    protected SpriteRenderer spriteRenderer;
    public int DisplayOrder => spriteRenderer.sortingOrder;
    public bool IsDragging => isDragging;

    public void Init(SpriteRenderer currentSpriteRenderer)
    {
        spriteRenderer = currentSpriteRenderer;
    }

    private void Awake()
    {
        draggingCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        bool releasing = Input.GetMouseButtonUp(0);
        if (isDragging && releasing)
        {
            OnRelease();
        }
    }

    public void OnDrag()
    {
        Vector2 mousePosition = GetMousePosition();
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

    public void OnRelease()
    {
        isDragging = false;
        Debug.Log("Pointer up");
    }
    public Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
