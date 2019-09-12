using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.JobExtensions.Core;
using Hangfire.JobExtensions.Models;
using Hangfire.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.JobExtensions.Pages
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
            var recurringJob = _connection.GetRecurringJobs();
            var periodicJob = new List<PeriodicJob>();

            recurringJob.ForEach((x) =>
            {

                periodicJob.Add(new PeriodicJob
                {
                    Id = x.Id,
                    Cron = x.Cron,
                    CreatedAt = x.CreatedAt,
                    Error = x.Error,
                    LastExecution = x.LastExecution,
                    Queue = x.Queue,
                    LastJobId = x.LastJobId,
                    LastJobState = x.LastJobState,
                    NextExecution = x.NextExecution,
                    Removed = x.Removed,
                    TimeZoneId = x.TimeZoneId
                });
            });



            if (!"GET".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.StatusCode = 405;

                return;
            }

            await context.Response.WriteAsync(JsonConvert.SerializeObject(periodicJob));
        }
    }
}
