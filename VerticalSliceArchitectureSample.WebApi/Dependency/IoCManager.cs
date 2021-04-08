using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VerticalSliceArchitectureSample.WebApi.Dependency
{
    public class IoCManager : IDependencyManager
    {
        private readonly IServiceProvider _serviceProvider;
        public IoCManager(IServiceCollection serviceCollection)
        {
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
        public T Resolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }
        public List<T> ResolveAll<T>()
        {
            return _serviceProvider.GetServices<T>().ToList();
        }
    }
}
