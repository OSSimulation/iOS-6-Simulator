using UnityEngine;

public class Dock_Anim : MonoBehaviour
{
    public void DockShow()
    {
        LeanTween.moveLocalY(gameObject, -518f, 0.25f).setDelay(0.2f).setEase(LeanTweenType.easeOutQuint);
    }

    public void DockHide()
    {
        LeanTween.moveLocalY(gameObject, -718f, 0.25f).setEase(LeanTweenType.easeInQuint);
        LeanTween.moveLocalY(gameObject, -983f, 0.1f).setDelay(0.35f);
    }
}
