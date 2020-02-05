using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEngine
{
    public class UIPressDiaptcher: Button, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<PointerEventData> OnSelected;
        public event Action<PointerEventData> OnDeselected;


        private void Update()
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
//            OnSelected?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
//            OnDeselected?.Invoke(eventData);
        }
    }
}