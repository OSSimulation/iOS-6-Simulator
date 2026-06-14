using Microsoft.Win32;
using OS6.Power;
using System.Diagnostics;
using UnityEngine;

namespace OS6.IO.Displays
{
    public partial class Display_Manager
    {
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

                    Power_Manager.Reboot();
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
}
