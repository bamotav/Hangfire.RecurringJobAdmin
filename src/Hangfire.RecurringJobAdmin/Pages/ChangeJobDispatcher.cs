using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.RecurringJobAdmin.Core;
using Hangfire.RecurringJobAdmin.Models;
using Hangfire.States;
using Hangfire.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobAdmin.Pages
{
    internal sealed class ChangeJobDispatcher : IDashboardDispatcher
    {
        private readonly IStorageConnection _connection;
        private readonly RecurringJobRegistry _recurringJobRegistry;

        public ChangeJobDispatcher()
        {

            _connection = JobStorage.Current.GetConnection();
            _recurringJobRegistry = new RecurringJobRegistry();
        }


        public async Task Dispatch([NotNull] DashboardContext context)
        {
            var response = new Response() { Status = true };

            var job = new PeriodicJob();
            job.Id = context.Request.GetQuery("Id");
            job.Cron = context.Request.GetQuery("Cron");
            job.Class = context.Request.GetQuery("Class");
            job.Method = context.Request.GetQuery("Method");
            job.Queue = context.Request.GetQuery("Queue");
            job.TimeZoneId = context.Request.GetQuery("TimeZoneId");

            var timeZone = TimeZoneInfo.Utc;

            if (!Utility.IsValidSchedule(job.Cron))
            {
                response.Status = false;
                response.Message = "Invalid CRON";

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

                return;
            }

            try
            {
                if (!string.IsNullOrEmpty(job.TimeZoneId))
                {
                    timeZone = TimeZoneInfo.FindSystemTimeZoneById(job.TimeZoneId);
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

                return;
            }
          

            if (!StorageAssemblySingleton.GetInstance().IsValidType(job.Class))
            {
                response.Status = false;
                response.Message = "The Class not found";

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

                return;
            }

            if (!StorageAssemblySingleton.GetInstance().IsValidMethod(job.Class, job.Method))
            {
                response.Status = false;
                response.Message = "The Method not found";

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

                return;
            }


            var methodInfo = StorageAssemblySingleton.GetInstance().currentAssembly
                                                                                .Where(x => x?.GetType(job.Class)?.GetMethod(job.Method) != null)
                                                                                .FirstOrDefault()
                                                                                .GetType(job.Class)
                                                                                .GetMethod(job.Method);

            _recurringJobRegistry.Register(
                      job.Id,
                      methodInfo,
                      job.Cron,
                      timeZone,
                      job.Queue ?? EnqueuedState.DefaultQueue);


            context.Response.StatusCode = (int)HttpStatusCode.OK;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

        }
    }
}
