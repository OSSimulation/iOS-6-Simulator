using UnityEngine;
using UnityEngine.UI;

public class LockScreen : MonoBehaviour
{
    [SerializeField] private TOSSP6 main;

    [SerializeField] private GameObject batteryUI;
    [SerializeField] private Image batteryBG;
    [SerializeField] private Sprite[] batteryBGImages;

    Animator anim;

    private void Start()
    {
        TOSSP6.DeviceUnlocked += PlayOpenAnimation;
        TOSSP6.LockDevice += ResetLockScreen;

        anim = GetComponent<Animator>();
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

            for (int i = 0; i < 17; i++)
            {
                if (SystemInfo.batteryLevel * 100 <= (i + 1) * 6.25)
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
        anim.Play("Lock_Screen_UI_Push");
    }

    public void ResetLockScreen()
    {
        anim.Play("Lock_Screen_Reset");
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
