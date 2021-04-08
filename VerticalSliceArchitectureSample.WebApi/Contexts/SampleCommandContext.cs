using Microsoft.EntityFrameworkCore;
using VerticalSliceArchitectureSample.WebApi.Dependency;

namespace VerticalSliceArchitectureSample.WebApi.Contexts
{
    public class SampleCommandContext : BaseDBContext<SampleCommandContext>
    {
        public SampleCommandContext(IDependencyManager _dependencyManager, DbContextOptions<SampleCommandContext> option) : base(_dependencyManager, option)
        {
        }
    }
}
