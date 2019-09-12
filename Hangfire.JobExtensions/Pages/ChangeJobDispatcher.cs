using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.Storage;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.JobExtensions.Pages
{
    internal sealed class ChangeJobDispatcher : IDashboardDispatcher
    {
        private readonly IStorageConnection _connection;
        public ChangeJobDispatcher()
        {

            _connection = JobStorage.Current.GetConnection();
        }


        public async Task Dispatch([NotNull] DashboardContext context)
        {
            var recurringJobIds = _connection.GetRecurringJobs();

            var firstJob = recurringJobIds[0];

            var method = recurringJobIds[0].Job.Method;

            var parameters = method.GetParameters();

            Expression[] args = new Expression[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                args[i] = Expression.Default(parameters[i].ParameterType);
            }

            var x = Expression.Parameter(method.DeclaringType, "x");

            var methodCall = method.IsStatic ? Expression.Call(method, args) : Expression.Call(x, method, args);

            // var x = Expression.Parameter(recurringJobIds[0].Job.Method.DeclaringType, "x");
            //   var a = Expression.Lambda(Expression.Call(recurringJobIds[0].Job.Method, recurringJobIds[0].Job.Args), x)


            //var x = Expression.Parameter(Expression.Call(recurringJobIds[0].Job.Method.DeclaringType, "x"));

            // var methodCall = method.IsStatic ? Expression.Call(method, args) : Expression.Call(x, method, args);

            //RecurringJob.AddOrUpdate(recurringJobIds[0].Id,, "", null, recurringJobIds[0].Queue);

            var recurring = recurringJobIds[0];
            //var a = firstJob.Job.Method.Invoke(new object(), new object[] { 100 }));

            //RecurringJob.AddOrUpdate(firstJob.Id,Job.FromExpression(()=>Console.WriteLine("")), "", null, "");

            RecurringJob.AddOrUpdate(recurring.Id, () => ReflectionHelper.InvokeVoidMethod("Hangfire.JobExtensions.DotNetCore.Test.TestExecution", "Testing"), "*/1 * * * *", null, recurring.Queue);

            //var addOrUpdate = Expression.Call(
            //    typeof(RecurringJob),
            //    nameof(RecurringJob.AddOrUpdate),
            //    new Type[] { method.DeclaringType },
            //    new Expression[]
            //    {
            //        Expression.Constant(recurring.Id),
            //        Expression.Lambda(methodCall, x),
            //        Expression.Constant("*/1 * * * *"),
            //        null,
            //        Expression.Constant(recurring.Queue)
            //    });

            //Expression.Lambda(addOrUpdate).Compile().DynamicInvoke();

            //_connection.CreateWriteTransaction().
        }
    }
}
