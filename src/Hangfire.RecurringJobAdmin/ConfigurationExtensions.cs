using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.RecurringJobAdmin.Core;
using Hangfire.RecurringJobAdmin.Pages;
using System;
using System.Linq;
using System.Reflection;

namespace Hangfire.RecurringJobAdmin
{
    public static class ConfigurationExtensions
    {


        /// <param name="includeReferences">If is true it will load all dlls references of the current project to find all jobs.</param>
        /// <param name="assemblies"></param>
        [PublicAPI]
        public static IGlobalConfiguration UseRecurringJobAdmin(this IGlobalConfiguration config, [NotNull] params string[] assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            StorageAssemblySingleton.GetInstance().SetCurrentAssembly(assemblies: assemblies.Select(x => Type.GetType(x).Assembly).ToArray());
            PeriodicJobBuilder.GetAllJobs();
            CreateManagmentJob();
            return config;
        }

        /// <param name="includeReferences">If is true it will load all dlls references of the current project to find all jobs.</param>
        /// <param name="assemblies"></param>
        [PublicAPI]
        public static IGlobalConfiguration UseRecurringJobAdmin(this IGlobalConfiguration config, bool includeReferences = false, [NotNull] params string[] assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            StorageAssemblySingleton.GetInstance().SetCurrentAssembly(includeReferences, assemblies.Select(x => Type.GetType(x).Assembly).ToArray());
            PeriodicJobBuilder.GetAllJobs();
            CreateManagmentJob();
            return config;
        }



        /// <param name="includeReferences">If is true it will load all dlls references of the current project to find all jobs.</param>
        /// <param name="assemblies"></param>
        [PublicAPI]
        public static IGlobalConfiguration UseRecurringJobAdmin(this IGlobalConfiguration config, [NotNull] params Assembly[] assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            StorageAssemblySingleton.GetInstance().SetCurrentAssembly(assemblies: assemblies);
            PeriodicJobBuilder.GetAllJobs();
            CreateManagmentJob();
            return config;
        }

        /// <param name="includeReferences">If is true it will load all dlls references of the current project to find all jobs.</param>
        /// <param name="assemblies"></param>
        [PublicAPI]
        public static IGlobalConfiguration UseRecurringJobAdmin(this IGlobalConfiguration config, bool includeReferences = false, [NotNull] params Assembly[] assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            StorageAssemblySingleton.GetInstance().SetCurrentAssembly(includeReferences, assemblies);
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
