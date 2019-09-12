using Hangfire.Annotations;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.JobExtensions.Core;
using Hangfire.Storage;
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
            var recurringJobIds = _connection.GetRecurringJobs();//_connection.GetAllItemsFromSet("recurring-jobs");

          
            if (!"POST".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.StatusCode = 405;
                return;
            }

            try
            {
                var testJob = context.GetBackgroundJobClient();
                var testJob1 = context.GetRecurringJobManager();

            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
