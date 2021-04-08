using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VerticalSliceArchitectureSample.WebApi.Contexts
{
    public static class DBContextServiceExtension
    {
        public static void AddContexts(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddDbContext<SampleCommandContext>(options => options.UseSqlServer(configuration["ConnectionStrings:CommandDbSQL"]));
            services.AddDbContext<SampleQueryContext>(options => options.UseSqlServer(configuration["ConnectionStrings:QueryDbSQL"]));
        }
    }
}
