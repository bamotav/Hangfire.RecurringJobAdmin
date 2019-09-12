using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.JobExtensions.Pages;
using System;

namespace Hangfire.JobExtensions
{
    public static class ConfigurationExtensions
    {
        [PublicAPI]
        public static IGlobalConfiguration UseJobExtension(this IGlobalConfiguration config)
        {
            DashboardRoutes.Routes.AddRazorPage(JobExtensionPage.PageRoute, x => new JobExtensionPage());
            DashboardRoutes.Routes.Add("/JobConfiguration/post", new GetJobDispatcher());

            NavigationMenu.Items.Add(page => new MenuItem(JobExtensionPage.Title, "JobConfiguration")
            {
                Active = page.RequestPath.StartsWith(JobExtensionPage.PageRoute)
            });

            AddDashboardRouteToEmbeddedResource(
             "/JobConfiguration/js/page",
             "application/js",
             "Hangfire.JobExtensions.Dashboard.Content.jobextension.js");

            return config;
        }

        private static void AddDashboardRouteToEmbeddedResource(string route, string contentType, string resourceName)
           => DashboardRoutes.Routes.Add(route, new ContentDispatcher(contentType, resourceName, TimeSpan.FromDays(1)));
    }
}
