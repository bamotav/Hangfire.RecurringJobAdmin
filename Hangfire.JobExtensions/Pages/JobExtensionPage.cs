using Hangfire.Dashboard;
using Hangfire.Dashboard.Pages;
using Hangfire.JobExtensions.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobExtensions.Pages
{
    internal sealed class JobExtensionPage : RazorPage
    {
        public const string Title = "Job Configuration";
        public const string PageRoute = "/JobConfiguration";

        private static readonly string PageHtml;

        static JobExtensionPage()
        {
            PageHtml = Utility.ReadStringResource("Hangfire.JobExtensions.Dashboard.JobExtension.html");
        }

        public override void Execute()
        {
            WriteEmptyLine();
            Layout = new LayoutPage(Title);
            WriteLiteralLine(PageHtml);
            WriteEmptyLine();
        }

        private void WriteLiteralLine(string textToAppend)
        {
            WriteLiteral(textToAppend);
           // WriteConfig();
            WriteLiteral("\r\n");
        }

        private void WriteEmptyLine()
        {
            WriteLiteral("\r\n");
        }

    }
}
