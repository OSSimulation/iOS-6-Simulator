using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

namespace OS6.Authentication
{
    public class ValidateBiometrics : MonoBehaviour
    {
        [DllImport("BiometricAuthentication")]
        private static extern bool Authenticate();

        public static async Task<bool> AuthenticateBiometrics()
        {
            bool authStatus = await Task.Run(() => Authenticate());
            return authStatus;
        }
    }
}