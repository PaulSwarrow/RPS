using System;
using UnityEngine.EventSystems;

namespace UnityEngine
{
    public class CanvasController : MonoBehaviour, IPointerDownHandler,  IPointerUpHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            
        }

        public event Action<PointerEventData> PointerUpEvent;

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerUpEvent?.Invoke(eventData);
        }
    }
}