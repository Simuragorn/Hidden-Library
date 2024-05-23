using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MiniGames.Balancing
{
    public class BalancingBasisObject : BalancingObject
    {
        private float movementVelocity;
        private float rotationSpeed;
        private float forcedVelocity;

        public void SetMovementData(float newVelocity, float newRotationSpeed)
        {
            movementVelocity = newVelocity;
            rotationSpeed = newRotationSpeed;
        }

        private void Update()
        {
            HandleMovement();

            float verticalAxis = Input.GetAxisRaw("Vertical");
            float rotationAmount = verticalAxis * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, rotationAmount);
        }

        private void HandleMovement()
        {
            float horizontalAxis = Input.GetAxis("Horizontal");
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
