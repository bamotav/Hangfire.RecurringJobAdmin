using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.RecurringJobAdmin.Core;
using Hangfire.RecurringJobAdmin.Models;
using Hangfire.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using System.Threading.Tasks;

namespace Hangfire.RecurringJobAdmin.Pages
{
    internal sealed class GetJobForEdit : IDashboardDispatcher
    {
        private readonly IStorageConnection _connection;

        public GetJobForEdit()
        {
            _connection = JobStorage.Current.GetConnection();
        }
        public async Task Dispatch([NotNull] DashboardContext conterecurringJobt)
        {

            var response = new Response() { Status = true };

            if (!"GET".Equals(conterecurringJobt.Request.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                conterecurringJobt.Response.StatusCode = 405;

                return;
            }

            var jobId = conterecurringJobt.Request.GetQuery("Id");
            //var jobId = (await conterecurringJobt.Request.GetFormValuesAsync("Id"))[0];


            var recurringJob = _connection.GetRecurringJobs().FirstOrDefault(x => x.Id == jobId);

            if (recurringJob == null)
            {
                response.Status = false;
                response.Message = "Job not found";

                await conterecurringJobt.Response.WriteAsync(JsonConvert.SerializeObject(response));

                return;
            }

            var periodicJob = new PeriodicJob
            {
                Id = recurringJob.Id,
                Cron = recurringJob.Cron,
                CreatedAt = recurringJob.CreatedAt,
                Error = recurringJob.Error,
//                LastExecution = recurringJob.LastExecution,
                Method = recurringJob.Job.Method.Name,
                Class = recurringJob.Job.Method.ReflectedType.FullName,
                Queue = recurringJob.Queue,
                LastJobId = recurringJob.LastJobId,
                LastJobState = recurringJob.LastJobState,
//                NextExecution = recurringJob.NextExecution,
                Removed = recurringJob.Removed,
                TimeZoneId = recurringJob.TimeZoneId
            };

            response.Object = periodicJob;



            await conterecurringJobt.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
