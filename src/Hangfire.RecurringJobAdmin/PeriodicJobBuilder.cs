using Hangfire.RecurringJobAdmin.Core;
using Hangfire.States;
using System;
using System.Linq;
using System.Reflection;

namespace Hangfire.RecurringJobAdmin
{
    internal static class PeriodicJobBuilder
    {
        internal static void GetAllJobs()
        {
            var _registry = new RecurringJobRegistry();

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
                        }

                        if(!JobAgent.IsValidJobId(attribute.RecurringJobId) && !JobAgent.IsValidJobId(attribute.RecurringJobId, JobAgent.tagStopJob))
                        {
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
}
