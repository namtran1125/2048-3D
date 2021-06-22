using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityAction OnPointerDownEvent;
    public UnityAction<float> OnPointerDragEvent;
    public UnityAction OnPointerUpEvent;

    private Slider uiSlider;

    private void Awake()
    {
        uiSlider = GetComponent<Slider>();
        uiSlider.onValueChanged.AddListener(OnSliderValueChange);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnPointerDownEvent != null)
            OnPointerDownEvent.Invoke();
        if (OnPointerDragEvent != null)
            OnPointerDragEvent.Invoke(uiSlider.value);
    }

    private void OnSliderValueChange(float _value)
    {
        if (OnPointerDragEvent != null)
            OnPointerDragEvent.Invoke(_value);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnPointerUpEvent != null)
            OnPointerUpEvent.Invoke();

        //Reset slider value
        uiSlider.value = 0f;
    }

    private void OnDestroy()
    {
        uiSlider.onValueChanged.RemoveListener(OnSliderValueChange);
    }

}
