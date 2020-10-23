using Hangfire.Dashboard;
using Hangfire.Dashboard.Pages;
using Hangfire.RecurringJobAdmin.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.RecurringJobAdmin.Pages
{
    abstract internal class PageBase : RazorPage
    {
        public override void Execute() { }

        protected void WriteLiteralLine(string textToAppend)
        {
            WriteLiteral(textToAppend);
            WriteLiteral("\r\n");
        }

        protected void WriteEmptyLine()
        {
            WriteLiteral("\r\n");
        }
    }
}
