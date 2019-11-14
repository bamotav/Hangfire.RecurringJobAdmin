using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.RecurringJobAdmin.Core;
using Hangfire.RecurringJobAdmin.Models;
using Hangfire.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobAdmin.Pages
{
    internal sealed class GetJobDispatcher : IDashboardDispatcher
    {
        private readonly IStorageConnection _connection;
        public GetJobDispatcher()
        {
            _connection = JobStorage.Current.GetConnection();
        }
        public async Task Dispatch([NotNull] DashboardContext context)
        {
            if (!"GET".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.StatusCode = 405;

                return;
            }


            var recurringJob = _connection.GetRecurringJobs();
            var periodicJob = new List<PeriodicJob>();

            if (periodicJob.Count >  0)
            {
                recurringJob.ForEach((x) =>
                {
                    periodicJob.Add(new PeriodicJob
                    {
                        Id = x.Id,
                        Cron = x.Cron,
                        CreatedAt = x.CreatedAt,
                        Error = x.Error,
                        LastExecution = x.LastExecution,
                        Method = x.Job.Method.Name,
                        Class = x.Job.Method.ReflectedType.FullName,
                        Queue = x.Queue,
                        LastJobId = x.LastJobId,
                        LastJobState = x.LastJobState,
                        NextExecution = x.NextExecution,
                        Removed = x.Removed,
                        TimeZoneId = x.TimeZoneId
                    });
                });
            }
            



           

            await context.Response.WriteAsync(JsonConvert.SerializeObject(periodicJob));
        }
    }
}
