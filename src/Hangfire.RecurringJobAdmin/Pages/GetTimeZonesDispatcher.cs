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
    internal sealed class GetTimeZonesDispatcher : Dashboard.IDashboardDispatcher
    {
        public async Task Dispatch([NotNull] Dashboard.DashboardContext context)
        {
            await context.Response.WriteAsync(JsonConvert.SerializeObject(Utility.GetTimeZones()));
        }
    }
}
