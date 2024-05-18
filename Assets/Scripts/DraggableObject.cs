using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    [SerializeField] protected DragListener dragListener;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        dragListener.SetDraggableTarget(transform);
        dragListener.Init(spriteRenderer);
    }

    protected virtual void Update()
    {
        if (dragListener.IsDragging)
        {
            transform.position = dragListener.GetNewPosition();
        }
    }
}
