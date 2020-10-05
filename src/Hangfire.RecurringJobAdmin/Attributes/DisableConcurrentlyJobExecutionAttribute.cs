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
        public readonly Type _stateReason =  typeof(DeletedState);



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

        public DisableConcurrentlyJobExecutionAttribute(string methodName, Type stateReason = null)
        {
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException(nameof(methodName));
            if (stateReason != null && !typeof(IState).IsAssignableFrom(_stateReason)) throw new ArgumentException($"The type should implement the interface IState");

            if (stateReason != null)
            {
                _stateReason = stateReason;
            }

            _methodName = methodName;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        public DisableConcurrentlyJobExecutionAttribute(string methodName, int from = 0, int count = 2000, Type stateReason = null)
        {
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException(nameof(methodName));
            if (stateReason != null && !typeof(IState).IsAssignableFrom(_stateReason)) throw new ArgumentException($"The type should implement the interface IState");

            if (stateReason != null)
            {
                _stateReason = stateReason;
            }

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
        public DisableConcurrentlyJobExecutionAttribute(string methodName, int from = 0, int count = 2000, string reason = "It is not allowed to perform multiple same tasks.", Type stateReason = null)
        {
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException(nameof(methodName));
            if (stateReason != null && !typeof(IState).IsAssignableFrom(_stateReason)) throw new ArgumentException($"The type should implement the interface IState");
            if (stateReason != null)
            {
                _stateReason = stateReason;
            }

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

                    if (typeof(IState).IsAssignableFrom(_stateReason))
                    {

                        if (_stateReason == typeof(DeletedState))
                        {
                            context.CandidateState = new DeletedState() {  Reason = _reason };
                        }

                        if (_stateReason == typeof(FailedState))
                        {
                           // context.CandidateState = new FailedState(context.Cand) { Reason = _reason };
                        }

                        if (_stateReason == typeof(DeletedState))
                        {
                            context.CandidateState = new DeletedState() { Reason = _reason };
                        }


                    }

                    

                    return;
                }
            }
        }
    }
}
