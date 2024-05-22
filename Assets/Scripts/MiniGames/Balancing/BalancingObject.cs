using Assets.Scripts.Enums;
using Assets.Scripts.MiniGames.Balancing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BalancingObject : MonoBehaviour
{
    [SerializeField] private List<BalancingObjectPosition> balancingObjectPositions;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BalancingObjectTypeEnum type;
    private Rigidbody2D rigidbody;
    public BalancingObjectTypeEnum Type => type;
    public IReadOnlyList<BalancingObjectPosition> BalancingObjectPositions => balancingObjectPositions;
    public int DisplayOrder => spriteRenderer.sortingOrder;
    protected void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetDisplayOrder(int displayOrder)
    {
        spriteRenderer.sortingOrder = displayOrder;
    }
}
