using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageTurner_Home : MonoBehaviour, IDragHandler, IEndDragHandler
{
    TOSSP6 main;

    private Vector3 panelLocation;
    [SerializeField] private float percentThreshold = 0.25f;
    [SerializeField] private float easing = 0.25f;
    [SerializeField] private int totalPages;
    public Transform[] pageObjects;
    public List<Transform> pages = new List<Transform>();
    public int currentPage = 1;

    private void Start()
    {
        TOSSP6.DeviceUnlocked += PlayZoom;
        App.AppOpened += PlayZoomOut;

        main = GetComponentInParent<TOSSP6>();

        panelLocation = transform.position;

        totalPages = this.transform.childCount;

        foreach (Transform child in transform)
        {
            pages.Add(child);
        }

        pageObjects = pages.ToArray();
    }

    private void Update()
    {
        DeactivateEmptyPages();
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
            Vector3 newLocation = panelLocation;
            if (percentage > 0 && currentPage < totalPages)
            {
                currentPage++;
                newLocation += new Vector3(-Screen.width, 0, 0);
            }
            else if (percentage < 0 && currentPage > 1)
            {
                currentPage--;
                newLocation += new Vector3(Screen.width, 0, 0);
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }

        if (pageObjects.Length == 1)
        {
            GoToPage(1);
        }
    }

    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }

    public void GoToPage(int pageNumber)
    {
        int targetPage = Mathf.Clamp(pageNumber, 1, totalPages);
        Vector3 newLocation = panelLocation + new Vector3((currentPage - targetPage) * Screen.width, 0, 0);
        StartCoroutine(SmoothMove(transform.position, newLocation, easing));
        panelLocation = newLocation;
        currentPage = targetPage;
    }

    private void DeactivateEmptyPages()
    {
        List<Transform> pagesToRemove = new List<Transform>();

        foreach (Transform page in pageObjects)
        {
            if (page.childCount == 0)
            {
                page.gameObject.SetActive(false);
                pagesToRemove.Add(page);
            }
        }

        foreach (Transform pageToRemove in pagesToRemove)
        {
            pages.Remove(pageToRemove);
        }

        pageObjects = pages.ToArray();
    }

    public void PlayZoom()
    {
        if (!main.isInApp)
        {
            pages[currentPage - 1].gameObject.GetComponent<Animator>().Play("Page_Zoom_In");
        }
    }

    public void PlayZoomOut()
    {
        pages[currentPage - 1].gameObject.GetComponent<Animator>().Play("Page_Zoom_Out");
    }
}
