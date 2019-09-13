using Hangfire.RecurringJobAdmin.Core;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hangfire.RecurringJobAdmin
{

    //https://github.com/pjrharley/Hangfire.Core.Dashboard.Management/blob/master/Support/JobsHelper.cs
    public static class PeriodicJobBuilder
    {
        public static List<RecurringJobAttribute> Metadata { get; private set; }
        internal static void GetAllJobs(Assembly assembly)
        {
            Metadata = new List<RecurringJobAttribute>();

            foreach (var type in assembly.GetTypes())
            {
                foreach (var method in type.GetTypeInfo().DeclaredMethods)
                {
                    //foreach (Type ti in assembly.GetTypes().Where(x => !x.IsInterface && x.GetCustomAttribute<RecurringJobAttribute>() != null))
                    //{

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

                    //foreach (var methodInfo in ti.GetMethods().Where(m => m.DeclaringType == ti))
                    //{
                    //    var meta = new JobMetadata { Type = ti, Queue = q };

                    //    if (methodInfo.GetCustomAttributes(true).OfType<DescriptionAttribute>().Any())
                    //    {
                    //        meta.Description = methodInfo.GetCustomAttribute<DescriptionAttribute>().Description;
                    //    }

                    //    if (methodInfo.GetCustomAttributes(true).OfType<DisplayNameAttribute>().Any())
                    //    {
                    //        meta.MethodInfo = methodInfo;
                    //        meta.DisplayName = methodInfo.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                    //    }

                    //    Metadata.Add(meta);
                    //}
                }

            }
        }
    }
}
