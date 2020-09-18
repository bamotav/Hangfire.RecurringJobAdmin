using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Hangfire.RecurringJobAdmin.Core
{
    internal sealed class StorageAssemblySingleton
    {
        private StorageAssemblySingleton()
        {
        }

        private static StorageAssemblySingleton _instance;

        public List<Assembly> currentAssembly { get; private set; } = new List<Assembly>();

        internal static StorageAssemblySingleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new StorageAssemblySingleton();
            }
            return _instance;
        }

        internal void SetCurrentAssembly(Assembly assembly)
        {
            currentAssembly.Add(assembly);

            var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            var toLoad = referencedPaths.Where(r => !assembly.Location.Equals(r))
                                        .Where(x => !x.Contains("Hangfire.RecurringJobAdmin.dll"))
                                        .ToList();

            toLoad.ForEach(path => currentAssembly.Add(Assembly.LoadFile(path)));

        }

        public bool IsValidType(string type) => currentAssembly.Any(x => x.GetType(type) != null);

        public bool IsValidMethod(string type, string method) => currentAssembly?
                                                                    .FirstOrDefault(x => x.GetType(type) != null)?.GetType(type)?.GetMethod(method) != null;


    }
}
