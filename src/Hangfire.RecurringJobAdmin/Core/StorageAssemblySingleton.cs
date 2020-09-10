using System;
using System.Reflection;

namespace Hangfire.RecurringJobAdmin.Core
{
    public class StorageAssemblySingleton
    {
        private StorageAssemblySingleton()
        {
        }

        private static StorageAssemblySingleton _instance;

        public Assembly _assembly { get; private set; }

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
            _assembly = assembly;
        }
    }
}
