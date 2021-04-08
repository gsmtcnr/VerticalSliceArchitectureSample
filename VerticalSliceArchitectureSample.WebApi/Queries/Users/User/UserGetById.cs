using Corex.Model.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Sample.CQRS.Inftrastructure.Mapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VerticalSliceArchitectureSample.WebApi.Contexts;
using VerticalSliceArchitectureSample.WebApi.Dependency;
using VerticalSliceArchitectureSample.WebApi.ExceptionHandler;
using VerticalSliceArchitectureSample.WebApi.Results;
using VerticalSliceArchitectureSample.WebApi.Validation;
using static VerticalSliceArchitectureSample.WebApi.Domain.Users.User.UserDomain;

namespace VerticalSliceArchitectureSample.WebApi.Queries.Users.User
{
    public static class UserGetById
    {
        public record Query(int Id) : IRequest<Response>;
        public record Response : CQRSResponseData<UserDto>;
      
        public class ValidatorHandler : IValidationHandler<Query>
        {
            public Task<CQRSResponse> Validate(Query request)
            {
                CQRSResponse resultModel = new CQRSResponse();
                Validator validationRules = new Validator();
                FluentValidation.Results.ValidationResult validateResult = validationRules.Validate(request);
                if (!validateResult.IsValid)
                {
                    resultModel.IsSuccess = false;
                    resultModel.Messages.AddRange(validateResult.Errors.Select(v =>
                    new MessageItem
                    {
                        Code = v.ErrorCode,
                        Message = v.ErrorMessage
                    }).ToList());
                }
                return Task.FromResult(resultModel);
            }
        }
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }
        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IDependencyManager _dependencyManager;
            private readonly SampleQueryContext _dbContext;
            public Handler(IDependencyManager ioc, SampleQueryContext context)
            {
                _dependencyManager = ioc;
                _dbContext = context;
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                Response responseModel = new Response();
                ISampleMapper mapper = _dependencyManager.Resolve<ISampleMapper>();
                using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        UserEntity userEntity = _dbContext.Set<UserEntity>().Where(s => s.Id == request.Id).FirstOrDefault();
                        UserDto userDto = mapper.Map<UserEntity, UserDto>(userEntity);
                        responseModel.Data = userDto;
                        responseModel.IsSuccess = userDto != null;
                    }
                    catch (System.Exception ex)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        responseModel.IsSuccess = false;
                        ExceptionManager exceptionManager = new ExceptionManager(ex);
                        responseModel.Messages.AddRange(exceptionManager.GetMessages());
                    }
                }
                return await Task.FromResult(responseModel);
            }
        }
    }
}
