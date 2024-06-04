using Assets.Scripts.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.MiniGames.Balancing
{
    [RequireComponent(typeof(Animator))]
    public class BalancingObject : DraggableObject
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] protected SpriteRenderer highlighting;
        [SerializeField] private TriggerEmitter2D innerBodyTrigger;
        private List<Collider2D> physicalColliders;
        private bool isTouched;
        private Animator animator;

        [SerializeField] private List<BalancingObject> innerBalancingObjects = new();
        private BalancingManager balancingManager;
        private bool isBaseObject = false;
        public Rigidbody2D Rigidbody => rigidbody;
        public event EventHandler<BalancingObject> OnCollisionHappened;
        public event EventHandler<BalancingObject> OnDragStarted;
        public IReadOnlyList<BalancingObject> InnerBalancingObjects => innerBalancingObjects;
        public int DisplayOrder => spriteRenderer.sortingOrder;

        public bool IsTouched => isTouched;
        public bool IsDragging => dragListener != null && dragListener.IsDragging;
        private Vector2 basePosition;
        public Vector2 BasePosition => basePosition;

        protected override void Awake()
        {
            base.Awake();
            basePosition = transform.position;
            physicalColliders = GetComponentsInChildren<Collider2D>()
                .Where(c => !c.isTrigger).ToList();
            DisablePhysics();
            if (dragListener != null)
            {
                dragListener.OnDragStarted += DragListener_OnDragStarted;
            }
            balancingManager = FindObjectOfType<BalancingManager>();
            animator = GetComponent<Animator>();
            SetDisplayOrder(balancingManager.DefaultStaticObjectDisplayOrder);
            if (highlighting != null)
            {
                highlighting.gameObject.SetActive(false);
            }
            innerBodyTrigger.OnTriggerEnter += InnerBodyTrigger_OnTriggerEnter;
            innerBodyTrigger.OnTriggerExit += InnerBodyTrigger_OnTriggerExit;
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
            HandleHighlight();
            HandleReturningInput();
            HandleReturning();
        }

        private void OnDrawGizmos()
        {
            if (rigidbody != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(rigidbody.worldCenterOfMass, 0.5f);
                Gizmos.color = Color.yellow;
            }
        }

        private void HandleReturningInput()
        {
            if (IsDragging && Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                SetDisplayOrder(balancingManager.DefaultStaticObjectDisplayOrder);
                DisablePhysics();
                isTouched = false;
                animator.SetBool(AnimationConsts.BalancingObject.IsTouchedValueName, false);
            }
        }

        private void HandleReturning()
        {
            if (!isTouched && !isBaseObject)
            {
                bool moved = Vector2.Distance(transform.position, basePosition) > CalculationConsts.DistanceOffset;
                if (moved)
                {
                    transform.position = Vector2.MoveTowards(transform.position, basePosition, balancingManager.ReturningSpeed * Time.deltaTime);
                }
                if (transform.rotation != Quaternion.identity)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, balancingManager.ReturningRotationSpeed * Time.deltaTime);
                }
            }
        }

        private void HandleHighlight()
        {
            if (highlighting == null)
            {
                return;
            }
            Vector2 mousePosition = dragListener.GetMousePosition();
            bool highlightNeeded = !isTouched && dragListener.DraggingCollider.bounds.Contains(mousePosition);
            if (isHighlighted != highlightNeeded)
            {
                isHighlighted = highlightNeeded;
                highlighting.gameObject.SetActive(isHighlighted);
            }
        }

        public float GetMassCenterWorldPosY()
        {
            return rigidbody.worldCenterOfMass.y;
        }

        private void DragListener_OnDragStarted(object sender, EventArgs e)
        {
            OnDragStarted?.Invoke(this, this);
            if (!IsTouched)
            {
                SetDisplayOrder(balancingManager.DefaultDraggingObjectDisplayOrder);
                EnablePhysics();
                isTouched = true;
                animator.SetBool(AnimationConsts.BalancingObject.IsTouchedValueName, true);
            }
        }

        public void DisablePhysics()
        {
            rigidbody.bodyType = RigidbodyType2D.Static;
            innerBodyTrigger.enabled = false;
            foreach (var collider in physicalColliders)
            {
                collider.enabled = false;
            }
        }

        private void EnablePhysics(bool isBase = false)
        {
            rigidbody.bodyType = isBase ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
            innerBodyTrigger.enabled = true;
            foreach (var collider in physicalColliders)
            {
                collider.enabled = true;
            }
            rigidbody.centerOfMass = Vector2.zero;
        }

        public void SetAsBalancingBaseObject()
        {
            isDraggable = false;
            isBaseObject = true;
            EnablePhysics(true);
            Vector2 bottomVisiblePoint = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0));
            transform.position = new Vector2(bottomVisiblePoint.x, bottomVisiblePoint.y + spriteRenderer.size.y / 2);
            basePosition = transform.position;
            var pusher = FindObjectOfType<BalancingObjectPusher>();
            pusher.SetTarget(this);
        }

        public void SetDisplayOrder(int displayOrder)
        {
            spriteRenderer.sortingOrder = displayOrder;
            if (highlighting != null)
            {
                highlighting.sortingOrder = displayOrder - 1;
            }
        }

        protected override void HandleDragging()
        {
            if (isDraggable)
            {
                base.HandleDragging();
            }
        }

        private void InnerBodyTrigger_OnTriggerExit(object sender, Collider2D collision)
        {
            var otherObject = collision.gameObject.GetComponentInParent<BalancingObject>();
            if (otherObject != null && innerBalancingObjects.Contains(otherObject))
            {
                innerBalancingObjects.Remove(otherObject);
                OnCollisionHappened?.Invoke(this, this);
            }
        }

        private void InnerBodyTrigger_OnTriggerEnter(object sender, Collider2D collision)
        {
            var otherObject = collision.gameObject.GetComponentInParent<BalancingObject>();
            if (otherObject != null && otherObject != this && !innerBalancingObjects.Contains(otherObject))
            {
                innerBalancingObjects.Add(otherObject);
                OnCollisionHappened?.Invoke(this, this);
            }
        }
    }
}
