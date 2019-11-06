using Hangfire.Common;
using Hangfire.RecurringJobAdmin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.RecurringJobAdmin.Core
{
    public static class JobAgent
    {
        public static bool StartBackgroundJob(JobItem jobItem)
        {
            try
            {
                if (string.IsNullOrEmpty(jobItem.Data)) return true;
                using (var connection = JobStorage.Current.GetConnection())
                {
                    var hashKey = Utility.MD5(jobItem.JobName + ".runtime");
                    using (var tran = connection.CreateWriteTransaction())
                    {
                        tran.SetRangeInHash(hashKey, new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("Data", jobItem.Data)
                        });
                        tran.Commit();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
               // Logger.ErrorException("HttpJobDispatcher.StartBackgroudJob", ex);
                return false;
            }
        }
        public static bool StopBackgroundJob(JobItem jobItem)
        {
            try
            {
                using (var connection = JobStorage.Current.GetConnection())
                {
                    var hashKey = Utility.MD5(jobItem.JobName + ".runtime");
                    using (var tran = connection.CreateWriteTransaction())
                    {
                        tran.SetRangeInHash(hashKey, new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("Action", "stop") });
                        tran.Commit();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
               // Logger.ErrorException("HttpJobDispatcher.StopBackgroudJob", ex);
                return false;
            }
        }
    
    }
}
