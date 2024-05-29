using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DraggableObject : MonoBehaviour
{
    [SerializeField] protected DragListener dragListener;
    [SerializeField] protected float jointFrequency = 1;
    [SerializeField] protected float jointDamping = 1;
    [SerializeField] protected float maxDraggingVelocity = 5f;
    protected Rigidbody2D rigidbody;
    protected TargetJoint2D targetJoint;
    protected bool isDraggable = true;
    public bool IsDraggable => isDraggable;

    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        if (dragListener != null)
        {
            dragListener.Init(this);
        }
    }

    protected virtual void Update()
    {
        HandleDragging();
    }

    private void FixedUpdate()
    {
        LimitVelocity();
    }

    private void LimitVelocity()
    {
        if (dragListener != null && dragListener.IsDragging)
        {
            rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, maxDraggingVelocity);
        }
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
        targetJoint.frequency = jointFrequency;
    }
}
