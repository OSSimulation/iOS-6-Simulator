using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Home_Layout : MonoBehaviour
{
    public static event Action AppsLoaded;
    public static event Action AppsOverflow;

    [SerializeField] private PageTurner_Home pageTurner;

    [Space(10)]
    [SerializeField] private TextAsset defaultLayout;
    [SerializeField] private GameObject pagePrefab;
    [SerializeField] private GameObject appPrefab;

    [Space(10)]
    [SerializeField] private Home_Dock dock;
    private List<App> dockApps = new();

    [Space(10)]
    [SerializeField] private GameObject pageHolder;
    private int pageCount = 0;
    private List<Home_Page> pages = new();

    Dictionary<string, List<string>> layoutDictionary = new();

    private string layoutFileName = "com.os6.iconState.json";
    private string layoutFilePath => System_Locations.SYS_PREFERENCES + layoutFileName;

    private void Awake()
    {
        TOSSP6.WiggleStop += DestroyEmptyPage;

        PageTurner_Home.WiggleEmptyPage += CreateNewPage;

        LayoutFileExists();

        string json = File.ReadAllText(layoutFilePath);
        Dictionary<string, List<string>> layoutData = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

        int pageIndex = 1;
        while (true)
        {
            string key = $"page{pageIndex}";

            if (!layoutData.ContainsKey(key))
                break;

            pageCount++;
            pageIndex++;
        }
    }

    private void Start()
    {
        LoadAppLayout();
    }

    private void Update()
    {
        HandlePageOverflow();
    }

    private void LayoutFileExists()
    {
        if (!File.Exists(layoutFilePath))
        {
            File.WriteAllText(layoutFilePath, defaultLayout.text);
            Debug.Log("Creating iconState file...");
        }
    }

    private void SaveData()
    {
        //Dock
        dockApps.Clear();

        foreach (Transform app in dock.AppHolder.transform)
        {
            if (app.GetComponent<App>())
            {
                App appObj = app.GetComponent<App>();
                dockApps.Add(appObj);
            }
        }

        List<string> dockOrder = new();

        foreach (App app in dockApps)
        {
            if (app.app != null)
            {
                string appName = app.app.appName + ".asset";
                if (!dockOrder.Contains(appName))
                {
                    dockOrder.Add(appName);
                }
            }
        }

        layoutDictionary["dock"] = dockOrder;

        // Pages
        for (int i = 0; i < pageCount; i++)
        {
            List<string> pageApps = new();

            foreach (Transform app in pages[i].AppHolder.transform)
            {
                App appComponent = app.GetComponent<App>();

                if (appComponent != null && appComponent.app != null)
                {
                    string appName = appComponent.app.appName + ".asset";
                    if (!pageApps.Contains(appName))
                    {
                        pageApps.Add(appName);
                    }
                }
            }

            string key = $"page{i + 1}";
            layoutDictionary[key] = pageApps;
        }


        //Write Data
        string json = JsonConvert.SerializeObject(layoutDictionary, Formatting.Indented);
        File.WriteAllText(layoutFilePath, json);
    }

    private void LoadAppLayout()
    {
        string json = File.ReadAllText(layoutFilePath);
        Dictionary<string, List<string>> layoutData = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);

        // Dock
        if (layoutData.TryGetValue("dock", out List<string> dockList))
        {
            List<string> loadedDockApps = new();

            foreach (string asset in dockList)
            {
                if (!loadedDockApps.Contains(asset))
                {
                    loadedDockApps.Add(asset);

                    string appFileName = asset.Replace(".asset", "");

                    AppObject appObj = FindAppObject(appFileName);

                    GameObject newApp = Instantiate(appPrefab, dock.AppHolder.transform);
                    newApp.GetComponent<App>().app = appObj;

                    dockApps.Add(newApp.GetComponent<App>());
                }
            }
        }

        //Pages
        for (int i = 0; i < pageCount; i++)
        {
            List<string> loadedPageApps = new();

            string key = $"page{i + 1}";
            if (!layoutData.TryGetValue(key, out List<string> pageList)) continue;

            GameObject newPage = Instantiate(pagePrefab, pageHolder.transform);
            newPage.transform.SetAsLastSibling();

            Home_Page pageComponent = newPage.GetComponent<Home_Page>();
            pages.Add(pageComponent);

            for (int j = 0; j < pageList.Count; j++)
            {
                string appFileName = pageList[j].Replace(".asset", "");
                if (!loadedPageApps.Contains(appFileName))
                {
                    loadedPageApps.Add(appFileName);

                    AppObject appObj = FindAppObject(appFileName);

                    GameObject newApp = Instantiate(appPrefab, newPage.GetComponent<Home_Page>().AppHolder.transform);
                    newApp.GetComponent<App>().app = appObj;
                }
            }
        }

        StartCoroutine(InvokeAppsLoadedNextFrame());
    }

    private IEnumerator InvokeAppsLoadedNextFrame()
    {
        yield return new WaitForEndOfFrame();
        AppsLoaded?.Invoke();
    }

    private AppObject FindAppObject(string appName)
    {
        AppObject[] allApps = Resources.LoadAll<AppObject>("Apps/Default Apps");

        foreach (AppObject obj in allApps)
        {
            if (obj.name == appName)
                return obj;
        }

        return null;
    }

    private void HandlePageOverflow()
    {
        bool dataSaved = true;

        for (int i = 0; i < pages.Count; i++)
        {
            Home_Page currentPage = pages[i];

            while (currentPage.AppHolder.transform.childCount > 20)
            {
                dataSaved = false;

                int lastIndex = currentPage.AppHolder.transform.childCount - 1;
                Transform lastApp = currentPage.AppHolder.transform.GetChild(lastIndex);

                if (i + 1 < pages.Count)
                {
                    lastApp.SetParent(pages[i + 1].GetComponent<Home_Page>().AppHolder.transform);
                    lastApp.SetAsFirstSibling();
                }
                else
                {
                    CreateNewPage();
                }
            }
        }


        if (!dataSaved)
        {
            SaveData();
        }
    }

    private void CreateNewPage()
    {
        GameObject newPage = Instantiate(pagePrefab, pageHolder.transform);
        pages.Add(newPage.GetComponent<Home_Page>());
        newPage.transform.SetAsLastSibling();

        pageCount++;

        AppsOverflow?.Invoke();
    }

    private void DestroyEmptyPage()
    {
        foreach (Transform child in pageHolder.transform)
        {
            Home_Page pageComponent = child.GetComponent<Home_Page>();
            if (pageComponent == null)
            {
                continue;
            }

            if (pageComponent.isEmpty)
            {
                pages.Remove(pageComponent);
                pageCount--;
                Destroy(child.gameObject);
                pageTurner.GoToPage(pageCount + 1);
            }
        }

        SaveData();
    }
}
