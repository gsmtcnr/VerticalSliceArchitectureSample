using Corex.Data.Derived.EntityFramework;
using System;

namespace VerticalSliceArchitectureSample.WebApi.Domain
{
    public abstract class BaseIntKeyEntityConfiguration<TEntity> : BaseEntityConfiguration<TEntity, int>
    where TEntity : class, IEntity<int>
    {
    }
}
