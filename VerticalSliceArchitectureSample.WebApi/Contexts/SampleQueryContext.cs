using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitectureSample.WebApi.Dependency;

namespace VerticalSliceArchitectureSample.WebApi.Contexts
{
    public class SampleQueryContext : BaseDBContext<SampleQueryContext>
    {
        public SampleQueryContext(IDependencyManager _dependencyManager, DbContextOptions<SampleQueryContext> option) : base(_dependencyManager, option)
        {
        }
    }
}
