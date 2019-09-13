using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.RecurringJobAdmin.Core
{
    public interface IPeriodicJob
    {
        void Execute(PerformContext context);
    }
}
