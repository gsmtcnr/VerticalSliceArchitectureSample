using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VerticalSliceArchitectureSample.WebApi.Results;
using VerticalSliceArchitectureSample.WebApi.Validation;

namespace VerticalSliceArchitectureSample.WebApi.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : ResultModel, new()
    {
        private readonly IValidationHandler<TRequest> _validationHandler;
        public ValidationBehaviour(IValidationHandler<TRequest> validationHandler)
        {
            _validationHandler = validationHandler;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            ResultModel result = await _validationHandler.Validate(request);
            if (!result.IsSuccess)
                return new TResponse
                {
                    IsSuccess = false,
                    Messages = result.Messages
                };
            return await next();
        }
    }
}
