using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_NotificationCentre : MonoBehaviour, IDragHandler, IEndDragHandler
{
    TOSSP6 main;

    private Vector3 panelLocation;
    [SerializeField] private Transform screenTop;
    [SerializeField] private Transform screenBottom;
    [SerializeField] private GameObject notificationCentreObject;
    [SerializeField] private GameObject statusBarDragger;
    [SerializeField] private float percentThreshold = 0.25f;
    [SerializeField] private float easing = 0.25f;
    [SerializeField] private int totalPages;
    public Transform[] pageObjects;
    public int currentPage = 1;

    public static event Action ShowNotificationCentre;
    public static event Action HideNotificationCentre;

    private void Start()
    {
        main = GetComponentInParent<TOSSP6>();

        panelLocation = transform.position;

        totalPages = this.transform.childCount;
    }

    public void OnDrag(PointerEventData data)
    {
        if (!main.isSystemLocked)
        {
            HideNotificationCentre?.Invoke();

            if (pageObjects.Length > 1)
            {
                float difference = data.pressPosition.y - data.position.y;
                transform.position = panelLocation - new Vector3(0, difference, 0);
            }

            if (notificationCentreObject.transform.position.y < screenBottom.position.y + Screen.height / 2)
            {
                data.pointerDrag = null;
                GoToPage(2);
                ShowNotificationCentre?.Invoke();
            }
            else if (notificationCentreObject.transform.position.y > screenTop.position.y + Screen.height / 2 && currentPage != 1 || statusBarDragger.transform.position.y > screenTop.position.y)
            {
                data.pointerDrag = null;
                GoToPage(1);
                HideNotificationCentre?.Invoke();
            }
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (!main.isSystemLocked)
        {
            float percentage = (data.pressPosition.y - data.position.y) / Screen.height;
            if (Mathf.Abs(percentage) >= percentThreshold)
            {
                Vector3 newLocation = panelLocation;
                if (percentage > 0 && currentPage < totalPages)
                {
                    currentPage++;
                    newLocation += new Vector3(0, -Screen.height, 0);
                    ShowNotificationCentre?.Invoke();
                }
                else if (percentage < 0 && currentPage > 1)
                {
                    currentPage--;
                    newLocation += new Vector3(0, Screen.height, 0);
                    HideNotificationCentre?.Invoke();
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

        if (currentPage == 2)
        {
            main.isInNotificationCentre = true;
            ShowNotificationCentre?.Invoke();
        }
        else if (currentPage == 1)
        {
            main.isInNotificationCentre = false;
            HideNotificationCentre?.Invoke();
        }
    }

    public void GoToPage(int pageNumber)
    {
        int targetPage = Mathf.Clamp(pageNumber, 1, totalPages);
        Vector3 newLocation = panelLocation + new Vector3(0, (currentPage - targetPage) * Screen.height, 0);
        StartCoroutine(SmoothMove(transform.position, newLocation, easing));
        panelLocation = newLocation;
        currentPage = targetPage;
    }

    public void InvokeHideNotificationCentre()
    {
        HideNotificationCentre?.Invoke();
    }
}
