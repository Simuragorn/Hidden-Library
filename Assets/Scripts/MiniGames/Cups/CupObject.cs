using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CupObject : DraggableObject
{
    private Rigidbody2D rigidbody;
    public int DisplayOrder => spriteRenderer.sortingOrder;
    public DragListener DragListener => dragListener;

    protected override void Awake()
    {
        base.Awake();
        rigidbody = GetComponent<Rigidbody2D>();
    }
    protected override void Update()
    {
        base.Update();
        if (dragListener.IsDragging)
        {
            transform.rotation = Quaternion.identity;
            rigidbody.velocity = Vector2.zero;
        }
    }

    public void SetDisplayOrder(int displayOrder)
    {
        spriteRenderer.sortingOrder = displayOrder;
    }
}
