using Assets.Scripts.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.MiniGames.Balancing
{
    public class BalancingObjectPusher : MonoBehaviour
    {
        [Range(0, 2)]
        [SerializeField] private float minVelocity;
        [Range(0, 5)]
        [SerializeField] private float maxVelocity;
        [SerializeField] private float maxDistance = 2f;
        [SerializeField] private float changeVelocityDelay = 2f;
        private float changeVelocityDelayLeft;

        private BalancingObject target;
        private Rigidbody2D targetRigidbody;

        private void Start()
        {
            changeVelocityDelayLeft = changeVelocityDelay;
        }

        void FixedUpdate()
        {
            HandleBalancingObjectVelocity();
        }

        public void SetTarget(BalancingObject newTarget)
        {
            target = newTarget;
            targetRigidbody = target.Rigidbody;
        }

        private void HandleBalancingObjectVelocity()
        {
            if (target == null || targetRigidbody.bodyType == RigidbodyType2D.Static)
            {
                return;
            }
            var velocity = new Vector2(targetRigidbody.velocity.x, targetRigidbody.velocity.y);
            if (changeVelocityDelayLeft <= 0)
            {
                velocity = GetRandomVelocity();
                changeVelocityDelayLeft = changeVelocityDelay;
            }
            targetRigidbody.velocity = ValidateVelocity(velocity);
            changeVelocityDelayLeft -= Time.deltaTime;
        }

        private Vector2 GetRandomVelocity()
        {
            var velocity = new Vector2();
            velocity.x = Random.Range(minVelocity, maxVelocity) * MathHelper.GetRandomSign();
            velocity.y = Random.Range(minVelocity, maxVelocity) * MathHelper.GetRandomSign();
            return velocity;
        }

        private Vector2 ValidateVelocity(Vector2 velocity)
        {
            float xDistance = Mathf.Abs(target.BasePosition.x - target.transform.position.x);
            if (xDistance >= maxDistance)
            {
                float expectedXDistance = Mathf.Abs(target.BasePosition.x - (target.transform.position.x + velocity.x));
                if (expectedXDistance > xDistance)
                {
                    velocity.x *= -1;
                }
            }
            float yDistance = Mathf.Abs(target.BasePosition.y - target.transform.position.y);
            if (yDistance >= maxDistance)
            {
                float expectedYDistance = Mathf.Abs(target.BasePosition.y - (target.transform.position.y + velocity.y));
                if (expectedYDistance > yDistance)
                {
                    velocity.y *= -1;
                }
            }
            return velocity;
        }
    }
}