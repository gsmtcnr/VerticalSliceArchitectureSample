using Microsoft.Extensions.DependencyInjection;

namespace VerticalSliceArchitectureSample.WebApi.Validation
{
    public static class ValidationExtensions
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromApplicationDependencies()
                .AddClasses(classes => classes.AssignableTo<IValidationHandler>())
                  .AsImplementedInterfaces()
                  .WithTransientLifetime());
        }
    }
}
