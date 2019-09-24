using Hangfire.Common;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.RecurringJobAdmin.Attributes
{
    public class DisableConcurrentlyJobExecution : JobFilterAttribute, IElectStateFilter
    {
        public DisableConcurrentlyJobExecution() { }

        
        public readonly int _from = 0;
        public readonly int _count = 2000;
        public readonly string _reason = "It is not allowed to perform multiple same tasks.";

        private string _methodName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        public DisableConcurrentlyJobExecution(string methodName)
        {
            _methodName = methodName;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        public DisableConcurrentlyJobExecution(string methodName, int from = 0, int count = 2000)
        {
            _from = from;
            _count = count;
            _methodName = methodName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="reason"></param>
        public DisableConcurrentlyJobExecution(string methodName, int from = 0, int count = 2000, string reason = "It is not allowed to perform multiple same tasks.")
        {
            _from = from;
            _count = count;
            _methodName = methodName;

            _reason = reason;
        }


        public void OnStateElection(ElectStateContext context)
        {
            if (string.IsNullOrWhiteSpace(_methodName))
                _methodName = context.BackgroundJob.Job.Method.Name;

            var processingJobs = context.Storage.GetMonitoringApi().ProcessingJobs(_from, _count);

            foreach (var processingJob in processingJobs)
            {
                if (processingJob.Value.Job.Method.Name.Equals(_methodName, StringComparison.InvariantCultureIgnoreCase) && !context.CandidateState.IsFinal)
                {
                    context.CandidateState = new DeletedState
                    {
                        Reason = _reason
                    };

                    return;
                }
            }
        }
    }
}
