using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlideToUnlock : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public static event Action SliderMovedToEnd;

    private void Start()
    {
        TOSSP6.LockDevice += ResetSlider;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 position = transform.localPosition;
        transform.localPosition = new Vector3(Mathf.Clamp(position.x + eventData.delta.x, -165, 165), position.y, position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 position = transform.localPosition;

        if (position.x < 165 && position.x > -166)
        {
            transform.localPosition = new Vector3(-165, position.y, position.z);
            return;
        }

        SliderMovedToEnd?.Invoke();
    }

    private void ResetSlider()
    {
        Vector3 position = transform.localPosition;
        transform.localPosition = new Vector3(-165, position.y, position.z);
    }
}
