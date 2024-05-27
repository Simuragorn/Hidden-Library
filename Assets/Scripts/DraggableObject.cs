using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DraggableObject : MonoBehaviour
{
    [SerializeField] protected DragListener dragListener;
    [SerializeField] protected float jointFrequency = 1;
    [SerializeField] protected float jointDamping = 1;
    protected Rigidbody2D rigidbody;
    protected TargetJoint2D targetJoint;

    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        HandleDragging();
    }

    protected virtual void HandleDragging()
    {
        if (dragListener.IsDragging)
        {
            if (targetJoint == null)
            {
                AddJoint(dragListener.GetMousePosition());
            }
            targetJoint.target = dragListener.GetMousePosition();
        }
        else
        {
            if (targetJoint != null)
            {
                Destroy(targetJoint);
                targetJoint = null;
            }
        }
    }

    private void AddJoint(Vector2 mousePosition)
    {
        targetJoint = gameObject.AddComponent<TargetJoint2D>();
        targetJoint.anchor = targetJoint.transform.InverseTransformPoint(mousePosition);
        targetJoint.dampingRatio = jointDamping;
        targetJoint.enableCollision = false;
        targetJoint.frequency = jointFrequency;
    }
}
