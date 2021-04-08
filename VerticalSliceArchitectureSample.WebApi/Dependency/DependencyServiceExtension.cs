using Corex.Model.Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VerticalSliceArchitectureSample.WebApi.Behaviours;
using VerticalSliceArchitectureSample.WebApi.Contexts;
using VerticalSliceArchitectureSample.WebApi.Validation;

namespace VerticalSliceArchitectureSample.WebApi.Dependency
{
    public static class DependencyServiceExtension
    {

        public static void AddDependencies(this IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            RegisterIoCManager(services);
            RegisterAllDependencies(services);
            RegisterMediatR(services);
            RegisterContext(services, configurationRoot);
        }
        #region Private Methods
        private static void RegisterContext(IServiceCollection services, IConfigurationRoot configurationRoot)
        {
            services.AddContexts(configurationRoot);
        }

        private static void RegisterMediatR(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddValidators();
            //Behaviour sırası önemli. Verilecek sıraya göre davranış gösterecektir.
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        }
        private static void RegisterIoCManager(IServiceCollection services)
        {
            services.AddSingleton<IDependencyManager>(x =>
            ActivatorUtilities.CreateInstance<IoCManager>(x, services));
        }
        private static void RegisterAllDependencies(IServiceCollection services)
        {
            services.Scan(scan => scan
                     .FromApplicationDependencies()
                     .AddClasses(classes => classes.AssignableTo<ITransientDependecy>()).AsImplementedInterfaces().WithTransientLifetime()
                     .AddClasses(classes => classes.AssignableTo<IScopedDependency>()).AsImplementedInterfaces().WithScopedLifetime()
                     .AddClasses(classes => classes.AssignableTo<ISingletonDependecy>()).AsImplementedInterfaces().WithSingletonLifetime()
                 );
        }
        #endregion
    }
}
