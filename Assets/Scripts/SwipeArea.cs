using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public delegate void SwipeDelegate(Vector3 mouseDirection);
    public event SwipeDelegate OnSwipe;

    private Vector3 lastMousePosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        lastMousePosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        lastMousePosition = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (lastMousePosition != Vector3.zero)
        {
            Vector3 currentMousePosition = eventData.position;
            Vector3 mouseDirection = (currentMousePosition - lastMousePosition).normalized;
            lastMousePosition = currentMousePosition;
            OnSwipe?.Invoke(mouseDirection);
        }
    }
}
