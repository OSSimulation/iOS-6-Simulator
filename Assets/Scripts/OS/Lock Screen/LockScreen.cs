using OS6.Authentication;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LockScreen : MonoBehaviour
{
	public static event Action BioAccept;
	public static event Action BioReject;

	[SerializeField] private bool usePasscode;

	public bool UsePasscode => usePasscode;

	[SerializeField] private TOSSP6 main;

	[SerializeField] private Image batteryBG;
	[SerializeField] private Sprite[] batteryBGImages;

	[SerializeField] private GameObject topGO, bottomGO, batteryUI, mediaControlGO, interactablesGO;
	[SerializeField] private Toggle[] passcodeToggles;
	[SerializeField] private CanvasGroup batteryUICanvas;

	int lastBatterySpriteIndex = -2;

	private void Start()
	{
		SlideToUnlock.SliderMovedToEnd += PlayPasscodeShowAnimation;

		TOSSP6.DeviceUnlocked += PlayOpenAnimation;
		TOSSP6.LockDevice += ResetLockScreen;
	}

	private void Update()
	{
		BatteryBG();
	}

	private void BatteryBG()
	{
		if (!main.isSystemCharging)
		{
			batteryUI.SetActive(false);
			return;
		}

		if (!batteryUI.activeSelf)
		{
			batteryUI.SetActive(true);
		}

		float step = 6.25f;
		float currentBatteryLevel = SystemInfo.batteryLevel * 100;
		int batterySpriteIndex = Mathf.Clamp((int)(currentBatteryLevel / step), 0, batteryBGImages.Length - 1);

		if (batterySpriteIndex != lastBatterySpriteIndex)
		{
			batteryBG.sprite = batteryBGImages[batterySpriteIndex];
			lastBatterySpriteIndex = batterySpriteIndex;
		}
	}

	private void PlayPasscodeShowAnimation()
	{
		if (usePasscode)
		{
			LeanTween.moveLocalX(interactablesGO, 640f, 0.3f);
			LeanTween.moveLocalY(bottomGO, 432, 0.3f).setOnComplete(() =>
			{
				LeanTween.moveLocalY(bottomGO, 432f, 0).setDelay(0.25f).setOnComplete(async () =>
				{
					if (await ValidateBiometrics.AuthenticateBiometrics())
					{
						foreach (Toggle toggle in passcodeToggles)
						{
							toggle.isOn = true;
						}

						StartCoroutine(AcceptBio());
					}
					else
					{
						PlayPasscodeHideAnimation();
						BioReject?.Invoke();
					}
				});
			});
		}
		else
		{
			BioAccept?.Invoke();
		}
	}

	private void PlayPasscodeHideAnimation()
	{
		LeanTween.moveLocalY(bottomGO, 0, 0.3f).setDelay(0.25f);
		LeanTween.moveLocalX(interactablesGO, 0, 0.3f).setDelay(0.25f);
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

		LeanTween.alphaCanvas(batteryUICanvas, 0, 0.25f);
		batteryUICanvas.blocksRaycasts = false;
	}

	public void ResetLockScreen()
	{
		//anim.Play("Lock_Screen_Reset");
		LeanTween.cancel(batteryUI);
		LeanTween.cancel(bottomGO);
		LeanTween.cancel(interactablesGO);
		LeanTween.cancel(mediaControlGO);
		LeanTween.cancel(topGO);

		LeanTween.moveLocalY(topGO, 0, 0);
		LeanTween.moveLocalY(mediaControlGO, 0, 0);
		LeanTween.moveLocalY(bottomGO, 0, 0);

		LeanTween.alphaCanvas(batteryUICanvas, 1f, 0);
		batteryUICanvas.blocksRaycasts = true;

		LeanTween.moveLocalY(bottomGO, 0, 0);
		LeanTween.moveLocalX(interactablesGO, 0, 0);

		foreach (Toggle toggle in passcodeToggles)
		{
			toggle.isOn = false;
		}
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

	IEnumerator AcceptBio()
	{
		yield return new WaitForSeconds(0.75f);

		BioAccept?.Invoke();
	}
}