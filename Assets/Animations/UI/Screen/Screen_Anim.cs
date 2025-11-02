using UnityEngine;

public class Screen_Anim : MonoBehaviour
{
    TOSSP6 main;

    private void Awake()
    {
        GameObject OS = GameObject.FindGameObjectWithTag("TOSSP6");
        main = OS.GetComponent<TOSSP6>();
    }

    public void ScreenLift()
    {
        LeanTween.moveLocalY(gameObject, 225f, 0.5f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.alphaCanvas(main.homeScreenGO.GetComponent<CanvasGroup>(), 0.5f, 0.5f).setEase(LeanTweenType.easeOutQuint);
    }

    public void ScreenLower()
    {
        LeanTween.moveLocalY(gameObject, 0, 0.5f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.alphaCanvas(main.homeScreenGO.GetComponent<CanvasGroup>(), 1f, 0.5f).setEase(LeanTweenType.easeOutQuint);
    }
}
