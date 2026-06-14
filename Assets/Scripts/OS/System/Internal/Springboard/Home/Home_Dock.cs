using UnityEngine;
using UnityEngine.UI.Extensions;

public class Home_Dock : MonoBehaviour
{
    [SerializeField] private GameObject appHolder;
    [SerializeField] private NonDrawingGraphic back;
    public GameObject AppHolder => appHolder;

    private void Update()
    {
        if (appHolder.transform.childCount >= 4)
        {
            foreach (Transform app in appHolder.transform)
            {
                app.Find("Left").gameObject.SetActive(false);
                app.Find("Right").gameObject.SetActive(false);
            }

            back.enabled = false;
        }
        else if (appHolder.transform.childCount < 4)
        {
            foreach (Transform app in appHolder.transform)
            {
                app.Find("Left").gameObject.SetActive(true);
                app.Find("Right").gameObject.SetActive(true);
            }

            back.enabled = true;
        }
    }
}
