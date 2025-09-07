using System.Runtime.InteropServices;
using Unity.VectorGraphics;
using UnityEngine;

public class WiFiSignal : MonoBehaviour
{
    [SerializeField] private Sprite[] variants;
    [SerializeField] private GameObject wifiIconGO;

    private SVGImage wifiIcon;

    [SerializeField] bool debugMode;

    [DllImport("WiFiRSSIReader")]
    private static extern int GetCurrentRSSI();

    private const float CheckInterval = 5f;

    private void Start()
    {
        wifiIcon = wifiIconGO.GetComponent<SVGImage>();
        InvokeRepeating(nameof(CheckRSSI), 0f, CheckInterval);
    }

    private void CheckRSSI()
    {
        int currentRSSI = GetCurrentRSSI();

        if (currentRSSI == 100 || currentRSSI == -4)
        {
            wifiIcon.sprite = null;
            wifiIconGO.SetActive(false);

            if (debugMode)
            {
                Debug.Log("Network Disconnected");
            }

        }
        else
        {
            wifiIconGO.SetActive(true);
            wifiIcon.sprite = GetSpriteForRSSI(currentRSSI);

            if (debugMode)
            {
                Debug.Log($"Current RSSI: {currentRSSI}");
            }

        }
    }

    private Sprite GetSpriteForRSSI(int rssi)
    {
        if (IsBetween(rssi, 75, 99))
        {
            return variants[3];
        }
        else if (IsBetween(rssi, 50, 74))
        {
            return variants[2];
        }
        else if (IsBetween(rssi, 25, 49))
        {
            return variants[1];
        }
        else if (IsBetween(rssi, 0, 24))
        {
            return variants[0];
        }

        return null;
    }

    private bool IsBetween(int value, int lower, int upper)
    {
        return value >= Mathf.Min(lower, upper) && value <= Mathf.Max(lower, upper);
    }
}
