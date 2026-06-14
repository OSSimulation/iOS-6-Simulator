using System;
using System.Collections.Generic;

namespace OS6.Kernel
{
    public static class System_Services
    {
        private static Dictionary<Type, object> services = new();

        public static void RegisterService<T>(T service)
        {
            services[typeof(T)] = service;
        }

        public static T GetService<T>()
        {
            return (T)services[typeof(T)];
        }
    }
}
