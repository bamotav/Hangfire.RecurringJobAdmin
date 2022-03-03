using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.RecurringJobAdmin.Core;
using Hangfire.RecurringJobAdmin.Models;
using Hangfire.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobAdmin.Pages
{
    internal sealed class GetJobDispatcher : Dashboard.IDashboardDispatcher
    {
        private readonly IStorageConnection _connection;
        public GetJobDispatcher()
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


            var recurringJob = _connection.GetRecurringJobs();
            var periodicJob = new List<PeriodicJob>();
            

            CultureInfo US_TimeFormat = new CultureInfo("en-US");

            if (recurringJob.Count >  0)
            {
                recurringJob.ForEach((x) =>
                {
                    periodicJob.Add(new PeriodicJob
                    {
                        Id = x.Id,
                        Cron = x.Cron,
                        CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.Value.ChangeTimeZone(x.TimeZoneId) : new DateTime(),
                        Error = x.Error,
                        LastExecution = x.LastExecution.HasValue ? x.LastExecution.Value.ChangeTimeZone(x.TimeZoneId).ToString("G", US_TimeFormat) : "N/A",
                        Method = x.Job?.Method.Name,
                        JobState = "Running",
                        Class = x.Job?.Type.Name,
                        Queue = x.Queue,
                        LastJobId = x.LastJobId,
                        LastJobState = x.LastJobState,
                        NextExecution = x.NextExecution.HasValue ? x.NextExecution.Value.ChangeTimeZone(x.TimeZoneId).ToString("G", US_TimeFormat) : "N/A",
                        Removed = x.Removed,
                        TimeZoneId = x.TimeZoneId
                    });
                });
            }
            //Add job was stopped:
            periodicJob.AddRange(JobAgent.GetAllJobStopped());

            await context.Response.WriteAsync(JsonConvert.SerializeObject(periodicJob));
        }
    }
}
