using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class TOSSP6 : MonoBehaviour
{
    //System
    [Header("System")]
    [SerializeField] private GameObject screenHolder;
    public bool isSystemLocked;
    public int maxPasscodeTries;

    //Status Bar
    [Space(10)]
    [Header("Status Bar")]
    [SerializeField] private StatusBar statusBar;

    //Home Button
    [Space(10)]
    [Header("Home Button")]
    [SerializeField] private float doublePressTime = 0.25f;
    [SerializeField] private bool spacePressed = false;
    [SerializeField] private bool singlePress = false;

    //Lock Screen
    [Space(10)]
    [Header("Lock Screen")]
    [SerializeField] private GameObject lockScreenGO;
    public bool isLockScreen;

    //Home Screen
    [Space(10)]
    [Header("Home Screen")]
    [SerializeField] private Home homeScreen;
    [SerializeField] private GameObject homeScreenGO;
    public bool isHomeScreen;

    //App Switcher
    [Space(10)]
    [Header("App Switcher")]
    [SerializeField] private GameObject appSwitcherButton;
    [SerializeField] private GameObject appSwitcherHolder;
    public bool appSwitcherOpen = false;
    public List<AppObject> openApps = new List<AppObject>();
    private List<Transform> appSwitcherPages = new List<Transform>();

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("TOSSP6");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        foreach (Transform child in appSwitcherHolder.transform)
        {
            appSwitcherPages.Add(child.transform);
        }
    }

    void Update()
    {
        HomeButton();

        foreach (AppObject app in openApps)
        {
            InstantiateAppButton(app);
        }
    }

    public void HomeButton()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!spacePressed)
            {
                spacePressed = true;
                StartCoroutine(CheckDoublePress());
            }
            else
            {
                spacePressed = false;
                singlePress = false;
                StopCoroutine("CheckDoublePress");
                Debug.Log("Space bar double pressed");
                OpenAppSwitcher();
            }
        }
    }

    public void OpenAppSwitcher()
    {
        screenHolder.GetComponent<Animator>().Play("Screen_Lift");
        appSwitcherOpen = true;
    }

    public void CloseAppSwitcher()
    {
        screenHolder.GetComponent<Animator>().Play("Screen_Lower");
        appSwitcherOpen = false;
    }

    IEnumerator CheckDoublePress()
    {
        singlePress = true;
        yield return new WaitForSeconds(doublePressTime);
        if (singlePress)
        {
            spacePressed = false;
            singlePress = false;
            Debug.Log("Space bar single pressed");
            GoHome();
        }
    }

    public void GoHome()
    {
        if (appSwitcherOpen)
        {
            CloseAppSwitcher();
        }
        else if (!isHomeScreen)
        {
            SceneManager.LoadScene(0);
            ShowHomeScreen();
            isHomeScreen = true;
        }
        else if (isHomeScreen && homeScreen.currentPage != 1)
        {
            homeScreen.GoToPage(1);
        }
    }

    public void ShowHomeScreen()
    {
        homeScreenGO.SetActive(true);
    }

    public void HideHomeScreen()
    {
        homeScreenGO.SetActive(false);
    }

    public void InstantiateAppButton(AppObject app)
    {
        if (!DoesButtonExistForApp(app))
        {
            GameObject newButton = Instantiate(appSwitcherButton, appSwitcherHolder.transform);

            App newAppComponent = newButton.GetComponent<App>();

            newAppComponent.app = app;
        }
    }

    private bool DoesButtonExistForApp(AppObject app)
    {
        foreach (Transform child in appSwitcherHolder.transform)
        {
            App appComponent = child.GetComponent<App>();
            if (appComponent != null && appComponent.app == app)
            {
                return true;
            }
        }
        return false;
    }
}
