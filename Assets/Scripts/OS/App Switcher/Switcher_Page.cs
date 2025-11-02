using Tailspin96.SmoothUILayouts.Core;
using UnityEngine;

public class Switcher_Page : MonoBehaviour
{
    [SerializeField] private PageType Type;
    public Transform holder => GetComponentInChildren<ItemHolderManager>().transform;
    public PageType type => Type;

    public int appCount => holder.childCount;
    public bool isEmpty => appCount == 0;
}

public enum PageType
{
    VOLUME,
    SETTINGS,
    APP_HOLDER
}
