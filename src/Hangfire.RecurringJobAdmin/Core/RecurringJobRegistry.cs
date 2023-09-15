using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Hangfire.RecurringJobAdmin.Core
{
    /// <summary>
	/// Register <see cref="RecurringJob"/> dynamically.
	/// <see cref="IRecurringJobRegistry"/> interface.
	/// </summary>
	public class RecurringJobRegistry : IRecurringJobRegistry
    {
        static readonly MethodInfo
            addOrUpdateGenericMethod =
                typeof(RecurringJob)
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Single(
                        m => m.Name == nameof(RecurringJob.AddOrUpdate) &&
                             m.GetGenericArguments().Length == 1 &&
                             m.GetParameters()
                                 .Select(i => i.ParameterType)
                                 .SequenceEqual(
                                     new[]
                                     {
                                         typeof(string),
                                         typeof(string),
                                         typeof(Expression<>)
                                             .MakeGenericType(
                                                 typeof(Func<,>)
                                                     .MakeGenericType(m.GetGenericArguments()[0], typeof(Task))),
                                         typeof(string),
                                         typeof(RecurringJobOptions)
                                     }));

        static readonly MethodInfo expressionLambdaGenericMethod =
            typeof(Expression).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(
                    m => m.Name == nameof(Expression.Lambda) &&
                         m.GetGenericArguments().Length == 1 &&
                         m.GetParameters().Select(p => p.ParameterType)
                             .SequenceEqual(new[] { typeof(Expression), typeof(ParameterExpression[]) }));

        /// <summary>
        /// Register RecurringJob via <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="recurringJobId">The identifier of the RecurringJob</param>
        /// <param name="method">the specified method</param>
        /// <param name="cron">Cron expressions</param>
        /// <param name="timeZone"><see cref="TimeZoneInfo"/></param>
        /// <param name="queue">Queue name</param>
        public void Register(string recurringJobId, MethodInfo method, string cron, TimeZoneInfo timeZone, string queue)
        {
            if (recurringJobId == null) throw new ArgumentNullException(nameof(recurringJobId));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (cron == null) throw new ArgumentNullException(nameof(cron));
            if (timeZone == null) throw new ArgumentNullException(nameof(timeZone));
            if (queue == null) throw new ArgumentNullException(nameof(queue));

            var parameters = method.GetParameters();

            Expression[] args = new Expression[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                args[i] = Expression.Default(parameters[i].ParameterType);
            }

            var x = Expression.Parameter(method.DeclaringType, "x");

            var methodCall = method.IsStatic ? Expression.Call(method, args) : Expression.Call(x, method, args);

            var returnType = method.ReturnType;
            var isCallResultGenericTask =
                returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>);
            if (isCallResultGenericTask)
            {
                // Dynamic compilation cannot find RecurringJob.AddOrUpdate<T>(..., Expression<Func<T, Task>>, ...) when
                // the methodCall argument has type Expression<Func<T, Task<TResult>>>, so we construct and invoke
                // the call manually:
                var jobOptions = new RecurringJobOptions { TimeZone = timeZone };
                var declaringType = method.DeclaringType;
                var expressionLambdaMethodReturningNonGenericTask =
                    expressionLambdaGenericMethod.MakeGenericMethod(
                        typeof(Func<,>).MakeGenericType(declaringType, typeof(Task)));
                var lambdaExpression =
                    expressionLambdaMethodReturningNonGenericTask.Invoke(
                        null, new object[] { methodCall, new [] { x } });
                var addOrUpdateMethod = addOrUpdateGenericMethod.MakeGenericMethod(declaringType);
                addOrUpdateMethod.Invoke(null, new [] { recurringJobId, queue, lambdaExpression, cron, jobOptions });
            }
            else
            {
                var addOrUpdate = Expression.Call(
                    typeof(RecurringJob),
                    nameof(RecurringJob.AddOrUpdate),
                    new Type[] { method.DeclaringType },
                    new Expression[]
                    {
                        Expression.Constant(recurringJobId),
                        Expression.Lambda(methodCall, x),
                        Expression.Constant(cron),
                        Expression.Constant(timeZone),
                        Expression.Constant(queue)
                    });
                
                Expression.Lambda(addOrUpdate).Compile().DynamicInvoke();
            }
        }

        ///// <summary>
        ///// Register RecurringJob via <see cref="RecurringJobInfo"/>.
        ///// </summary>
        ///// <param name="recurringJobInfo"><see cref="RecurringJob"/> info.</param>
        //public void Register(RecurringJobInfo recurringJobInfo)
        //{
        //    if (recurringJobInfo == null) throw new ArgumentNullException(nameof(recurringJobInfo));

        //    Register(recurringJobInfo.RecurringJobId, recurringJobInfo.Method, recurringJobInfo.Cron, recurringJobInfo.TimeZone, recurringJobInfo.Queue);

        //    using (var storage = new RecurringJobInfoStorage())
        //    {
        //        storage.SetJobData(recurringJobInfo);
        //    }
        //}
    }
}
