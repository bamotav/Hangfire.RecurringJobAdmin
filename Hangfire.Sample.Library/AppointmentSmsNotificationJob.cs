using Hangfire.RecurringJobAdmin;
using System;

namespace Hangfire.Sample.Library
{
    public class AppointmentSmsNotificationJob
    {
        [RecurringJob("*/2 * * * *", "UTC", "default", RecurringJobId = "Run")]
        public void Run()
        {
            Console.WriteLine("Check File Exists");
        }
    }
}
