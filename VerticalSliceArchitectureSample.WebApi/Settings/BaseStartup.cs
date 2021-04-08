using Microsoft.Extensions.Configuration;
using System;

namespace VerticalSliceArchitectureSample.WebApi.Settings
{
    public abstract class BaseStartup
    {
        public IConfigurationRoot Configuration { get; }
        protected BaseStartup()
        {
            IConfigurationBuilder builder = SetBuilder();
            this.Configuration = builder.Build();
        }
        private static IConfigurationBuilder SetBuilder()
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfigurationBuilder builder = new ConfigurationBuilder()
                //.AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"Settings/appsettings.{environmentName}.json", true, true);
            return builder;
        }
    }
}
