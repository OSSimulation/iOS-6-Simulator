using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageTurner_Home : MonoBehaviour, IDragHandler, IEndDragHandler
{
    TOSSP6 main;

    private Vector3 panelLocation;
    [SerializeField] private float percentThreshold = 0.25f;
    [SerializeField] private int totalPages;
    public Transform[] pageObjects => pages.ToArray();
    public List<Transform> pages = new();
    public int currentPage = 1;

    public static event Action WiggleEmptyPage;

    private void Start()
    {
        TOSSP6.DeviceUnlocked += PlayZoom;
        App.AppOpened += PlayZoomOut;
        Home_Layout.AppsLoaded += AddPages;
        Home_Layout.AppsOverflow += AddPagesAfterLoad;
        TOSSP6.WiggleStop += RebuildPageCaller;

        main = GetComponentInParent<TOSSP6>();

        panelLocation = transform.position;
    }

    private void AddPages()
    {
        foreach (Transform child in transform)
        {
            pages.Add(child);
        }

        pages[0].transform.localPosition = new Vector2(-640, 0);

        float xPos = 0;

        for (int i = 1; i < pages.Count; i++)
        {
            pages[i].transform.localPosition = new Vector2(xPos, 0);
            xPos += 640;
        }

        totalPages = transform.childCount;
    }

    private void AddPagesAfterLoad()
    {
        Transform page = transform.GetChild(transform.childCount - 1);
        page.localPosition = new Vector2(pageObjects[totalPages - 1].transform.localPosition.x + 640, 0f);

        pages.Add(page);
        totalPages = transform.childCount;
    }

    private void RebuildPageCaller()
    {
        StartCoroutine(RebuildPageList());
    }

    IEnumerator RebuildPageList()
    {
        yield return new WaitForSeconds(0.5f);

        pages.Clear();

        foreach (Transform child in transform)
        {
            pages.Add(child);
        }

        totalPages = transform.childCount;
    }

    private void Update()
    {
        if (main.isWiggleMode)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                NextPage();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PreviousPage();
            }
        }
    }

    private void NextPage()
    {
        if (currentPage < pages.Count)
        {
            GoToPage(currentPage + 1);
        }
        else if (currentPage == pages.Count && !pages[currentPage - 1].GetComponent<Home_Page>().isEmpty)
        {
            WiggleEmptyPage?.Invoke();

            GoToPage(currentPage + 1);
        }
    }

    private void PreviousPage()
    {
        if (currentPage != 2)
        {
            GoToPage(currentPage - 1);
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (pageObjects.Length > 1)
        {
            float difference = data.pressPosition.x - data.position.x;
            transform.position = panelLocation - new Vector3(difference, 0, 0);

            if (difference < 0 && currentPage == 2 && main.isWiggleMode)
            {
                data.dragging = false;
                GoToPage(2);
            }
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

        if (pageObjects.Length == 2)
        {
            GoToPage(2);
        }
    }

    void SmoothMove(float startX, float endX)
    {
        Vector3 endPos = new Vector3(endX, 663, transform.position.z);
        LeanTween.move(gameObject, endPos, 0.5f).setEase(LeanTweenType.easeOutQuint);
    }

    public void GoToPage(int pageNumber)
    {
        int targetPage = Mathf.Clamp(pageNumber, 1, totalPages);
        float newX = panelLocation.x + (currentPage - targetPage) * Screen.width;
        SmoothMove(transform.position.x, newX);
        panelLocation.x = newX;
        currentPage = targetPage;
    }

    public void PlayZoom()
    {
        if (!main.isInApp)
        {
            pages[currentPage - 1].gameObject.GetComponent<Page_Anim>().PageZoomIn();
        }
    }

    public void PlayZoomOut()
    {
        pages[currentPage - 1].gameObject.GetComponent<Page_Anim>().PageZoomOut();
    }
}
