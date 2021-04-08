using Corex.Model.Infrastructure;
using System;

namespace VerticalSliceArchitectureSample.WebApi.Domain
{
    public abstract class BaseGuidKeyEntity : BaseModel<Guid>, IEntity<Guid>
    {
    }
}
