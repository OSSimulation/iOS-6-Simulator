using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class App_ForceQuit : MonoBehaviour
{
    public static event Action AppForceQuit;

    [SerializeField] private GameObject quit;

    TOSSP6 os;

    App app;

    private void Awake()
    {
        GameObject OS = GameObject.FindGameObjectWithTag("TOSSP6");
        os = OS.GetComponent<TOSSP6>();

        this.app = GetComponentInParent<App>();
    }

    private void Start()
    {
        App.AppOpened += MoveForceQuitButton;
    }

    private void OnEnable()
    {
        quit.SetActive(true);
    }

    private void OnDisable()
    {
        quit.SetActive(false);
    }

    public void ForceQuitApp()
    {
        App.AppOpened -= MoveForceQuitButton;

        SceneManager.UnloadSceneAsync(app.app.sceneName);

        AppForceQuit?.Invoke();

        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.45f).setEase(LeanTweenType.easeOutQuint).setOnComplete(() =>
        {
            os.openApps.Remove(app.app);
            os.openAppHolder.Remove(app.app.sceneName);
            Destroy(gameObject);
        });
    }

    private void MoveForceQuitButton()
    {
        this.gameObject.transform.SetAsLastSibling();
    }
}
