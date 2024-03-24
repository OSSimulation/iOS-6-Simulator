using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusBar : MonoBehaviour
{
    [SerializeField] private TOSSP6 main;

    //Bars
    [Space(10)]
    [Header("Bars")]
    [SerializeField] private GameObject lockStatus;
    [SerializeField] private GameObject mainStatus;

    //Date & Time
    [Space(10)]
    [Header("Date & Time")]
    [SerializeField] private TMP_Text smallTimeLabel;
    private string date;
    public bool is12HourTime = true;

    //Battery
    [Space(10)]
    [Header("Battery")]
    [SerializeField] private TMP_Text batteryPercentLabel;

    void Start()
    {

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
            is12HourTime = true;
        }
        else if (PlayerPrefs.GetInt("TimeFormat") == 0)
        {
            is12HourTime = false;
        }
    }

    public void SetTime()
    {
        System.DateTime time = System.DateTime.Now;

        if (is12HourTime && !main.isLockScreen)
        {
            smallTimeLabel.text = time.ToString("h:mm tt");
        }
        else if (!is12HourTime && !main.isLockScreen)
        {
            smallTimeLabel.text = time.ToString("HH:mm");
        }
        else if (main.isLockScreen && is12HourTime)
        {
            smallTimeLabel.text = time.ToString("h:mm");
        }
        else if (main.isLockScreen && !is12HourTime)
        {
            smallTimeLabel.text = time.ToString("HH:mm");
        }
    }

    public void SetDate()
    {
        System.DateTime theDate = System.DateTime.Now;
        date = System.DateTime.Now.ToString("dddd, d MMMM");
    }

    public void SetBatteryPercent()
    {
        batteryPercentLabel.text = SystemInfo.batteryLevel.ToString() + "%";
    }

    public void SetChargeStatus()
    {

    }
}
