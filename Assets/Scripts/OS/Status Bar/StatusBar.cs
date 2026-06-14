using OS6.Kernel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private TOSSP6 main;
    private MobileGestalt mobileGestalt;

    //Status Bars
    [Space(10)]
    [Header("Bars")]
    [SerializeField] private GameObject statusBarHolder;
    public BarType barType;

    //Left Side
    [Space(10)]
    [Header("Left Side")]
    public TMP_Text deviceName;

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

    [Space(10)]
    [SerializeField] private GameObject alarmIcon;

    [Space(10)]
    [SerializeField] private GameObject btDisconnected;
    [SerializeField] private GameObject btConnecting;
    [SerializeField] private GameObject btConnected;

    [Space(10)]
    [SerializeField] private GameObject musicPlay;

    [Space(10)]
    [SerializeField] private GameObject rotLock;

    [Space(10)]
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

    private void Awake()
    {
        TOSSP6.DeviceUnlocked += NormalBar;
        UI_NotificationCentre.ShowNotificationCentre += NotificationBar;
        UI_NotificationCentre.HideNotificationCentre += NormalBar;

        smallTimeLabel.gameObject.SetActive(false);

        statusBarHolder.GetComponent<Canvas>().sortingOrder = 500;

        mobileGestalt = System_Services.GetService<MobileGestalt>();
    }

    void Start()
    {
        SetCarrierText();
    }

    void Update()
    {
        SetTime();
        SetDate();
        SetBatteryPercent();
        SetBatteryState();
    }

    private void SetCarrierText()
    {
        switch (mobileGestalt.DeviceType)
        {
            case MobileGestalt.IDeviceType.iPhone:
                deviceName.text = "No SIM";
                break;
            case MobileGestalt.IDeviceType.iPad:
                deviceName.text = "iPad";
                break;
            case MobileGestalt.IDeviceType.iPod:
                deviceName.text = "iPod";
                break;
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
        var batteryStatus = SystemInfo.batteryStatus;

        main.isSystemCharging = batteryStatus == BatteryStatus.Charging;
        bool isSystemHolding = batteryStatus == BatteryStatus.NotCharging
            || batteryStatus == BatteryStatus.Full
            || batteryStatus == BatteryStatus.Unknown
            || SystemInfo.batteryLevel == -1f;

        batteryCharging.SetActive(main.isSystemCharging);
        batteryNormal.SetActive(!main.isSystemCharging && !isSystemHolding);
        batteryHold.SetActive(isSystemHolding);

        if (batteryStatus == BatteryStatus.Discharging)
        {
            hasChargingSoundPlayed = false;
        }
        else if (main.isSystemCharging || isSystemHolding)
        {
            if (!hasChargingSoundPlayed)
            {
                main.audioSource.PlayOneShot(main.systemSFX[(int)SystemSounds.CHARGING]);
                hasChargingSoundPlayed = true;
            }
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