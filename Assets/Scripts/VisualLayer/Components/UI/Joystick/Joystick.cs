using UnityEngine;
using UnityEngine.EventSystems;

namespace VisualLayer.Components.UI.Joystick
{
    public class Joystick: MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public bool IsPressed { get; private set; }
        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
            //Debug.Log("OnPointerDown");
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            return;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
            //Debug.Log("OnPointerUp");
        }

        
    }
}