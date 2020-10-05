using Hangfire.Common;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.RecurringJobAdmin
{
    public class DisableConcurrentlyJobExecutionAttribute : JobFilterAttribute, IElectStateFilter
    {
        public DisableConcurrentlyJobExecutionAttribute() { }


        public readonly int _from = 0;
        public readonly int _count = 2000;
        public readonly string _reason = "It is not allowed to perform multiple same tasks.";
        public readonly JobState _jobState = JobState.DeletedState;
        private string _methodName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        public DisableConcurrentlyJobExecutionAttribute(string methodName)
        {
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException(nameof(methodName));

            _methodName = methodName;

        }

        public DisableConcurrentlyJobExecutionAttribute(string methodName, JobState jobState = JobState.DeletedState)
        {
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException(nameof(methodName));

            _jobState = jobState;
            _methodName = methodName;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        public DisableConcurrentlyJobExecutionAttribute(string methodName, int from = 0, int count = 2000, JobState jobState = JobState.DeletedState)
        {
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException(nameof(methodName));
            _jobState = jobState;
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
        public DisableConcurrentlyJobExecutionAttribute(string methodName, int from = 0, int count = 2000, string reason = "It is not allowed to perform multiple same tasks.", JobState jobState = JobState.DeletedState)
        {
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException(nameof(methodName));

            _jobState = jobState;
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
                    //When job is throw an exception the State alway should be Failed state.
                    if (context.CandidateState is FailedState)
                    {
                        context.CandidateState = new FailedState(((FailedState)context.CandidateState).Exception) { Reason = _reason };
                        return;
                    }

                    switch (_jobState)
                    {
                        case JobState.DeletedState:

                            if (!(context.CandidateState is FailedState))
                            {
                                context.CandidateState = new DeletedState() { Reason = _reason };
                            }

                            break;
                        case JobState.FailedState:

                            if (!(context.CandidateState is FailedState))
                            {
                                context.CandidateState = new FailedState(new Exception(_reason)) { Reason = _reason };
                            }

                            break;
                        case JobState.EnqueuedState:

                            if (!(context.CandidateState is FailedState))
                            {
                                context.CandidateState = new EnqueuedState() { Reason = _reason };
                            }
                            break;
                        default:
                            break;
                    }

                    //The others state is comming soon.
                    return;
                }
            }
        }
    }
}
