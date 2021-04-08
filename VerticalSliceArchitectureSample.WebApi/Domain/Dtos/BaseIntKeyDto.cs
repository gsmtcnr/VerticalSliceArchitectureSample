using Corex.Model.Infrastructure;

namespace VerticalSliceArchitectureSample.WebApi.Domain
{
    public abstract class BaseIntKeyDto : BaseModel<int>, IDto<int>
    {
    }
}
