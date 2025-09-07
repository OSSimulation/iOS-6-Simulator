using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class OS6_Editor_Menu : MonoBehaviour
{
    [MenuItem("OS6/Open Filesystem", priority = 0)]
    static void OpenFileSystem()
    {
        Process.Start(Application.persistentDataPath);
    }

    [MenuItem("OS6/Reset Home Screen Layout", priority = 11)]
    static void ResetHomeScreen()
    {
        string layoutFilePath = System_Locations.SYS_PREFERENCES + "com.os6.iconState.json";

        if (File.Exists(layoutFilePath))
        {
            if (EditorUtility.DisplayDialog("Reset Home Screen Home Screen Layout", "Are you sure you want to reset the Home Screen Layout? This cannot be undone. Changes will take effect after a restart.", "Yes", "No"))
            {
                File.Delete(layoutFilePath);
            }
        }
    }

    [MenuItem("OS6/Get Smooth Grid Layout", priority = 100)]
    static void SmoothGridLayout()
    {
        if (EditorUtility.DisplayDialog("Get 'Smooth Grid UI Layout'", "You will be redirected to the Unity Asset Store. If you decide to fork or distribute the iOS 6 Simulator source code, this package cannot be included.", "iAgree", "iDisagree"))
        {
            Application.OpenURL("https://assetstore.unity.com/packages/tools/gui/smooth-grid-layout-ui-208357");
        }
    }
}
