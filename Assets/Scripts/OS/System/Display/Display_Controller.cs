using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Display_Controller : MonoBehaviour
{
    int currentDisplay = 0;
    int previousDisplay = -1;

    float aspectRatio = 640f / 1136f;

    List<DisplayInfo> displayList = new();

    private void Start()
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        OverrideDPISettings();
#endif

        UpdateDisplayDetails();
        SetResolution(currentDisplay);

        DisplayInfo displayInfo = displayList[currentDisplay];
        SetWindowPosition(displayInfo);
    }

    private void Update()
    {
        if (UpdateDisplayDetails() || CheckWindowSize(currentDisplay))
            SetResolution(currentDisplay);
    }

    private void OverrideDPISettings()
    {
        string executablePath = Process.GetCurrentProcess().MainModule.FileName;
        string regKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers";
        string regKeyValue = "~ HIGHDPIAWARE";

        try
        {
            using RegistryKey key = Registry.CurrentUser.CreateSubKey(regKey, writable: true);
            object currentKeyValue = key.GetValue(executablePath);

            if (currentKeyValue == null || currentKeyValue.ToString() != regKeyValue)
            {
                key.SetValue(executablePath, regKeyValue, RegistryValueKind.String);
                UnityEngine.Debug.Log("Updated DPI Scaling Registry Value");

                RestartApp(executablePath);
            }
            else
            {
                UnityEngine.Debug.Log("DPI Scaling Registry Value Is Already Correct");
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("Failed To Update DPI Scaling Registry Value: " + e.Message);
        }
    }

    private void RestartApp(string exePath)
    {
        Process.Start(exePath);
        Application.Quit();
    }

    private bool UpdateDisplayDetails()
    {
        displayList.Clear();
        Screen.GetDisplayLayout(displayList);

        int newDisplayInt = displayList.IndexOf(Screen.mainWindowDisplayInfo);

        if (newDisplayInt != currentDisplay)
        {
            previousDisplay = currentDisplay;
            currentDisplay = newDisplayInt;
            return true;
        }

        return false;
    }

    private bool CheckWindowSize(int displayIndex)
    {
        if (displayIndex < 0 || displayIndex >= Display.displays.Length)
            return false;
        
        Display display = Display.displays[displayIndex];
        int maxHeight = Mathf.RoundToInt(display.systemHeight * 0.75f);

        if (Screen.height > maxHeight)
        {
            return true;
        }

        return false;
    }

    private void SetWindowPosition(DisplayInfo display)
    {
        Vector2Int centre = new(display.width / 2, display.height / 2);
        Vector2Int currentSize = new(Screen.width, Screen.height);
        Vector2Int newPostition = centre - (currentSize / 2);

        Screen.MoveMainWindowTo(display, newPostition);
    }

    private void SetResolution(int displayIndex)
    {
        if (displayIndex < 0 || displayIndex >= Display.displays.Length)
            return;

        Display display = Display.displays[displayIndex];

        int maxHeight = Mathf.RoundToInt(display.systemHeight * 0.75f);
        int maxWidth = Mathf.RoundToInt(maxHeight * aspectRatio);

        if (maxWidth > display.systemWidth)
        {
            maxWidth = Mathf.RoundToInt(Display.main.systemWidth * 0.75f);
            maxHeight = Mathf.RoundToInt(maxWidth / aspectRatio);
        }

        Screen.SetResolution(maxWidth, maxHeight, false);
    }
}
