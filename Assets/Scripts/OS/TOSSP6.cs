using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TOSSP6 : MonoBehaviour
{
    //System
    [Header("System")]
    [SerializeField] private GameObject screenHolder;
    public static event Action DeviceUnlocked;
    public static event Action AppOpened;
    public bool isSystemLocked = true;
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
    public GameObject lockScreenGO;
    public bool isLockScreen = true;

    //Home Screen
    [Space(10)]
    [Header("Home Screen")]
    public TMP_Text calendarDateText;
    public TMP_Text calendarDayText;
    [SerializeField] private PageTurner_Home pageTurner;
    public GameObject homeScreenGO;
    public bool isHomeScreen = false;

    //App Switcher
    [Space(10)]
    [Header("App Switcher")]
    [SerializeField] private GameObject appSwitcherButton;
    public GameObject appSwitcherHolder;
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
        SlideToUnlock.SliderMovedToEnd += UnlockSystem;

        foreach (Transform child in appSwitcherHolder.transform)
        {
            appSwitcherPages.Add(child.transform);
        }

        Debug.Log(SystemInfo.operatingSystem);
    }

    void Update()
    {
        HomeButton();

        foreach (AppObject app in openApps)
        {
            InstantiateAppButton(app);

            AppOpened?.Invoke();
        }

        GameObject[] dayObjects = GameObject.FindGameObjectsWithTag("DAY");
        foreach (GameObject dayObject in dayObjects)
        {
            if (dayObject != null)
            {
                TMP_Text dayTextComponent = dayObject.GetComponent<TMP_Text>();
                if (dayTextComponent != null)
                {
                    dayTextComponent.text = System.DateTime.Now.ToString("dddd");
                }
                else
                {
                    Debug.LogError("TMP_Text component not found on GameObject with tag 'DAY'");
                }
            }
            else
            {
                Debug.LogError("GameObject with tag 'DAY' not found in the scene");
            }
        }

        GameObject[] dateObjects = GameObject.FindGameObjectsWithTag("DATE");
        foreach (GameObject dateObject in dateObjects)
        {
            if (dateObject != null)
            {
                TMP_Text dateTextComponent = dateObject.GetComponent<TMP_Text>();
                if (dateTextComponent != null)
                {
                    dateTextComponent.text = System.DateTime.Now.ToString("dd");
                }
                else
                {
                    Debug.LogError("TMP_Text component not found on GameObject with tag 'DATE'");
                }
            }
            else
            {
                Debug.LogError("GameObject with tag 'DATE' not found in the scene");
            }
        }
    }

    private void UnlockSystem()
    {
        isSystemLocked = false;
        isLockScreen = false;
        isHomeScreen = true;

        screenHolder.GetComponent<Animator>().Play("Screen_Home_Zoom");

        DeviceUnlocked?.Invoke();
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
                if (!isSystemLocked && !isLockScreen)
                {
                    spacePressed = false;
                    singlePress = false;
                    StopCoroutine("CheckDoublePress");
                    OpenAppSwitcher();
                }
            }
        }
    }

    IEnumerator CheckDoublePress()
    {
        singlePress = true;
        yield return new WaitForSeconds(doublePressTime);
        if (singlePress)
        {
            spacePressed = false;
            singlePress = false;
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
        else if (isHomeScreen && pageTurner.currentPage != 1)
        {
            pageTurner.GoToPage(1);
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

    public void InstantiateAppButton(AppObject app)
    {
        if (!DoesButtonExistForApp(app))
        {
            GameObject newButton = Instantiate(appSwitcherButton, appSwitcherHolder.transform);

            App newAppComponent = newButton.GetComponent<App>();

            newButton.name = app.name;

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
