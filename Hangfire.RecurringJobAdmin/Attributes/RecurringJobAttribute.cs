using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.RecurringJobAdmin
{
    /// <summary>
	/// Attribute to add or update <see cref="RecurringJob"/> automatically
	/// by target it to interface/instance/static method.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
    public class RecurringJobAttribute : Attribute
    {
        /// <summary>
        /// The identifier of the RecurringJob
        /// </summary>
        public string RecurringJobId { get; set; }
        /// <summary>
        /// Cron expressions
        /// </summary>
        public string Cron { get; set; }
        /// <summary>
        /// Queue name
        /// </summary>
        public string Queue { get; set; }
        /// <summary>
        /// Converts to <see cref="TimeZoneInfo"/> via method <seealso cref="TimeZoneInfo.FindSystemTimeZoneById(string)"/>,
        /// default value is <see cref="TimeZoneInfo.Utc"/>
        /// </summary>
        public string TimeZone { get; set; }
        /// <summary>
        /// Whether to build RecurringJob automatically, default value is true.
        /// If false it will be deleted automatically.
        /// </summary>
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// Initializes a new instance of the <see cref="RecurringJobAttribute"/>
        /// </summary>
        /// <param name="cron">Cron expressions</param>
        public RecurringJobAttribute(string cron) : this(cron, EnqueuedState.DefaultQueue) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="RecurringJobAttribute"/>
        /// </summary>
        /// <param name="cron">Cron expressions</param>
        /// <param name="queue">Queue name</param>
        public RecurringJobAttribute(string cron, string queue) : this(cron, "UTC", queue) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="RecurringJobAttribute"/>
        /// </summary>
        /// <param name="cron">Cron expressions</param>
        /// <param name="timeZone">Converts to <see cref="TimeZoneInfo"/> via method <seealso cref="TimeZoneInfo.FindSystemTimeZoneById(string)"/>.</param>
        /// <param name="queue">Queue name</param>
        public RecurringJobAttribute(string cron, string timeZone, string queue)
        {
            if (string.IsNullOrEmpty(cron)) throw new ArgumentNullException(nameof(cron));
            if (string.IsNullOrEmpty(timeZone)) throw new ArgumentNullException(nameof(timeZone));
            if (string.IsNullOrEmpty(queue)) throw new ArgumentNullException(nameof(queue));

            Cron = cron;
            TimeZone = timeZone;
            Queue = queue;
        }
    }
}
