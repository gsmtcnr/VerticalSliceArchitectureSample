using Corex.Model.Infrastructure;

namespace VerticalSliceArchitectureSample.WebApi.Results
{
    public class ResultObjectModel<TData> : BaseResultObjectModel<TData>, IResultObjectModel<TData>
         where TData : class, new()
    { 
        public ResultObjectModel(TData data) : base(data)
        {
            IsSuccess = true;
        }
    }
}
