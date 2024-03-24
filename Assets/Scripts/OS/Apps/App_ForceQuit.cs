using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void ForceQuitApp()
    {
        os.openApps.Remove(app.app);
        Destroy(this.transform.parent.gameObject);
    }
}
