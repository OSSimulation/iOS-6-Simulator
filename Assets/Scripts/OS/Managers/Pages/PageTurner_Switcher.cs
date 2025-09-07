using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageTurner_Switcher : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation;
    [SerializeField] private float percentThreshold = 0.25f;
    [SerializeField] private int totalPages;
    [SerializeField] private Transform[] pageObjects => pages.ToArray();
    private List<Transform> pages = new();
    public int currentPage;

    private void Start()
    {
        Switcher_Layout.SwitcherAppsOverflow += AddPages;

        panelLocation = transform.position;

        totalPages = transform.childCount;

        foreach (Transform child in transform)
        {
            pages.Add(child);
        }

        currentPage = pageObjects.Length;
    }

    private void AddPages()
    {
        Transform page = transform.GetChild(transform.childCount - 1);
        page.localPosition = new Vector2(pageObjects[totalPages - 1].transform.localPosition.x + 640, 0f);

        pages.Add(page);
        totalPages = transform.childCount;
    }

    public void OnDrag(PointerEventData data)
    {
        if (pageObjects.Length > 1)
        {
            float difference = data.pressPosition.x - data.position.x;
            transform.position = panelLocation - new Vector3(difference, 0, 0);
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            float newX = panelLocation.x;
            if (percentage > 0 && currentPage < totalPages)
            {
                currentPage++;
                newX -= Screen.width;
            }
            else if (percentage < 0 && currentPage > 1)
            {
                currentPage--;
                newX += Screen.width;
            }
            SmoothMove(transform.position.x, newX);
            panelLocation.x = newX;
        }
        else
        {
            SmoothMove(transform.position.x, panelLocation.x);
        }

        if (pageObjects.Length == 1)
        {
            GoToPage(1);
        }
    }

    void SmoothMove(float endX, float moveTime = 0.5f)
    {
        Vector3 endPos = new Vector3(endX, transform.position.y, transform.position.z);
        LeanTween.move(gameObject, endPos, moveTime).setEase(LeanTweenType.easeOutQuint);
    }

    public void GoToPage(int pageNumber, float moveTime = 0.5f)
    {
        int targetPage = Mathf.Clamp(pageNumber, 1, totalPages);
        float newX = panelLocation.x + (currentPage - targetPage) * Screen.width;
        SmoothMove(newX, moveTime);
        panelLocation.x = newX;
        currentPage = targetPage;
    }
}