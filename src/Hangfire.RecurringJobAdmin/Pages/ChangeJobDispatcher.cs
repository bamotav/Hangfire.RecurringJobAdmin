using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.RecurringJobAdmin.Core;
using Hangfire.RecurringJobAdmin.Models;
using Hangfire.States;
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
            job.Id = context.Request.GetQuery("Id");
            job.Cron = context.Request.GetQuery("Cron");
            job.Class = context.Request.GetQuery("Class");
            job.Method = context.Request.GetQuery("Method");
            job.Queue = context.Request.GetQuery("Queue");

            if (!Utility.IsValidSchedule(job.Cron))
            {
                response.Status = false;
                response.Message = "Invalid CRON";

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

                return;
            }
            

          //  var manager = new RecurringJobManager(context.Storage);

         //   manager.AddOrUpdate(job.Id, () => ReflectionHelper.InvokeVoidMethod(job.Class, job.Method), job.Cron, TimeZoneInfo.Utc, job.Queue);


            var _registry = new RecurringJobRegistry();

           

            Type calledType = StorageAssemblySingleton.GetInstance()._assembly.GetType(job.Class);

            var methodInfo = calledType.GetMethod(job.Method);

            _registry.Register(
                      job.Id,
                      methodInfo,
                      job.Cron,
                       TimeZoneInfo.Utc,
                      //string.IsNullOrEmpty(attribute.TimeZone) ? TimeZoneInfo.Utc : TimeZoneInfo.FindSystemTimeZoneById(attribute.TimeZone),
                      job.Queue ?? EnqueuedState.DefaultQueue);


            context.Response.StatusCode = (int)HttpStatusCode.OK;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

        }
    }
}
