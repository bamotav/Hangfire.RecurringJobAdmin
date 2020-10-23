using Hangfire.Annotations;
using Hangfire.RecurringJobAdmin.Core;
using Hangfire.RecurringJobAdmin.Models;
using Hangfire.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobAdmin.Pages
{
    internal sealed class GetJobsStoppedDispatcher : Dashboard.IDashboardDispatcher
    {
        private readonly IStorageConnection _connection;
        public GetJobsStoppedDispatcher()
        {
            _connection = JobStorage.Current.GetConnection();
        }
        public async Task Dispatch([NotNull] Dashboard.DashboardContext context)
        {
            if (!"GET".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.StatusCode = 405;

                return;
            }

            var periodicJob = new List<PeriodicJob>();
            periodicJob.AddRange(JobAgent.GetAllJobStopped());

            await context.Response.WriteAsync(JsonConvert.SerializeObject(periodicJob));
        }
    }
}
