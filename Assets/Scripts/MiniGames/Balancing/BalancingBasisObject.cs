using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MiniGames.Balancing
{
    public class BalancingBasisObject : MonoBehaviour
    {
        [SerializeField] private List<BalancingObjectPosition> balancingObjectPositions;
        [SerializeField] private SpriteRenderer spriteRenderer;
        public IReadOnlyList<BalancingObjectPosition> BalancingObjectPositions => balancingObjectPositions;

        public int DisplayOrder => spriteRenderer.sortingOrder;
    }
}
