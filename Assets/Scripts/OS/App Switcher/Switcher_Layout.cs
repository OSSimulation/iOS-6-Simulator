using System;
using System.Collections.Generic;
using UnityEngine;

public class Switcher_Layout : MonoBehaviour
{
    public static event Action SwitcherAppsOverflow;

    [SerializeField] private PageTurner_Switcher pageTurner;
    [SerializeField] private TOSSP6 main;

    [Space(10)]
    [SerializeField] private GameObject pagePrefab;
    [SerializeField] private GameObject appPrefab;
    [SerializeField] private GameObject pageHolder;

    [SerializeField] private GameObject currentdbg;

    private List<Switcher_Page> appPages = new();
    private int appPageCount => appPages.Count;

    private int where = 0;

    private void Awake()
    {
        //App.AppOpened += CreateAppInSwitcher;
        App_ForceQuit.AppForceQuit += DestroyEmptyPage;

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

    }

    private void CreateAppInSwitcher()
    {
        InstantiateAppButton(main.openApps[0]);
    }

    private void InstantiateAppButton(AppObject app)
    {
        if (!DoesButtonExistForApp(app))
        {
            GameObject newButton = Instantiate(appPrefab, appPages[0].transform);

            App newAppComponent = newButton.GetComponent<App>();

            newButton.name = app.name;

            newAppComponent.app = app;

            newButton.transform.SetAsFirstSibling();
        }
        else
        {
            for (int i = 0; i < appPages.Count; i++)
            {
                Transform foundApp = appPages[i].transform.Find(app.name);
                if (foundApp != null)
                {
                    foundApp.SetParent(appPages[0].transform);
                    foundApp.SetAsFirstSibling();
                    break;
                }
            }
        }
    }

    private bool DoesButtonExistForApp(AppObject app)
    {
        for (int i = 0; i < appPages.Count; i++)
        {
            foreach (Transform child in appPages[i].transform)
            {
                App appComponent = child.GetComponent<App>();
                if (appComponent != null && appComponent.app == app)
                {
                    where = i;
                    return true;
                }
            }
        }

        return false;
    }

    private void HandlePageOrder()
    {
        if (main.openAppHolder.ContainsKey(main.currentOpenApp))
        {
            for (int i = 0; i < appPages.Count; i++)
            {
                Transform foundApp = appPages[i].transform.Find(main.currentOpenApp);

                if (foundApp != null)
                {
                    foundApp.SetParent(appPages[0].transform);

                    foundApp.SetAsFirstSibling();

                    break;
                }
            }
        }

        // Overflow
        Debug.Log("AppOpened event fired1");

        for (int i = 0; i < appPages.Count; i++)
        {
            Debug.Log("AppOpened event fired2");

            Switcher_Page currentPage = appPages[i];

            if (currentPage.type != PageType.APP_HOLDER)
                continue;

            currentdbg = currentPage.gameObject;

            if (currentPage.appCount > 3)
            {
                Debug.Log("AppOpened event fired3");

                CreateNewPage();

                Transform lastApp = currentPage.transform.GetChild(currentPage.transform.childCount - 1);
                lastApp.SetParent(appPages[i + 1].transform);
            }
        }

        // Underflow
    }

    private void CreateNewPage()
    {
        GameObject newPage = Instantiate(pagePrefab, pageHolder.transform);
        Debug.Log("hi");
        appPages.Add(newPage.GetComponent<Switcher_Page>());
        newPage.transform.SetAsLastSibling();

        SwitcherAppsOverflow?.Invoke();
    }

    private void DestroyEmptyPage()
    {
        foreach (Transform child in pageHolder.transform)
        {
            Switcher_Page pageComponent = child.GetComponent<Switcher_Page>();
            if (pageComponent == null)
                continue;

            if (pageComponent.type != PageType.APP_HOLDER)
                continue;

            if (pageComponent.isEmpty && pageComponent.transform.GetSiblingIndex() > 2)
            {
                appPages.Remove(pageComponent);
                Destroy(child.gameObject);
                pageTurner.GoToPage(appPageCount - 1);
            }
        }
    }
}
