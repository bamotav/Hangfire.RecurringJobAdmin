using Hangfire.Annotations;
using System;

namespace Hangfire.JobExtensions
{
    public static class ConfigurationExtensions
    {
        [PublicAPI]
        public static IGlobalConfiguration UseJobExtension(this IGlobalConfiguration config, string connectionString)
        {
            
            return config;
        }
    }
}
