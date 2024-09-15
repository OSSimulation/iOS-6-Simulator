using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlideToUnlock : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public static event Action SliderMovedToEnd;

    [SerializeField] private GameObject startGO;
    [SerializeField] private GameObject endGO;

    float start;
    float end;

    private void Start()
    {
        TOSSP6.LockDevice += ResetSlider;
    }

    private void Update()
    {
        start = startGO.transform.localPosition.x + 62f;
        end = endGO.transform.localPosition.x - 62f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 position = transform.localPosition;
        transform.localPosition = new Vector3(Mathf.Clamp(position.x + eventData.delta.x, start, end), position.y, position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 position = transform.localPosition;

        if (position.x < end && position.x > start - 1f)
        {
            StartCoroutine(SmoothReset());
            return;
        }

        SliderMovedToEnd?.Invoke();
    }

    private void ResetSlider()
    {
        Vector3 position = transform.localPosition;
        transform.localPosition = new Vector3(start, position.y, position.z);
    }

    private IEnumerator SmoothReset()
    {
        Vector2 startPos = transform.localPosition;
        Vector2 targetPos = new Vector2(start, startPos.y);
        float elapsedTime = 0;
        float duration = 0.1f;

        while (elapsedTime < duration)
        {
            transform.localPosition = Vector2.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPos;
    }
}
