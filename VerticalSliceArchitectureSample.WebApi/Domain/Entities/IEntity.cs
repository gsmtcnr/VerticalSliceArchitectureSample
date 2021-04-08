using Corex.Model.Derived.EntityModel;

namespace VerticalSliceArchitectureSample.WebApi.Domain
{
    public interface IEntity<TKey> : IEntityModel<TKey>
    {
    }
}
