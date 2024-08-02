using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event EventHandler<OnButtonPressEventArgs> OnButtonPress;
    public class OnButtonPressEventArgs : EventArgs
    {
        public bool isPress { get; set; }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonPress?.Invoke(this, new OnButtonPressEventArgs { isPress = true });
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnButtonPress?.Invoke(this, new OnButtonPressEventArgs { isPress = false});
    }
}
