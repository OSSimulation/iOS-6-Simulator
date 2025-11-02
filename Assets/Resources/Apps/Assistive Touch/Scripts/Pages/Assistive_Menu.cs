using UnityEngine;

public class Assistive_Menu : MonoBehaviour
{
    public string menuName;
    public bool open;

    public void Open()
    {
        gameObject.SetActive(true);
        open = true;

        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.2f);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        open = false;
    }
}
