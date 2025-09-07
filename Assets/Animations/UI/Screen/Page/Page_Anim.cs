using UnityEngine;

public class Page_Anim : MonoBehaviour
{
    public void PageZoomIn()
    {
        gameObject.SetActive(true);
        LeanTween.scale(gameObject, new Vector3(21f, 21f, 21f), 0f).setDelay(0.01f).setOnComplete(() =>
        {
            LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0.5f).setEase(LeanTweenType.easeOutQuint);
        });
    }

    public void PageZoomOut(float speed = 0.5f)
    {
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0);
        LeanTween.scale(gameObject, new Vector3(21f, 21f, 21f), speed).setEase(LeanTweenType.easeInQuint).setOnComplete(Hide);
    }

    public void Hide()
    {
        transform.localScale = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
    }
}

