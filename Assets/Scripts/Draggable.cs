using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event Action<Vector2> DragStartEvent;
    public event Action<Vector2, Vector2> DragUpdateEvent;
    public event Action<Vector2> DragEndEvent;


    private Vector2 dragFrom;
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragFrom = eventData.position;
        DragStartEvent?.Invoke(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragUpdateEvent?.Invoke(eventData.position, dragFrom);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragEndEvent?.Invoke(eventData.position);
    }
}