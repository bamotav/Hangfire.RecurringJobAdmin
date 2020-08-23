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

        [DisableConcurrentlyJobExecution("CheckFileExists", 0, 10, "It is not allowed to perform multiple same tasks.")]
        [RecurringJob("*/2 * * * *", "Eastern Standard Time", "default", RecurringJobId = "Check-File-Exists")]
        public void CheckFileExists()
        {
            Console.WriteLine("Check File Exists");
        }
    }
}
