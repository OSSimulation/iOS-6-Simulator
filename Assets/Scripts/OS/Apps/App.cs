using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class App : MonoBehaviour
{
    public static event Action AppOpened;

    private TOSSP6 main;
    public AppObject app;

    Image image;
    TMP_Text appName;

    private void Awake()
    {
        GameObject OS = GameObject.FindGameObjectWithTag("TOSSP6");
        main = OS.GetComponent<TOSSP6>();
    }

    private void Start()
    {
        image = gameObject.GetComponent<Image>();
        image.sprite = app.appIcon;

        appName = gameObject.GetComponentInChildren<TMP_Text>();
        appName.text = app.appName;

        if (appName.text == "Calendar")
        {
            Instantiate(main.calendarDateText, transform);
            Instantiate(main.calendarDayText, transform);
        }
    }
    
    public void OpenApp()
    {
        if (main.isWiggleMode || main.isSwitcherWiggleMode) return; 
                
        if (!main.openApps.Contains(app))
        {
            main.openApps.Insert(0, app);
        }
        else if (main.openApps.Contains(app))
        {
            main.openApps.Remove(app);
            main.openApps.Insert(0, app);
            main.appSwitcherHolder.transform.Find(app.name).SetAsFirstSibling();
        }

        if (main.isHomeScreen)
        {
            if (!SceneManager.GetSceneByName(app.sceneName).isLoaded)
            {
                SceneManager.LoadScene(app.sceneName, LoadSceneMode.Additive);
            }

            if (main.openAppHolder.ContainsKey(app.sceneName))
            {
                GameObject currentAppHolder = main.openAppHolder[app.sceneName];

                if (currentAppHolder != null)
                {
                    currentAppHolder.SetActive(true);
                }
            }

            foreach (var entry in main.openAppHolder)
            {
                string sceneName = entry.Key;
                GameObject appHolder = entry.Value;

                if (appHolder != null && sceneName != app.sceneName)
                {
                    appHolder.SetActive(false);
                }
            }

            if (main.currentOpenApp != app.sceneName)
            {
                main.previousOpenApp = main.currentOpenApp;
            }

            main.currentOpenApp = app.sceneName;

            main.HideHomeScreen();
            main.ShowApp(app.sceneName);

            main.isInApp = true;
            main.isHomeScreen = false;

            AppOpened?.Invoke();
            return;
        }
        else if (main.isInApp && main.currentOpenApp != app.sceneName)
        {
            StartCoroutine(SwitchApp());
            return;
        }
    }

    private IEnumerator SwitchApp()
    {
        StartCoroutine(main.CloseAppSwitcher());
        yield return new WaitForSeconds(0.5f);

        main.isSwitchingApp = true;

        if (main.currentOpenApp != app.sceneName)
        {
            main.previousOpenApp = main.currentOpenApp;
        }

        main.currentOpenApp = app.sceneName;

        main.openAppHolder[main.currentOpenApp].GetComponent<Object_State>().Activate();
        main.openAppHolder[main.currentOpenApp].GetComponent<App_Anim>().Cancel();
        main.openAppHolder[main.currentOpenApp].GetComponent<App_Anim>().AppSwitchIn();
        main.openAppHolder[main.previousOpenApp].GetComponent<App_Anim>().AppSwitchOut();

        yield return new WaitForSeconds(0.25f);

        main.openAppHolder[main.currentOpenApp].GetComponentInParent<Canvas>().sortingOrder = 200;
        main.openAppHolder[main.previousOpenApp].GetComponentInParent<Canvas>().sortingOrder = 0;

        yield return new WaitForSeconds(0.75f);

        main.openAppHolder[main.previousOpenApp].GetComponent<Object_State>().Deactivate();
        main.isSwitchingApp = false;
    }
}
