using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private TOSSP6 main;

    //Status Bars
    [Space(10)]
    [Header("Bars")]
    [SerializeField] private GameObject statusBarHolder;
    public BarType barType;

    //Left Side
    [Space(10)]
    [Header("Left Side")]
    public GameObject deviceName;

    //Middle
    [Space(10)]
    [Header("Middle")]
    [SerializeField] private TMP_Text smallTimeLabel;

    [SerializeField] private GameObject dnd;

    //Right Side
    [Space(10)]
    [Header("Right Side")]
    [SerializeField] private GameObject batteryNormal;
    [SerializeField] private GameObject batteryCharging;
    [SerializeField] private GameObject batteryHold;
    [SerializeField] private Image batteryFill;

    [SerializeField] private GameObject alarmIcon;

    [SerializeField] private GameObject btDisconnected;
    [SerializeField] private GameObject btConnecting;
    [SerializeField] private GameObject btConnected;

    [SerializeField] private GameObject musicPlay;

    [SerializeField] private GameObject rotLock;

    bool hasChargingSoundPlayed = false;

    //Date & Time
    [Space(10)]
    [Header("Date & Time")]
    [SerializeField] private TMP_Text largeTimeLabel;
    [SerializeField] private TMP_Text dateLabel;
    private string date;
    public bool is24HourTime = false;

    //Battery
    [Space(10)]
    [Header("Battery")]
    [SerializeField] private TMP_Text batteryPercentLabel;

    void Start()
    {
        TOSSP6.DeviceUnlocked += NormalBar;
        UI_NotificationCentre.ShowNotificationCentre += NotificationBar;
        UI_NotificationCentre.HideNotificationCentre += NormalBar;

        smallTimeLabel.gameObject.SetActive(false);

        statusBarHolder.GetComponent<Canvas>().sortingOrder = 500;
    }

    void Update()
    {
        SetTime();
        SetDate();
        SetBatteryPercent();
        SetBatteryState();
    }

    private void CheckStatusBarSettings()
    {
        if (PlayerPrefs.GetInt("TimeFormat") == 1)
        {
            is24HourTime = true;
        }
        else if (PlayerPrefs.GetInt("TimeFormat") == 0)
        {
            is24HourTime = false;
        }
    }

    public void SetTime()
    {
        System.DateTime time = System.DateTime.Now;

        if (!is24HourTime)
        {
            smallTimeLabel.text = time.ToString("h:mm tt");
            largeTimeLabel.text = time.ToString("h:mm");
        }
        else if (is24HourTime)
        {
            smallTimeLabel.text = time.ToString("HH:mm");
            largeTimeLabel.text = time.ToString("HH:mm");
        }
    }

    public void SetDate()
    {
        date = System.DateTime.Now.ToString("dddd, d MMMM");
        dateLabel.text = date;
    }

    public void SetBatteryPercent()
    {
        batteryFill.fillAmount = SystemInfo.batteryLevel;
    }

    public void SetBatteryState()
    {
        if (SystemInfo.batteryStatus == BatteryStatus.NotCharging || SystemInfo.batteryStatus == BatteryStatus.Full)
        {
            batteryCharging.SetActive(false);
            batteryNormal.SetActive(false);
            batteryHold.SetActive(true);

            main.isSystemCharging = true;

            if (!hasChargingSoundPlayed)
            {
                main.soundManager.PlaySound(SoundEvents.SYSTEM_CHARGE, main.GetSFXVolume(), SoundSources.SYSTEM_SFX);
                hasChargingSoundPlayed = true;
            }
        }
        else if (SystemInfo.batteryStatus == BatteryStatus.Charging)
        {
            batteryCharging.SetActive(true);
            batteryNormal.SetActive(false);
            batteryHold.SetActive(false);

            main.isSystemCharging = true;

            if (!hasChargingSoundPlayed)
            {
                main.soundManager.PlaySound(SoundEvents.SYSTEM_CHARGE, main.GetSFXVolume(), SoundSources.SYSTEM_SFX);
                hasChargingSoundPlayed = true;
            }
        }
        else if (SystemInfo.batteryStatus == BatteryStatus.Unknown)
        {
            batteryCharging.SetActive(false);
            batteryNormal.SetActive(false);
            batteryHold.SetActive(true);

            main.isSystemCharging = false;
        }
        else if (SystemInfo.batteryStatus == BatteryStatus.Discharging)
        {
            batteryCharging.SetActive(false);
            batteryNormal.SetActive(true);
            batteryHold.SetActive(true);

            main.isSystemCharging = false;

            hasChargingSoundPlayed = false;
        }
    }

    public void NormalBar()
    {
        statusBarHolder.GetComponent<Canvas>().sortingOrder = 500;
        smallTimeLabel.gameObject.SetActive(true);
        barType = BarType.NORMAL;
    }

    public void LockBar()
    {
        statusBarHolder.GetComponent<Canvas>().sortingOrder = 500;
        smallTimeLabel.gameObject.SetActive(false);
        barType = BarType.LOCK;
    }

    public void NotificationBar()
    {
        statusBarHolder.GetComponent<Canvas>().sortingOrder = 501;
        barType = BarType.NOTIFICATION;
    }
}

public enum BarType
{
    LOCK,
    NORMAL,
    NOTIFICATION
}