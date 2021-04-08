using System.Collections.Generic;

namespace VerticalSliceArchitectureSample.WebApi.Dependency
{
    public interface IDependencyManager
    {
        T Resolve<T>();
        List<T> ResolveAll<T>();
    }
}
