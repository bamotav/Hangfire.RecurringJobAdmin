using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.JobExtensions.Pages
{
    internal sealed class GetJobDispatcher : IDashboardDispatcher
    {
        public async Task Dispatch([NotNull] DashboardContext context)
        {
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
