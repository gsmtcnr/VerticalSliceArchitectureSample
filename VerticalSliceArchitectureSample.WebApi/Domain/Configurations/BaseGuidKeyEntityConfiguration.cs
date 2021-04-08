using Corex.Data.Derived.EntityFramework;
using System;

namespace VerticalSliceArchitectureSample.WebApi.Domain
{
    public abstract class BaseGuidKeyEntityConfiguration<TEntity> : BaseEntityConfiguration<TEntity, Guid>
    where TEntity : class, IEntity<Guid>
    {
    }
}
