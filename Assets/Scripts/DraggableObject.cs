using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    [SerializeField] protected DragListener dragListener;

    protected virtual void Awake()
    {
        dragListener.SetDraggableTarget(transform);
    }

    protected virtual void Update()
    {
        if (dragListener.IsDragging)
        {
            transform.position = dragListener.GetNewPosition();
        }
    }
}
