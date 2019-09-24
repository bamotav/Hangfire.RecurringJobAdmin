using Hangfire.RecurringJobAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangfire.JobExtensions.DotNetCore.Test
{
    public class TestExecutionJob
    {
        public void TestConsole()
        {
            Console.WriteLine("Testing Console");
        }

        [DisableConcurrentlyJobExecution("CheckFileExists")]
        [RecurringJob("*/2 * * * *", "China Standard Time", "default", RecurringJobId = "Check-File-Exists")]
        public void CheckFileExists()
        {
            Console.WriteLine("Check File Exists");
        }
    }
}
