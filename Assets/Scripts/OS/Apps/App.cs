using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class App : MonoBehaviour
{
    [SerializeField] private AppObject app;

    TOSSP6 os;

    Image image;

    Button button;

    TMP_Text appName;

    private void Awake()
    {
        GameObject OS = GameObject.FindGameObjectWithTag("TOSSP6");
        os = OS.GetComponent<TOSSP6>();
    }

    private void Start()
    {
        button = this.gameObject.GetComponent<Button>();

        image = this.gameObject.GetComponent<Image>();
        image.sprite = app.appIcon;

        appName = this.gameObject.GetComponentInChildren<TMP_Text>();
        appName.text = app.appName;
    }

    public void OpenApp()
    {
        SceneManager.LoadSceneAsync(app.sceneName);
        os.isHomeScreen = false;
    }
}
