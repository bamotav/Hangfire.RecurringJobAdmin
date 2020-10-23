using Hangfire.Dashboard;
using Hangfire.RecurringJobAdmin.Core;

namespace Hangfire.RecurringJobAdmin
{
    public static class TagDashboardMetrics
    {
        public static readonly DashboardMetric JobsStoppedCount = new DashboardMetric("JobsStopped:count", razorPage =>
            {
                return new Metric(JobAgent.GetAllJobStopped().Count);
            });
    }
}
