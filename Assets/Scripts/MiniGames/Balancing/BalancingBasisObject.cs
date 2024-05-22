using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MiniGames.Balancing
{
    public class BalancingBasisObject : BalancingObject
    {
        [SerializeField] private float movementVelocity = 2f;
        private float forcedVelocity;
        private void Update()
        {
            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            forcedVelocity = horizontalAxis > 0 ? 1 * movementVelocity : -1 * movementVelocity;
            if (horizontalAxis == 0)
            {
                forcedVelocity = 0;
            }
        }

        private void FixedUpdate()
        {
            if (forcedVelocity != 0)
            {
                rigidbody.velocity = new Vector2(forcedVelocity, 0);
            }
        }
    }
}
