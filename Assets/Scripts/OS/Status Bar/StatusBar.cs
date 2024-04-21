using UnityEngine;
using TMPro;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private TOSSP6 main;

    //Status Bars
    [Space(10)]
    [Header("Bars")]
    [SerializeField] private GameObject lockStatus;
    [SerializeField] private GameObject normalStatus;

    //Date & Time
    [Space(10)]
    [Header("Date & Time")]
    [SerializeField] private TMP_Text smallTimeLabel;
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
    }

    void Update()
    {
        SetTime();
        SetDate();
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

        if (!is24HourTime && !main.isLockScreen)
        {
            smallTimeLabel.text = time.ToString("h:mm tt");
        }
        else if (is24HourTime && !main.isLockScreen)
        {
            smallTimeLabel.text = time.ToString("HH:mm");
        }
        else if (main.isLockScreen && !is24HourTime)
        {
            largeTimeLabel.text = time.ToString("h:mm");
        }
        else if (main.isLockScreen && is24HourTime)
        {
            largeTimeLabel.text = time.ToString("HH:mm");
        }
    }

    public void SetDate()
    {
        System.DateTime theDate = System.DateTime.Now;
        date = System.DateTime.Now.ToString("dddd, d MMMM");
        dateLabel.text = date;
    }

    public void SetBatteryPercent()
    {
        batteryPercentLabel.text = SystemInfo.batteryLevel.ToString() + "%";
    }

    public void NormalBar()
    {
        normalStatus.SetActive(true);
        lockStatus.SetActive(false);
    }

    public void LockBar()
    {
        normalStatus.SetActive(false);
        lockStatus.SetActive(true);
    }
}
