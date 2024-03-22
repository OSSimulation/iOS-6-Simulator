using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TOSSP6 : MonoBehaviour
{
    //System
    [Header("System")]
    public bool isSystemLocked;
    public int maxPasscodeTries;

    //Status Bar
    [Space(10)]
    [Header("Status Bar")]
    [SerializeField] private StatusBar statusBar;

    //Home Button
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
    public GameObject appSwitcher;
    public bool appSwitcherOpen;
    public string[] openApps;

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

    }

    void Update()
    {
        HomeButton();
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
                spacePressed = false; // Reset for next press
                singlePress = false; // Reset for next press
                StopCoroutine("CheckDoublePress");
                Debug.Log("Space bar double pressed");
                //OpenAppSwitcher();
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
            Debug.Log("Space bar single pressed");
            GoHome();
        }
    }

    public void GoHome()
    {
        if (appSwitcherOpen)
        {
            //CloseAppSwitcher();
        }
        else if (!isHomeScreen)
        {
            SceneManager.LoadScene(0);
            isHomeScreen = true;
        }
        else if (isHomeScreen && homeScreen.currentPage != 1)
        {
            homeScreen.GoToPage(1);
        }
    }

}
