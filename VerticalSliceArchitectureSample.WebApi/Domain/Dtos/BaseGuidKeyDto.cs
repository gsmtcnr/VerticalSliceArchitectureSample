using Corex.Model.Infrastructure;
using System;

namespace VerticalSliceArchitectureSample.WebApi.Domain
{
    public abstract class BaseGuidKeyDto: BaseModel<Guid>, IDto<Guid>
    {

    }
}
