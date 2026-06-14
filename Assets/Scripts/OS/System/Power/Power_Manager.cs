using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace OS6.Power
{
    public class Power_Manager : MonoBehaviour
    {
        private Mutex mutex;

        private void Start()
        {
            mutex = new Mutex(true, "com.OSSimulation.iOS6Simulator", out bool createdNew);

            if (!createdNew)
            {
                mutex = null;
                Application.Quit();
            }
        }

        public void Reboot()
        {
            ReleaseMutex();
            Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            Application.Quit();
        }

        public void Shutdown()
        {
            ReleaseMutex();
            Application.Quit();
        }

        private void OnApplicationQuit()
        {
            ReleaseMutex();
        }

        private void ReleaseMutex()
        {
            if (mutex == null) return;

            mutex.ReleaseMutex();
            mutex.Dispose();
            mutex = null;
        }
    }
}
