using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.RecurringJobAdmin.Core;
using Hangfire.RecurringJobAdmin.Pages;
using System;
using System.Reflection;

namespace Hangfire.RecurringJobAdmin
{
    public static class ConfigurationExtensions
    {

        [PublicAPI]
        public static IGlobalConfiguration UseRecurringJobAdmin(this IGlobalConfiguration config, string assembly)
        {
            StorageAssemblySingleton.GetInstance().SetCurrentAssembly(Type.GetType(assembly).Assembly);
            PeriodicJobBuilder.GetAllJobs();
            CreateManagmentJob();
            return config;
        }


        [PublicAPI]
        public static IGlobalConfiguration UseRecurringJobAdmin(this IGlobalConfiguration config, Assembly assembly)
        {
            StorageAssemblySingleton.GetInstance().SetCurrentAssembly(assembly);
            PeriodicJobBuilder.GetAllJobs();
            CreateManagmentJob();
            return config;
        }


        [PublicAPI]
        public static IGlobalConfiguration UseRecurringJobAdmin(this IGlobalConfiguration config)
        {
            CreateManagmentJob();
            return config;
        }

        private static void CreateManagmentJob()
        {
            DashboardRoutes.Routes.AddRazorPage(JobExtensionPage.PageRoute, x => new JobExtensionPage());
            DashboardRoutes.Routes.Add("/JobConfiguration/GetJobs", new GetJobDispatcher());
            DashboardRoutes.Routes.Add("/JobConfiguration/UpdateJobs", new ChangeJobDispatcher());
            DashboardRoutes.Routes.Add("/JobConfiguration/GetJob", new GetJobForEdit());
            DashboardRoutes.Routes.Add("/JobConfiguration/JobAgent", new JobAgentDispatcher());



            NavigationMenu.Items.Add(page => new MenuItem(JobExtensionPage.Title, page.Url.To("/JobConfiguration"))
            {
                Active = page.RequestPath.StartsWith(JobExtensionPage.PageRoute),
                Metric = DashboardMetrics.RecurringJobCount
            });

            AddDashboardRouteToEmbeddedResource("/JobConfiguration/js/page", "application/js", "Hangfire.RecurringJobAdmin.Dashboard.Content.js.jobextension.js");
            AddDashboardRouteToEmbeddedResource("/JobConfiguration/js/vue", "application/js", "Hangfire.RecurringJobAdmin.Dashboard.Content.js.vue.js");
            AddDashboardRouteToEmbeddedResource("/JobConfiguration/js/axio", "application/js", "Hangfire.RecurringJobAdmin.Dashboard.Content.js.axios.min.js");
            AddDashboardRouteToEmbeddedResource("/JobConfiguration/js/sweetalert", "application/js", "Hangfire.RecurringJobAdmin.Dashboard.Content.js.sweetalert.js");
            AddDashboardRouteToEmbeddedResource("/JobConfiguration/css/jobExtension", "text/css", "Hangfire.RecurringJobAdmin.Dashboard.Content.css.JobExtension.css");

        }

        private static void AddDashboardRouteToEmbeddedResource(string route, string contentType, string resourceName)
           => DashboardRoutes.Routes.Add(route, new ContentDispatcher(contentType, resourceName, TimeSpan.FromDays(1)));
    }


}
