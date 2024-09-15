using UnityEngine;
using UnityEngine.SceneManagement;

public class App_ForceQuit : MonoBehaviour
{
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

    public void ForceQuitApp()
    {
        if (SceneManager.GetActiveScene().name == this.app.app.sceneName)
        {
            os.GoHome();
        }

        App.AppOpened -= MoveForceQuitButton;

        os.openApps.Remove(app.app);
        os.openAppHolder.Remove(app.app.sceneName);
        Destroy(this.transform.parent.gameObject);
        SceneManager.UnloadSceneAsync(this.app.app.sceneName);
    }

    private void MoveForceQuitButton()
    {
        this.gameObject.transform.SetAsLastSibling();
    }
}
