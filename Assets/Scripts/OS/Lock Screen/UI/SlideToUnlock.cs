using OS6.Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlideToUnlock : MonoBehaviour, IDragHandler, IEndDragHandler
{
    //public static event Action SliderMovedToEnd;

    [Space(10)]
    [SerializeField] private RectTransform startGO;
    [SerializeField] private RectTransform endGO;
    [SerializeField] private RectTransform handle;
    [SerializeField] private bool shouldReset;

    float start;
    float end;

    public static Action SliderMovedToEnd;

    private void Start()
    {
        TOSSP6.LockDevice += ResetSlider;
        LockScreen.BioReject += ResetSlider;
    }

    private void OnRectTransformDimensionsChange()
    {
        start = startGO.localPosition.x + (handle.rect.width / 2f);
        end = endGO.localPosition.x - (handle.rect.width / 2f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(handle.parent as RectTransform,
            eventData.position, eventData.pressEventCamera, out Vector2 position);

        float x = Mathf.Clamp(position.x + eventData.delta.x, start, end);
        handle.localPosition = new Vector2(x, handle.localPosition.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 position = transform.localPosition;

        if (position.x == end)
        {
            GlobalEvents.PublishEventMessage("LSSLIDER_MOVED_TO_END");

            SliderMovedToEnd?.Invoke();
            StartCoroutine(ResetDelay());
            return;
        }

        SmoothReset();
        return;
    }

    private IEnumerator ResetDelay()
    {
        yield return new WaitForSeconds(0.5f);
        ResetSlider();
    }

    private void ResetSlider()
    {
        if (!shouldReset) return;

        Vector2 position = transform.localPosition;
        transform.localPosition = new Vector3(start, position.y);
    }

    private void SmoothReset()
    {
        LeanTween.moveLocalX(gameObject, start, 0.1f);
    }
}
