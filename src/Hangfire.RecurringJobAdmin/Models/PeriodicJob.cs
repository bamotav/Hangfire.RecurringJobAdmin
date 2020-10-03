using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Hangfire.RecurringJobAdmin.Models
{

    /// <summary>
    /// It is used to build <see cref="RecurringJob"/> 
    ///// with <see cref="IRecurringJobBuilder.Build(Func{System.Collections.Generic.IEnumerable{RecurringJobInfo}})"/>.
    /// </summary>
    public class PeriodicJob
    {
        public string Id { get; set; }
        public string Cron { get; set; }
        public string Queue { get; set; }

        public string Class { get; set; }

        public string Method { get; set; }

        public string JobState { get; set; }

        public string NextExecution { get; set; }
        public string LastJobId { get; set; }
        public string LastJobState { get; set; }
        public string LastExecution { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool Removed { get; set; }
        public string TimeZoneId { get; set; }
        public string Error { get; set; }
    }

}
