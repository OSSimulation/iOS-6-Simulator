using UnityEngine;
using UnityEngine.UI;

public class LockScreen : MonoBehaviour
{
    [SerializeField] private TOSSP6 main;

    [SerializeField] private GameObject batteryUI;
    [SerializeField] private Image batteryBG;
    [SerializeField] private Sprite[] batteryBGImages;

    [SerializeField] private GameObject topGO, bottomGO, batteryUIGO, mediaControlGO;

    private void Start()
    {
        TOSSP6.DeviceUnlocked += PlayOpenAnimation;
        TOSSP6.LockDevice += ResetLockScreen;
    }

    private void Update()
    {
        BatteryBG();
    }

    void BatteryBG()
    {
        if (main.isSystemCharging)
        {
            batteryUI.SetActive(true);

            for (int i = 0; i < 16; i++)
            {
                if (SystemInfo.batteryLevel * 100 >= (i + 1) * 6.25)
                {
                    batteryBG.sprite = batteryBGImages[i];
                }
            }
        }
        else
        {
            batteryUI.SetActive(false);
        }
    }

    private void PlayOpenAnimation()
    {
        //anim.Play("Lock_Screen_UI_Push");

        LeanTween.moveLocalY(topGO, 230, 0.5f).setEase(LeanTweenType.easeOutQuart);
        LeanTween.moveLocalY(mediaControlGO, 325, 0.5f).setEase(LeanTweenType.easeOutQuart);
        HideLockScreenS2();
        LeanTween.moveLocalY(bottomGO, -190, 0.5f).setEase(LeanTweenType.easeOutQuart).setOnComplete(() =>
        {
            LeanTween.moveLocalY(bottomGO, -415, 0);
        });

        LeanTween.alphaCanvas(batteryUIGO.GetComponent<CanvasGroup>(), 0, 0.15f);
    }

    public void ResetLockScreen()
    {
        //anim.Play("Lock_Screen_Reset");

        LeanTween.moveLocalY(topGO, 0, 0);
        LeanTween.moveLocalY(mediaControlGO, 0, 0);
        LeanTween.moveLocalY(bottomGO, 0, 0);

        LeanTween.alphaCanvas(batteryUIGO.GetComponent<CanvasGroup>(), 1, 0);
    }

    void HideLockScreenS1()
    {
        //main.lockScreenGO.SetActive(false);
    }

    void HideLockScreenS2()
    {
        if (!main.isInApp)
        {
            main.ShowHomeScreen();
        }
    }
}
