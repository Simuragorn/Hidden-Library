using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MiniGames.Balancing
{
    [RequireComponent(typeof(Collider2D))]
    public class BalancingObject : DraggableObject
    {
        [SerializeField] private bool isTouched;
        [SerializeField] private bool isDraggable = true;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private List<BalancingObject> connectedObjects = new List<BalancingObject>();
        private BalancingManager balancingManager;
        public Rigidbody2D Rigidbody => rigidbody;
        public event EventHandler<BalancingObject> OnCollisionHappened;
        public IReadOnlyList<BalancingObject> ConnectedObjects => connectedObjects;

        public bool IsTouched => isTouched;
        public bool IsDragging => dragListener.IsDragging;

        protected override void Awake()
        {
            base.Awake();
            balancingManager = FindObjectOfType<BalancingManager>();
        }

        private void Start()
        {
            balancingManager.AddBalancingObject(this);
        }

        private void OnDestroy()
        {
            balancingManager.RemoveBalancingObject(this);
        }

        protected override void Update()
        {
            base.Update();
            if (dragListener.IsDragging)
            {
                isTouched = true;
            }
        }

        public void SetDisplayOrder(int displayOrder)
        {
            spriteRenderer.sortingOrder = displayOrder;
        }

        protected override void HandleDragging()
        {
            if (isDraggable)
            {
                base.HandleDragging();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var otherObject = collision.gameObject.GetComponent<BalancingObject>();
            if (!connectedObjects.Contains(otherObject))
            {
                connectedObjects.Add(otherObject);
                OnCollisionHappened?.Invoke(this, this);
            }

        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            var otherObject = collision.gameObject.GetComponent<BalancingObject>();
            if (connectedObjects.Contains(otherObject))
            {
                connectedObjects.Remove(otherObject);
                OnCollisionHappened?.Invoke(this, this);
            }
        }
    }
}
