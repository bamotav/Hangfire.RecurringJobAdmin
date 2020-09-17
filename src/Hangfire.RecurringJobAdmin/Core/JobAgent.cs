using Hangfire.Common;
using Hangfire.RecurringJobAdmin.Models;
using Hangfire.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangfire.RecurringJobAdmin.Core
{
    public static class JobAgent
    {
        private const string tagRecurringJob = "recurring-jobs";
        private const string tagStopJob = "recurring-jobs-stop";
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
                transaction.RemoveFromSet(tagStopJob, JobId);
                transaction.AddToSet($"{tagRecurringJob}", JobId);
                transaction.Commit();
            }
        }

        public static List<PeriodicJob> GetAllJobStopped()
        {
            var outPut = new List<PeriodicJob>();
            using (var connection = JobStorage.Current.GetConnection())
            {
                var allJobStopped = connection.GetAllItemsFromSet(tagStopJob);

                allJobStopped.ToList().ForEach(jobId =>
                {
                    var dto = new PeriodicJob();

                    var dataJob = connection.GetAllEntriesFromHash($"{tagRecurringJob}:{jobId}");


                    try
                    {
                        if (dataJob.TryGetValue("Job", out var payload) && !String.IsNullOrWhiteSpace(payload))
                        {
                            var invocationData = InvocationData.DeserializePayload(payload);
                            var job = invocationData.DeserializeJob();
                            dto.Method = job.Method.Name;
                            dto.Class = job.Method.ReflectedType.FullName;
                        }
                    }
                    catch (JobLoadException ex)
                    {
                        dto.Error = ex.Message;
                    }


                    if (dataJob.ContainsKey("NextExecution"))
                    {
                        dto.NextExecution = JobHelper.DeserializeNullableDateTime(dataJob["NextExecution"]);
                    }

                    if (dataJob.ContainsKey("LastJobId") && !string.IsNullOrWhiteSpace(dataJob["LastJobId"]))
                    {
                        dto.LastJobId = dataJob["LastJobId"];

                        var stateData = connection.GetStateData(dto.LastJobId);
                        if (stateData != null)
                        {
                            dto.LastJobState = stateData.Name;
                        }
                    }

                    if (dataJob.ContainsKey("Queue"))
                    {
                        dto.Queue = dataJob["Queue"];
                    }

                    if (dataJob.ContainsKey("LastExecution"))
                    {
                        dto.LastExecution = JobHelper.DeserializeNullableDateTime(dataJob["LastExecution"]);
                    }

                    if (dataJob.ContainsKey("TimeZoneId"))
                    {
                        dto.TimeZoneId = dataJob["TimeZoneId"];
                    }

                    if (dataJob.ContainsKey("CreatedAt"))
                    {
                        dto.CreatedAt = JobHelper.DeserializeNullableDateTime(dataJob["CreatedAt"]);
                    }

                    if (dataJob.TryGetValue("Error", out var error) && !String.IsNullOrEmpty(error))
                    {
                        dto.Error = error;
                    }

                    dto.Removed = false;
                    dto.JobState = "Stopped";

                    outPut.Add(dto);

                });
            }
            return outPut;
        }

        public static bool IsValidJobId(string JobId)
        {
            var result = false;
            using (var connection = JobStorage.Current.GetConnection())
            {
                result = connection.GetAllEntriesFromHash(JobId) != null;
            }
            return result;
        }

    }
}
