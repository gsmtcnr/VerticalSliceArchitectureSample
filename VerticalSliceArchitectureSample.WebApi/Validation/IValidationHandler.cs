using System.Threading.Tasks;
using VerticalSliceArchitectureSample.WebApi.Results;

namespace VerticalSliceArchitectureSample.WebApi.Validation
{
    public interface IValidationHandler 
    { 
    }
    public interface IValidationHandler<T> : IValidationHandler
    {
        Task<ResultModel> Validate(T request);
    }
}
