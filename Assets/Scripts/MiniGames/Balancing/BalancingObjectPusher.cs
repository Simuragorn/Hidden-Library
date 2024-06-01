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

        void Update()
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
            float xVelocity = targetRigidbody.velocity.x;
            if (changeVelocityDelayLeft <= 0)
            {
                xVelocity = Random.Range(minVelocity, maxVelocity) * MathHelper.GetRandomSign();
                changeVelocityDelayLeft = changeVelocityDelay;
            }

            float distance = Mathf.Abs(target.BasePosition.x - target.transform.position.x);
            if (distance >= maxDistance)
            {
                float expectedDistance = Mathf.Abs(target.BasePosition.x - (target.transform.position.x + xVelocity));
                if (expectedDistance > distance)
                {
                    xVelocity *= -1;
                }
            }
            targetRigidbody.velocity = new Vector2(xVelocity, targetRigidbody.velocity.y);
            changeVelocityDelayLeft -= Time.deltaTime;
        }
    }
}