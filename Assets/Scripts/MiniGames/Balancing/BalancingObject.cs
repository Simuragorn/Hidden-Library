using Assets.Scripts.Consts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MiniGames.Balancing
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Animator))]
    public class BalancingObject : DraggableObject
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private bool isTouched;
        private Collider2D collider;
        private Animator animator;

        private List<BalancingObject> connectedObjects = new List<BalancingObject>();
        private BalancingManager balancingManager;
        public Rigidbody2D Rigidbody => rigidbody;
        public event EventHandler<BalancingObject> OnCollisionHappened;
        public IReadOnlyList<BalancingObject> ConnectedObjects => connectedObjects;

        public bool IsTouched => isTouched;
        public bool IsDragging => dragListener != null && dragListener.IsDragging;

        protected override void Awake()
        {
            base.Awake();
            collider = GetComponent<Collider2D>();
            DisablePhysics();
            if (dragListener != null)
            {
                dragListener.OnDragStarted += DragListener_OnDragStarted;
            }
            balancingManager = FindObjectOfType<BalancingManager>();
            animator = GetComponent<Animator>();
            SetDisplayOrder(balancingManager.DefaultStaticObjectDisplayOrder);
        }

        public void DisablePhysics()
        {
            rigidbody.bodyType = RigidbodyType2D.Static;
            collider.enabled = false;
        }

        private void DragListener_OnDragStarted(object sender, EventArgs e)
        {
            SetDisplayOrder(balancingManager.DefaultDraggingObjectDisplayOrder);
            EnablePhysics();
            isTouched = true;
            animator.SetBool(AnimationConsts.BalancingObject.IsTouchednTriggerName, true);
        }

        private void EnablePhysics(bool isBase = false)
        {
            rigidbody.bodyType = isBase ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
            collider.enabled = true;
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
        }

        public void SetAsBalancingBaseObject()
        {
            isDraggable = false;
            EnablePhysics(true);
            Vector2 bottomVisiblePoint = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0));
            transform.position = new Vector2(bottomVisiblePoint.x, bottomVisiblePoint.y + spriteRenderer.size.y);
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
