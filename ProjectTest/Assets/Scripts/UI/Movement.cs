using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    public class Movement : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private RectTransform rect { get; set; }

        private Vector2 position { get; set; }

        private Vector2 screenPosition { get; set; }

        private void Awake()
        {
            rect = GetComponent<RectTransform>();

            position = rect.anchoredPosition + rect.sizeDelta/2;
            screenPosition = Vector2.zero;
        }

        public void Update()
        {
            if(screenPosition != Vector2.zero)
            {
                Direction();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            screenPosition = eventData.pointerCurrentRaycast.screenPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            screenPosition = Vector2.zero;
        }

        private void Direction()
        {
            /// Moving right
            if (position.x < screenPosition.x)
            {
                Debug.Log("Right");
            }

            /// Moving left
            if (position.x > screenPosition.x)
            {
                Debug.Log("Left");
            }

            /// Moving up
            if (position.y < screenPosition.y)
            {
                Debug.Log("Up");
            }

            /// Moving left
            if (position.y > screenPosition.y)
            {
                Debug.Log("Down");
            }
        }
    }
}
