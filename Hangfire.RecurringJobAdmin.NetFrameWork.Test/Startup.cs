
using Hangfire;
using Hangfire.Common;
using Hangfire.RecurringJobAdmin;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartup(typeof(Hangfire.JobExtensions.NetFrameWork.Test.Startup))]

namespace Hangfire.JobExtensions.NetFrameWork.Test
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("BSCoreDbHangfire")
                .UseRecurringJobAdmin(typeof(Startup).Assembly);

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            var manager = new RecurringJobManager();

            manager.AddOrUpdate("ReadTransactionJob", Job.FromExpression(() => Console.WriteLine("")), "*/5 * * * *");
            manager.AddOrUpdate("WriteTransactionJob", Job.FromExpression(() => Console.WriteLine("")), "*/8 * * * *");
        }
    }
}
