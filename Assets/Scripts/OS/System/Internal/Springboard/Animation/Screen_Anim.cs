using UnityEngine;

public class Screen_Anim : MonoBehaviour
{
    [SerializeField] private CanvasGroup HSCanvasGroup;

    public void ScreenLift()
    {
        LeanTween.moveLocalY(gameObject, 225f, 0.5f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.alphaCanvas(HSCanvasGroup, 0.5f, 0.5f).setEase(LeanTweenType.easeOutQuint);
    }

    public void ScreenLower()
    {
        LeanTween.moveLocalY(gameObject, 0, 0.5f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.alphaCanvas(HSCanvasGroup, 1f, 0.5f).setEase(LeanTweenType.easeOutQuint);
    }
}
