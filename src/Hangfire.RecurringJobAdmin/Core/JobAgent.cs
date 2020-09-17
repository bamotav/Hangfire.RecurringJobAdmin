using Hangfire.Common;
using Hangfire.RecurringJobAdmin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.RecurringJobAdmin.Core
{
    public static class JobAgent
    {
        private const string tagRecurringJob = "recurring-jobs";
        public static void StartBackgroundJob(string JobId)
        {
            using (var connection = JobStorage.Current.GetConnection())
            using (var transaction = connection.CreateWriteTransaction())
            {
                transaction.RemoveFromSet(tagRecurringJob, JobId);
                transaction.AddToSet($"{tagRecurringJob}-stop", JobId);
                transaction.Commit();
            }
        }
        public static void StopBackgroundJob(string JobId)
        {
            using (var connection = JobStorage.Current.GetConnection())
            using (var transaction = connection.CreateWriteTransaction())
            {
                transaction.RemoveFromSet($"{tagRecurringJob}-stop", JobId);
                transaction.AddToSet($"{tagRecurringJob}", JobId);
                transaction.Commit();
            }
        }
    
    }
}
