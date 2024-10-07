using UnityEngine;

public class Page_Anim : MonoBehaviour
{
    public void PageZoomIn()
    {
        LeanTween.scale(gameObject, new Vector3(21f, 21f, 21f), 0f);
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0.35f);
    }

    public void PageZoomOut()
    {
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0);
        LeanTween.scale(gameObject, new Vector3(21f, 21f, 21f), 0.35f).setOnComplete(Hide);
    }

    public void Hide()
    {
        transform.localScale = new Vector3(0, 0, 0);
    }
}
