using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;

namespace OS6.Power
{
    public class Power_Manager : MonoBehaviour
    {
        [DoesNotReturn]
        public static void Reboot()
        {
            PlayerSettings.forceSingleInstance = false;
            Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            Application.Quit();
        }

        [DoesNotReturn]
        public static void Shutdown()
        {
            Application.Quit();
        }
    }
}
