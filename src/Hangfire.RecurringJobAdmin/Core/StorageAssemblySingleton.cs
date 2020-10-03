using Hangfire.Annotations;
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
        private string[] prefixIgnore = new[] { "Hangfire.RecurringJobAdmin.dll", "Microsoft." };


        public List<Assembly> currentAssembly { get; private set; } = new List<Assembly>();

        internal static StorageAssemblySingleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new StorageAssemblySingleton();
            }
            return _instance;
        }

        internal void SetCurrentAssembly(bool includeReferences = false, params Assembly[] assemblies)
        {
            currentAssembly.AddRange(assemblies);

            if (includeReferences)
            {
                var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
                var toLoad = referencedPaths.Where(r => !assemblies.Any(x => x.Location.Equals(r)))
                                .Where(x => !prefixIgnore.Any(p => p.Contains(x)))
                                .ToList();

                toLoad.ForEach(path => currentAssembly.Add(Assembly.LoadFile(path)));
            }

        }

        public bool IsValidType(string type) => currentAssembly.Any(x => x.GetType(type) != null);

        public bool IsValidMethod(string type, string method) => currentAssembly?
                                                                    .FirstOrDefault(x => x.GetType(type) != null)?.GetType(type)?.GetMethod(method) != null;


    }
}
