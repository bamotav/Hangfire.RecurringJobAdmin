using Hangfire.RecurringJobAdmin;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.Sample.Library
{
    public class AppointmentSmsNotificationJob
    {
        [RecurringJob("*/2 * * * *", "SA Western Standard Time", "default", RecurringJobId = "Run")]
        public void Run()
        {
            Console.WriteLine("Check File Exists");
        }

        [RecurringJob("*/1 * * * *", "SA Western Standard Time", "default", RecurringJobId = "RunDelayJob")]
        [DisableConcurrentlyJobExecution(nameof(RunDelayJob))]
        public async Task RunDelayJob()
        {
            var id = Guid.NewGuid();
            Console.WriteLine(id);
            CancellationTokenSource source = new CancellationTokenSource();
            await Task.Delay(180000, source.Token);
        }

        [RecurringJob("*/1 * * * *", "SA Western Standard Time", "default", RecurringJobId = "ThisService")]
        [DisableConcurrentlyJobExecution(nameof(DoThis))]
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task DoThis()
        {
            throw new Exception();
        }


    }
}
