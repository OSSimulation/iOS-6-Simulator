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
        TOSSP6.AppOpened += MoveForceQuitButton;
    }

    public void ForceQuitApp()
    {
        if (SceneManager.GetActiveScene().name == app.app.sceneName)
        {
            os.GoHome();
        }

        os.openApps.Remove(app.app);
        Destroy(this.transform.parent.gameObject);
    }

    private void MoveForceQuitButton()
    {
        this.gameObject.transform.SetAsLastSibling();
    }
}
