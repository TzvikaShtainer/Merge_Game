using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace VisualLayer.Components.UI.Joystick
{
    public class Joystick: MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public event Action OnReleased;
        public bool IsPressed { get; private set; }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            return;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
            OnReleased?.Invoke();
        }

        
    }
}