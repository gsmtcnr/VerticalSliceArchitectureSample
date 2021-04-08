using Corex.Model.Infrastructure;

namespace VerticalSliceArchitectureSample.WebApi.Domain
{
    public abstract class BaseIntKeyEntity : BaseModel<int>, IEntity<int>
    {
    }
}
