using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class App : MonoBehaviour
{
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
        SceneManager.LoadSceneAsync(app.sceneName);
        main.isHomeScreen = false;

        if (main.appSwitcherOpen)
        {
            main.CloseAppSwitcher();
        }

        main.HideHomeScreen();

        if (!main.openApps.Contains(app))
        {
            main.openApps.Add(app);
        }
        else if (main.openApps.Contains(app))
        {
            main.openApps.Remove(app);
            main.openApps.Insert(0, app);

            main.appSwitcherHolder.gameObject.transform.Find(app.name).SetAsFirstSibling();
        }
    }
}
