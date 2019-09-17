using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Hangfire.RecurringJobAdmin
{
    /// <summary>
    /// Register <see cref="RecurringJob"/> dynamically.
    /// </summary>
    public interface IRecurringJobRegistry
    {
      
        /// <summary>
        /// Register RecurringJob via <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="recurringJobId">The identifier of the RecurringJob</param>
        /// <param name="method">the specified method</param>
        /// <param name="cron">Cron expressions</param>
        /// <param name="timeZone"><see cref="TimeZoneInfo"/></param>
        /// <param name="queue">Queue name</param>
        void Register(string recurringJobId, MethodInfo method, string cron, TimeZoneInfo timeZone, string queue);
   
    }
}
