using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class App_Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private TOSSP6 main;
    private Transform homeScreen;
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;

    public bool isDragging { get; private set; }

    private Transform parentAfterDrag;
    private int oldIndex;

    private static readonly List<RaycastResult> rayResult = new();

    private void Awake()
    {
        GameObject os = GameObject.FindGameObjectWithTag("TOSSP6");
        main = os.GetComponent<TOSSP6>();
        raycaster = GameObject.Find("OS").GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!main.isWiggleMode) return;

        LeanTween.scale(gameObject, Vector3.one * 1.25f, 0.5f).setEase(LeanTweenType.easeOutQuint);

        parentAfterDrag = transform.parent;
        oldIndex = transform.GetSiblingIndex();

        transform.SetParent(GameObject.Find("Home_Screen").transform);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!main.isWiggleMode) return;

        transform.position = Input.mousePosition;
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!main.isWiggleMode) return;
        isDragging = false;

        PointerEventData pointerData = new(eventSystem)
        {
            position = Input.mousePosition
        };

        rayResult.Clear();
        raycaster.Raycast(pointerData, rayResult);

        bool droppedOnTarget = false;

        foreach (var result in rayResult)
        {
            var direction = result.gameObject.GetComponent<App_Direction>();

            if (!direction) continue;

            switch (direction.direction)
            {
                case Direction.LEFT:
                case Direction.RIGHT:
                    transform.SetParent(result.gameObject.transform.parent.parent);
                    transform.SetSiblingIndex(direction.newIndex);
                    droppedOnTarget = true;
                    break;

                case Direction.BG:
                    transform.SetParent(result.gameObject.transform);
                    transform.SetSiblingIndex(direction.newIndex);
                    droppedOnTarget = true;
                    break;
            }

            if (droppedOnTarget) break;
        }

        if (!droppedOnTarget)
        {
            transform.SetParent(parentAfterDrag, true);
            transform.SetSiblingIndex(oldIndex);
        }

        LeanTween.scale(gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutQuint);
    }
}
