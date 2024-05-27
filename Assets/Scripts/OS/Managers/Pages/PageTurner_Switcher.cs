using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageTurner_Switcher : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation;
    [SerializeField] private float percentThreshold = 0.25f;
    [SerializeField] private float easing = 0.25f;
    [SerializeField] private int totalPages;
    [SerializeField] private Transform[] pageObjects;
    private List<Transform> pages = new List<Transform>();
    public int currentPage;

    private void Start()
    {
        panelLocation = transform.position;

        totalPages = this.transform.childCount;

        foreach (Transform child in transform)
        {

            pages.Add(child);
        }

        pageObjects = pages.ToArray();

        currentPage = pageObjects.Length;
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
}