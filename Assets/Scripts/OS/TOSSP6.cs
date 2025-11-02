using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TOSSP6 : MonoBehaviour
{
    //System
    [Header("System")]
    [SerializeField] private Settings settings;
    [SerializeField] private GameObject screenHolder;
    [SerializeField] private UI_NotificationCentre notificationCentre;
    [SerializeField] private Image wallpaper;
    public Transform screenCentreTransform;
    public float brightness;
    private bool firstBoot = true;
    public bool isSystemLocked;
    public bool isDisplayOff;
    public bool isInApp;
    public bool isInNotificationCentre;
    public int maxPasscodeTries;
    public bool isSystemCharging;

    public static event Action DeviceUnlocked;
    public static event Action LockDevice;

    //Sound
    [Space(10)]
    [Header("Sound")]
    public SoundManager soundManager;
    [SerializeField] private float volume;
    public bool isSilentMode;

    //Status Bar
    [Space(10)]
    [Header("Status Bar")]
    [SerializeField] private StatusBar statusBar;

    //Home Button
    [Space(10)]
    [Header("Home Button")]
    [SerializeField] private float doublePressTime = 0.25f;
    [SerializeField] private bool spacePressed = false;
    [SerializeField] private bool spaceSinglePress = false;

    //Power Button
    [Space(10)]
    [Header("Power Button")]
    [SerializeField] private bool powerPressed = false;
    [SerializeField] private bool powerSinglePress = false;

    //Lock Screen
    [Space(10)]
    [Header("Lock Screen")]
    [SerializeField] private LockScreen lockScreen;
    [SerializeField] private GameObject defaultLock;
    [SerializeField] private GameObject mediaCentre;
    public Image brightnessObject;
    public GameObject lockScreenGO;
    public bool isLockScreen = true;
    private bool isMediaControlCentre;

    //Home Screen
    [Space(10)]
    [Header("Home Screen")]
    [SerializeField] private PageTurner_Home pageTurner;
    public GameObject homeScreenGO;
    public TMP_Text calendarDateText;
    public TMP_Text calendarDayText;
    public bool isHomeScreen = false;
    public bool isSpotlight = false;
    public bool isWiggleMode = false;
    public bool isSwitcherWiggleMode = false;

    public static event Action WiggleStop;

    //Dock
    [Space(10)]
    [Header("Dock")]
    public GameObject dock;

    //App Switcher
    [Space(10)]
    [Header("App Switcher")]
    [SerializeField] private Switcher_Controller switcher;
    [SerializeField] private GameObject appSwitcherButton;
    [SerializeField] private GameObject closeButton;
    public bool appSwitcherOpen = false;
    public bool isSwitchingApp;

    public Dictionary<string, GameObject> openAppHolder = new();
    public List<AppObject> openApps = new();

    public string currentOpenApp;
    public string previousOpenApp;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("TOSSP6");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
        LockScreen.BioAccept += StartUnlockSystem;
        Home_Layout.AppsLoaded += FirstBootLoadPageCall;
    }

    private void FirstBootLoadPageCall()
    {
        StartCoroutine(FirstBootLoadPage());
    }

    private IEnumerator FirstBootLoadPage()
    {
        yield return new WaitForEndOfFrame();

        if (firstBoot)
        {
            HideHomeScreen(0);
            firstBoot = false;

            yield return new WaitForSeconds(0.15f);

            LockSystem();
        }
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        LockScreen.BioAccept += StartUnlockSystem;

        Debug.Log(SystemInfo.operatingSystem);

        CloseLockMediaCentre();

        closeButton.SetActive(false);
    }

    void Update()
    {
        SetSilentMode();
        GetSFXVolume();

        HomeButton();
        PowerButton();

        if (pageTurner.currentPage == 1)
        {
            isSpotlight = true;
        }
        else
        {
            isSpotlight = false;
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
                    dateTextComponent.text = System.DateTime.Now.ToString("%d");
                }
            }
        }

        if (openAppHolder.ContainsKey(currentOpenApp))
        {
            GameObject currentAppHolder = openAppHolder[currentOpenApp];

            if (currentAppHolder != null && !isSwitchingApp)
            {
                currentAppHolder.transform.position = screenCentreTransform.transform.position;
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0)
        {
            GameObject[] sceneContentArray = GameObject.FindGameObjectsWithTag("Holder");

            if (sceneContentArray.Length > 0)
            {
                foreach (GameObject sceneContent in sceneContentArray)
                {
                    if (!openAppHolder.ContainsKey(scene.name))
                    {
                        openAppHolder.Add(scene.name, sceneContent);
                    }
                }
            }
        }

        if (openAppHolder.ContainsKey(scene.name))
        {
            GameObject currentAppHolder = openAppHolder[scene.name];

            if (currentAppHolder != null)
            {
                currentAppHolder.SetActive(true);
            }
        }
    }

    private void StartUnlockSystem()
    {
        float delay = 0;

        if (lockScreen.UsePasscode)
            delay = 0f;
        else if (!lockScreen.UsePasscode)
            delay = 0.25f;

        StartCoroutine(UnlockSystem(delay));
    }

    private IEnumerator UnlockSystem(float delay)
    {
        yield return new WaitForSeconds(delay);

        GetComponent<Canvas>().sortingLayerName = "Main";
        GetComponent<Canvas>().sortingOrder = 0;

        if (!isInApp)
        {
            isSystemLocked = false;
            isLockScreen = false;
            isHomeScreen = true;

            if (isSpotlight)
            {
                pageTurner.GoToPage(2);
            }

            dock.GetComponent<Dock_Anim>().DockShow();
        }
        else if (isInApp)
        {
            isSystemLocked = false;
            isLockScreen = false;
            isHomeScreen = false;

            ShowApp(currentOpenApp);
        }

        soundManager.PlaySound(SoundEvents.SYSTEM_UNLOCK, volume, SoundSources.SYSTEM_SFX);
        DeviceUnlocked?.Invoke();
    }

    public void LockSystem()
    {
        if (!isSystemLocked && !isDisplayOff && isHomeScreen)
        {
            isSystemLocked = true;
            isLockScreen = true;
            isHomeScreen = false;
            isDisplayOff = true;

            soundManager.PlaySound(SoundEvents.SYSTEM_LOCK, volume, SoundSources.SYSTEM_SFX);

            if (isHomeScreen)
            {
                dock.GetComponent<Dock_Anim>().DockHide();
            }

            if (isMediaControlCentre)
            {
                CloseLockMediaCentre();
            }

            LockDisplay();
        }
        else if (isSystemLocked && isDisplayOff)
        {
            isLockScreen = true;
            isHomeScreen = false;
            isDisplayOff = false;
            lockScreenGO.SetActive(true);

            UnlockDisplay();
        }
        else if (isSystemLocked && !isDisplayOff)
        {
            isSystemLocked = true;
            isLockScreen = true;
            isHomeScreen = false;
            isDisplayOff = true;

            LockDisplay();
        }
        else if (isInApp)
        {
            isSystemLocked = true;
            isLockScreen = true;
            isHomeScreen = false;
            isDisplayOff = true;

            soundManager.PlaySound(SoundEvents.SYSTEM_LOCK, volume, SoundSources.SYSTEM_SFX);

            LockDisplay();
        }

        if (isInNotificationCentre)
        {
            HideNotificationCentre();
        }

        if (appSwitcherOpen)
        {
            StartCoroutine(CloseAppSwitcher());
        }

        isWiggleMode = false;
        isSwitcherWiggleMode = false;
    }

    private void LockDisplay()
    {
        GetComponent<Canvas>().sortingLayerName = "Main";
        GetComponent<Canvas>().sortingOrder = 9999;
        brightnessObject.gameObject.SetActive(true);
        brightnessObject.GetComponent<Image>().raycastTarget = true;
        LockDevice?.Invoke();
        screenHolder.GetComponent<Screen_Anim>().ScreenLower();
        statusBar.LockBar();
        HideHomeScreen(0);
        HideApp(currentOpenApp);
    }

    private void UnlockDisplay()
    {
        brightnessObject.gameObject.SetActive(false);
        brightnessObject.GetComponent<Image>().raycastTarget = false;

        GetComponent<Canvas>().sortingLayerName = "Main";
        GetComponent<Canvas>().sortingOrder = 499;
    }

    public void HomeButton()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!spacePressed)
            {
                spacePressed = true;
                StartCoroutine(CheckHomeDoublePress());
            }
            else
            {
                if (!isSystemLocked && !isLockScreen)
                {
                    spacePressed = false;
                    spaceSinglePress = false;
                    StopCoroutine("CheckHomeDoublePress");
                    OpenAppSwitcher();
                    return;
                }

                if (isLockScreen && isSystemLocked && !isMediaControlCentre)
                {
                    OpenLockMediaCentre();
                }
                else if (isMediaControlCentre)
                {
                    CloseLockMediaCentre();
                }
            }
        }
    }

    IEnumerator CheckHomeDoublePress()
    {
        spaceSinglePress = true;
        yield return new WaitForSeconds(doublePressTime);
        if (spaceSinglePress)
        {
            spacePressed = false;
            spaceSinglePress = false;
            GoHome();
        }
    }

    public void PowerButton()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (!powerPressed)
            {
                powerPressed = true;
                StartCoroutine(CheckPowerDoublePress());
            }
            else
            {
                powerPressed = false;
                powerSinglePress = false;
                StopCoroutine("CheckPowerDoublePress");
                Debug.Log("Double Press");
            }
        }
    }

    IEnumerator CheckPowerDoublePress()
    {
        powerSinglePress = true;
        yield return new WaitForSeconds(doublePressTime);
        if (powerSinglePress)
        {
            powerPressed = false;
            powerSinglePress = false;
            LockSystem();
        }
    }

    public void GoHome()
    {
        if (!isSystemLocked)
        {
            if (appSwitcherOpen)
            {
                StartCoroutine(CloseAppSwitcher());
            }
            else if (isInNotificationCentre)
            {
                HideNotificationCentre();
            }
            else if (!isHomeScreen)
            {
                HideApp(currentOpenApp);

                ShowHomeScreen();
                isHomeScreen = true;
                isInApp = false;
            }
            else if (isWiggleMode)
            {
                isWiggleMode = false;
                WiggleStop?.Invoke();
            }
            else if (isHomeScreen && pageTurner.currentPage != 2 || isSpotlight)
            {
                pageTurner.GoToPage(2);
            }
            else if (isHomeScreen && pageTurner.currentPage == 2 && !isSpotlight)
            {
                pageTurner.GoToPage(1);
            }
        }
    }

    public void ShowHomeScreen()
    {
        foreach (Transform transform in pageTurner.pageObjects)
        {
            transform.gameObject.SetActive(true);
        }

        pageTurner.pages[pageTurner.currentPage - 1].gameObject.GetComponent<Page_Anim>().Cancel();
        pageTurner.pages[pageTurner.currentPage - 1].gameObject.GetComponent<Page_Anim>().PageZoomIn();
        dock.GetComponent<Dock_Anim>().DockShow();
        dock.SetActive(true);
    }

    public void HideHomeScreen(float speed = 0.5f)
    {
        if (isSpotlight)
            pageTurner.GoToPage(2);

        pageTurner.pages[pageTurner.currentPage - 1].gameObject.GetComponent<Page_Anim>().Cancel();
        pageTurner.pages[pageTurner.currentPage - 1].gameObject.GetComponent<Page_Anim>().PageZoomOut(speed);

        if (appSwitcherOpen)
        {
            StartCoroutine(CloseAppSwitcher());
        }

        dock.GetComponent<Dock_Anim>().DockHide();
    }

    public void HideNotificationCentre()
    {
        notificationCentre.GoToPage(1);
        isInNotificationCentre = false;
        notificationCentre.InvokeHideNotificationCentre();
    }

    public void OpenAppSwitcher()
    {
        screenHolder.GetComponent<Screen_Anim>().ScreenLift();
        closeButton.SetActive(true);
        appSwitcherOpen = true;
    }

    public void CloseAppSwitcherTrigger()
    {
        StartCoroutine(CloseAppSwitcher());
    }

    public IEnumerator CloseAppSwitcher()
    {
        isSwitcherWiggleMode = false;
        screenHolder.GetComponent<Screen_Anim>().ScreenLower();
        closeButton.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        switcher.GoToPage(3, 0);
        appSwitcherOpen = false;
    }

    private void OpenLockMediaCentre()
    {
        isMediaControlCentre = true;
        statusBar.NormalBar();

        mediaCentre.SetActive(true);
        defaultLock.SetActive(false);
    }

    private void CloseLockMediaCentre()
    {
        isMediaControlCentre = false;
        statusBar.LockBar();

        mediaCentre.SetActive(false);
        defaultLock.SetActive(true);
    }

    public void HideApp(string sceneName)
    {
        LeanTween.value(wallpaper.gameObject, 0f, 1f, 0.5f).setOnUpdate((float t) =>
        {
            wallpaper.color = Color.Lerp(Color.black, Color.white, t);
        });

        if (openAppHolder.ContainsKey(sceneName))
        {
            GameObject currentAppHolder = openAppHolder[sceneName];

            if (currentAppHolder != null)
            {
                currentAppHolder.GetComponent<App_Anim>().AppZoomOut();

            }
        }
    }

    public void ShowApp(string sceneName)
    {
        LeanTween.value(wallpaper.gameObject, 0f, 1f, 0.5f).setOnUpdate((float t) =>
        {
            wallpaper.color = Color.Lerp(Color.white, Color.black, t);
        });

        if (openAppHolder.ContainsKey(sceneName))
        {
            GameObject currentAppHolder = openAppHolder[sceneName];

            if (currentAppHolder != null)
            {
                currentAppHolder.GetComponent<App_Anim>().AppZoomIn();
            }
        }
    }

    private void SetSilentMode()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            isSilentMode = !isSilentMode;
            soundManager.PlaySound(SoundEvents.SYSTEM_RINGER_CHANGED, volume, SoundSources.SYSTEM_SFX);
            settings.SetRinger(isSilentMode);
        }
    }

    public float GetSFXVolume()
    {
        return volume = PlayerPrefs.GetFloat("System_Volume_SFX");
    }
}
