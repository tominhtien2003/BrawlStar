using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event EventHandler<OnButtonShootPressEventArgs> OnButtonShootPress;
    public class OnButtonShootPressEventArgs : EventArgs
    {
        public bool isPress { get; set; }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonShootPress?.Invoke(this, new OnButtonShootPressEventArgs { isPress = true });
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnButtonShootPress?.Invoke(this, new OnButtonShootPressEventArgs { isPress = false });
    }
}
