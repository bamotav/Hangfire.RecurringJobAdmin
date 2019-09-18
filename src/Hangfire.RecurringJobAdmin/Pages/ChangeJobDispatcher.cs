using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.RecurringJobAdmin.Core;
using Hangfire.RecurringJobAdmin.Models;
using Hangfire.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobAdmin.Pages
{
    internal sealed class ChangeJobDispatcher : IDashboardDispatcher
    {
        private readonly IStorageConnection _connection;
        public ChangeJobDispatcher()
        {

            _connection = JobStorage.Current.GetConnection();
        }


        public async Task Dispatch([NotNull] DashboardContext context)
        {
            var response = new Response() { Status = true };

            var job = new PeriodicJob();
            job.Id = (await context.Request.GetFormValuesAsync("Id"))[0];
            job.Cron = (await context.Request.GetFormValuesAsync("Cron"))[0];
            job.Class = (await context.Request.GetFormValuesAsync("Class"))[0];
            job.Method = (await context.Request.GetFormValuesAsync("Method"))[0];
            job.Queue = (await context.Request.GetFormValuesAsync("Queue"))[0];

            if (Utility.IsValidSchedule(job.Cron))
            {
                response.Status = false;
                response.Message = "Invalid CRON";

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

                return;
            }
            

            var manager = new RecurringJobManager(context.Storage);

            manager.AddOrUpdate(job.Id, () => ReflectionHelper.InvokeVoidMethod(job.Class, job.Method), job.Cron, TimeZoneInfo.Utc, job.Queue);

            context.Response.StatusCode = (int)HttpStatusCode.OK;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

        }
    }
}
