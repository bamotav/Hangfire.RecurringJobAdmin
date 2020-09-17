using System;
using System.Reflection;

namespace Hangfire.RecurringJobAdmin.Core
{
    internal sealed class StorageAssemblySingleton
    {
        private StorageAssemblySingleton()
        {
        }

        private static StorageAssemblySingleton _instance;

        public Assembly currentAssembly { get; private set; }

        public static StorageAssemblySingleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new StorageAssemblySingleton();
            }
            return _instance;
        }

        public void SetCurrentAssembly(Assembly assembly)
        {
            currentAssembly = assembly;
        }

        public bool IsValidType(string type) => currentAssembly.GetType(type) != null;

        public bool IsValidMethod(string type, string method) => currentAssembly.GetType(type).GetMethod(method) != null;
    }
}
