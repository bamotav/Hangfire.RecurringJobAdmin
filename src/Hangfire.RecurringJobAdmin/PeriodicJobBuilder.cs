using Hangfire.RecurringJobAdmin.Core;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hangfire.RecurringJobAdmin
{

    internal static class PeriodicJobBuilder
    {
        public static List<RecurringJobAttribute> Metadata { get; private set; }
        internal static void GetAllJobs()
        {
            Metadata = new List<RecurringJobAttribute>();

            foreach (var assembly in StorageAssemblySingleton.GetInstance().currentAssembly)
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetTypeInfo().DeclaredMethods)
                    {

                        if (!method.IsDefined(typeof(RecurringJobAttribute), false)) continue;

                        var attribute = method.GetCustomAttribute<RecurringJobAttribute>(false);

                        if (attribute == null) continue;

                        if (method.GetCustomAttributes(true).OfType<RecurringJobAttribute>().Any())
                        {
                            var attr = method.GetCustomAttribute<RecurringJobAttribute>();
                            Metadata.Add(attr);
                        }

                        var _registry = new RecurringJobRegistry();

                        _registry.Register(
                                  attribute.RecurringJobId,
                                  method,
                                  attribute.Cron,
                                  string.IsNullOrEmpty(attribute.TimeZone) ? TimeZoneInfo.Utc : TimeZoneInfo.FindSystemTimeZoneById(attribute.TimeZone),
                                  attribute.Queue ?? EnqueuedState.DefaultQueue);

                    }

                }
            }

        }
    }
}
