using UnityEngine;

public class Home_Page : MonoBehaviour
{
    [SerializeField] private GameObject appHolder;
    public GameObject AppHolder => appHolder;

    public int appCount => appHolder.transform.childCount;
    public bool isEmpty => appCount == 0;

    private void Update()
    {
        foreach (Transform app in appHolder.transform)
        {
            app.Find("Left").gameObject.SetActive(true);
            app.Find("Right").gameObject.SetActive(true);
        }
    }
}
