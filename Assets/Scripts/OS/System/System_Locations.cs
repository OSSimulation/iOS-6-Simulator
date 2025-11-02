using System.IO;
using UnityEngine;

public class System_Locations : MonoBehaviour
{
    private static string rootFS => Path.Combine(Application.persistentDataPath, "rootFS");

    public static string USER_APPLICATIONS => Path.Combine(rootFS, "var/mobile/Applications/");
    public static string USER_PREFERENCES => Path.Combine(rootFS, "var/mobile/Library/Preferences/");
    public static string USER_IMAGES => Path.Combine(rootFS, "var/mobile/Media/DCIM/");

    public static string SYS_PREFERENCES => Path.Combine(rootFS, "Library/Preferences/");
    public static string RINGTONES => Path.Combine(rootFS, "Library/Ringtones/");
    public static string WALLPAPERS => Path.Combine(rootFS, "Library/Wallpapers/");
}
