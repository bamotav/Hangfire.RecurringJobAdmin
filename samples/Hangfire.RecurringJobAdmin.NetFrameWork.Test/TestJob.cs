using Hangfire.RecurringJobAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hangfire.JobExtensions.NetFrameWork.Test
{
    public class TestJob
    {
        public void Testing()
        {
            Console.WriteLine("Desde otro proyecto");
        }

        [RecurringJob("*/2 * * * *", "China Standard Time", "default", RecurringJobId = "Prueba-desde-atributo")]
        public void OtroTesting()
        {
            Console.WriteLine("Desde atributo");
        }
    }
}