using Hangfire.Dashboard;
using Hangfire.Dashboard.Pages;
using Hangfire.RecurringJobAdmin.Core;

namespace Hangfire.RecurringJobAdmin.Pages
{
    internal sealed class JobsStoppedPage : PageBase
    {
        public const string Title = "Stopped Jobs";
        public const string PageRoute = "/jobs/stopped";

        private static readonly string PageHtml;

        static JobsStoppedPage()
        {
            PageHtml = Utility.ReadStringResource("Hangfire.RecurringJobAdmin.Dashboard.JobsStopped.html");
        }

        public override void Execute()
        {
            WriteEmptyLine();
            Layout = new LayoutPage(Title);
            Write(Html.JobsSidebar());
            WriteLiteralLine(PageHtml);
            WriteEmptyLine();
        }
    }
}
