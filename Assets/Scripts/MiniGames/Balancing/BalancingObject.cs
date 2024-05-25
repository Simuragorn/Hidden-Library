using UnityEngine;

namespace Assets.Scripts.MiniGames.Balancing
{
    public class BalancingObject : DraggableObject
    {
        [SerializeField] private bool isTouched;
        [SerializeField] private bool isDraggable = true;

        public bool IsTouched => isTouched;
        public bool IsDragging => dragListener.IsDragging;

        protected override void Update()
        {
            base.Update();
            if (dragListener.IsDragging)
            {
                isTouched = true;
            }
        }

        protected override void HandleDragging()
        {
            if (isDraggable)
            {
                base.HandleDragging();
            }
        }
    }
}
