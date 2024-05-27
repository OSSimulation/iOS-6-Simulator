using System;
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
    Button button;
    TMP_Text appName;

    private void Awake()
    {
        GameObject OS = GameObject.FindGameObjectWithTag("TOSSP6");
        main = OS.GetComponent<TOSSP6>();
    }

    private void Start()
    {
        button = this.gameObject.GetComponent<Button>();

        image = this.gameObject.GetComponent<Image>();
        image.sprite = app.appIcon;

        appName = this.gameObject.GetComponentInChildren<TMP_Text>();
        appName.text = app.appName;

        if (appName.text == "Calendar")
        {
            Instantiate(main.calendarDateText, this.transform);
            Instantiate(main.calendarDayText, this.transform);
        }
    }

    public void OpenApp()
    {
        if (!SceneManager.GetSceneByName(this.app.sceneName).isLoaded)
        {
            SceneManager.LoadScene(app.sceneName, LoadSceneMode.Additive);
        }

        if (main.openAppHolder.ContainsKey(this.app.sceneName))
        {
            GameObject currentAppHolder = main.openAppHolder[this.app.sceneName];

            if (currentAppHolder != null)
            {
                currentAppHolder.SetActive(true);
            }
        }

        foreach (var entry in main.openAppHolder)
        {
            string sceneName = entry.Key;
            GameObject appHolder = entry.Value;

            if (appHolder != null && sceneName != this.app.sceneName)
            {
                appHolder.SetActive(false);
            }
        }

        if (!main.openApps.Contains(app))
        {
            main.openApps.Add(app);
        }
        else if (main.openApps.Contains(app))
        {
            main.openApps.Remove(app);
            main.openApps.Insert(0, app);
            main.appSwitcherHolder.transform.Find(app.name).SetAsFirstSibling();
        }

        main.HideHomeScreen();
        AppOpened?.Invoke();

        if (main.appSwitcherOpen)
        {
            StartCoroutine(main.CloseAppSwitcher());
        }

        main.currentOpenApp = this.app.sceneName;

        main.isInApp = true;
        main.isHomeScreen = false;
    }
}
