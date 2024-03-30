using UnityEngine;

public class LockScreen : MonoBehaviour
{
    [SerializeField] private TOSSP6 main;

    Animator anim;

    private void Start()
    {
        TOSSP6.DeviceUnlocked += PlayOpenAnimation;

        anim = GetComponent<Animator>();
    }

    private void PlayOpenAnimation()
    {
        anim.Play("Lock_Screen_UI_Push");
    }

    void HideLockScreenS1()
    {
        main.lockScreenGO.SetActive(false);
    }

    void HideLockScreenS2()
    {
        main.ShowHomeScreen();
    }
}
