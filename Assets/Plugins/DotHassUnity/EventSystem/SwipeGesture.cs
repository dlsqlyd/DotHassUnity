using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GYJ
{
    public enum SwipeDirection
    {
        Up,
        Down,
        Right,
        Left
    }


    public class SwipeGesture : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Vector2 startPos;


        public delegate void SwipeDelegate(SwipeDirection direction);


        public SwipeDelegate OnSwipe;

        public void OnBeginDrag(PointerEventData eventData)
        {
            startPos = eventData.pressPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Vector3 dragVectorDirection = (eventData.position - startPos).normalized;
            var direction = GetDragDirection(dragVectorDirection);

            if (OnSwipe != null)
                OnSwipe.Invoke(direction);
        }

        private SwipeDirection GetDragDirection(Vector3 dragVector)
        {
            float positiveX = Mathf.Abs(dragVector.x);
            float positiveY = Mathf.Abs(dragVector.y);
            SwipeDirection draggedDir;
            if (positiveX > positiveY)
            {
                draggedDir = (dragVector.x > 0) ? SwipeDirection.Right : SwipeDirection.Left;
            }
            else
            {
                draggedDir = (dragVector.y > 0) ? SwipeDirection.Up : SwipeDirection.Down;
            }
            return draggedDir;
        }

        public void OnDrag(PointerEventData eventData)
        {

        }
    }
}
