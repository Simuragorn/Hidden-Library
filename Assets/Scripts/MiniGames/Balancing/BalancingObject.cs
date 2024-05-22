using Assets.Scripts.Enums;
using Assets.Scripts.MiniGames.Balancing;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BalancingObject : MonoBehaviour
{
    [SerializeField] protected List<BalancingObjectPosition> balancingObjectPositions;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected BalancingObjectTypeEnum type;
    protected FixedJoint2D joint;
    protected Rigidbody2D rigidbody;

    public Rigidbody2D Rigidbody => rigidbody;
    public BalancingObjectTypeEnum Type => type;
    public IReadOnlyList<BalancingObjectPosition> BalancingObjectPositions => balancingObjectPositions;
    public int DisplayOrder => spriteRenderer.sortingOrder;
    protected void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init(int displayOrder, Rigidbody2D connectToRigidbody)
    {
        spriteRenderer.sortingOrder = displayOrder;
        ConnectWithRigidbody(connectToRigidbody);
    }
    protected void ConnectWithRigidbody(Rigidbody2D connectToRigidbody)
    {
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }
        if (connectToRigidbody == null)
        {
            return;
        }
        joint = gameObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = connectToRigidbody;
        joint.dampingRatio = 1;
        joint.enableCollision = false;
        joint.breakAction = JointBreakAction2D.Ignore;
    }
}
