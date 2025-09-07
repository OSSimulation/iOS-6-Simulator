using UnityEngine;

public class Switcher_Page : MonoBehaviour
{
    [SerializeField] private PageType Type;
    public PageType type => Type;

    public int appCount => transform.childCount;
    public bool isEmpty => appCount == 0;
}

public enum PageType
{
    VOLUME,
    SETTINGS,
    APP_HOLDER
}
