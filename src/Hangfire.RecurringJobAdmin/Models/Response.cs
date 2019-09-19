using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.RecurringJobAdmin.Models
{
    public class Response
    {
        public bool Status { get; set; }

        public object Object { get; set; }

        public string Message { get; set; }
    }
}
