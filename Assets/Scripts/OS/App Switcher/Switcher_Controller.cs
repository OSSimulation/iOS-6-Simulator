using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Switcher_Controller : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private TOSSP6 main;

    [Space(10)]
    private Vector3 panelLocation;
    [SerializeField] private float percentThreshold = 0.25f;
    [SerializeField] private int totalPages;
    public int currentPage;
    public Transform[] pageObjects => pages.ToArray();
    public List<Transform> pages = new();

    [Space(10)]
    [SerializeField] private GameObject pagePrefab;
    [SerializeField] private GameObject appPrefab;
    [SerializeField] private GameObject pageHolder;

    private List<Switcher_Page> appPages = new();

    private void Awake()
    {
        App.AppOpened += CreateAppInSwitcher;
        App_ForceQuit.AppForceQuit += HandleUnderflow;

        foreach (Transform child in pageHolder.transform)
        {
            Switcher_Page page = child.GetComponent<Switcher_Page>();
            if (page != null && page.type == PageType.APP_HOLDER)
            {
                appPages.Add(page);
            }
        }
    }

    private void Start()
    {
        panelLocation = transform.position;

        totalPages = transform.childCount;

        foreach (Transform child in transform)
        {

            pages.Add(child);
        }

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
            SmoothMove(newX);
            panelLocation.x = newX;
        }
        else
        {
            SmoothMove(panelLocation.x);
        }

        if (pageObjects.Length == 2)
        {
            GoToPage(2);
        }
    }

    private void SmoothMove(float endX, float moveTime = 0.5f)
    {
        Vector3 endPos = new(endX, transform.position.y, transform.position.z);
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

    private void CreateAppInSwitcher()
    {
        InstantiateAppButton(main.openApps[0]);
    }

    private void InstantiateAppButton(AppObject app)
    {
        if (!DoesButtonExistForApp(app))
        {
            GameObject newButton = Instantiate(appPrefab, appPages[0].holder);
            App newAppComponent = newButton.GetComponent<App>();

            newButton.name = app.name;
            newAppComponent.app = app;

            newButton.transform.SetAsFirstSibling();
        }
        else
        {
            for (int i = 0; i < appPages.Count; i++)
            {
                Transform foundApp = appPages[i].holder.Find(app.name);
                if (foundApp != null)
                {
                    foundApp.SetParent(appPages[0].holder);
                    foundApp.SetAsFirstSibling();
                    break;
                }
            }
        }

        HandleOverflow();
    }

    private bool DoesButtonExistForApp(AppObject app)
    {
        for (int i = 0; i < appPages.Count; i++)
        {
            foreach (Transform child in appPages[i].holder)
            {
                App appComponent = child.GetComponent<App>();
                if (appComponent != null && appComponent.app == app)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void HandleOverflow()
    {
        bool isSorting = true;

        while (isSorting)
        {
            isSorting = false;

            for (int i = 0; i < appPages.Count; i++)
            {
                Switcher_Page currentPage = appPages[i];

                if (currentPage.type != PageType.APP_HOLDER)
                    continue;

                if (currentPage.appCount > 4)
                {
                    if (i + 1 >= appPages.Count)
                        CreateNewPage();

                    while (currentPage.appCount > 4)
                    {
                        Transform lastChild = currentPage.holder.GetChild(currentPage.holder.childCount - 1);
                        lastChild.SetParent(appPages[i + 1].holder);
                        lastChild.SetAsFirstSibling();
                    }

                    isSorting = true;

                    if (currentPage.appCount == 0)
                        break;
                }
            }
        }
    }

    private void HandleUnderflow()
    {
        bool isSorting = true;

        int maxIterations = 1000;
        int iterationCount = 0;


        while (isSorting && iterationCount++ < maxIterations)
        {
            isSorting = false;

            for (int i = 0; i < appPages.Count - 1; i++)
            {
                Switcher_Page currentPage = appPages[i];
                Switcher_Page nextPage = appPages[i + 1];

                if (currentPage.type != PageType.APP_HOLDER || nextPage == null)
                    continue;

                if (nextPage.appCount > 0)
                {
                    Transform firstChild = nextPage.holder.GetChild(0);
                    firstChild.SetParent(currentPage.holder.transform);
                    firstChild.SetAsLastSibling();
                }
            }
        }

        DestroyEmptyPage();
    }


    private void CreateNewPage()
    {
        GameObject newPage = Instantiate(pagePrefab, pageHolder.transform);
        appPages.Add(newPage.GetComponent<Switcher_Page>());
        newPage.transform.SetAsLastSibling();
        newPage.transform.localPosition = new Vector2(pageObjects[totalPages - 1].transform.localPosition.x + 640, 112.5f);

        pages.Add(newPage.transform);
        totalPages = transform.childCount;
    }

    private void DestroyEmptyPage()
    {
        for (int i = appPages.Count - 1; i > 0; i--)
        {
            Switcher_Page current = appPages[i];

            if (current.isEmpty)
            {
                pages.Remove(current.transform);
                appPages.Remove(current);
                Destroy(current.gameObject);

                totalPages = pages.Count;

                if (currentPage == i && currentPage > 3)
                {
                    GoToPage(i - 1);
                }
            }
        }
    }
}
