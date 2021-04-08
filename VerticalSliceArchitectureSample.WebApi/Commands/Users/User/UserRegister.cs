using Corex.Model.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

namespace VerticalSliceArchitectureSample.WebApi.Commands.Users.User
{
    public static class UserRegister
    {
        public record Command(RegisterInputModel InputModel) : IRequest<Response>;
        public record Response : CQRSResponse
        {
            public int Id { get; set; }
        }
        public class ValidatorHandler : IValidationHandler<Command>
        {
            public Task<CQRSResponse> Validate(Command request)
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
                return Task.FromResult<CQRSResponse>(resultModel);
            }
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.InputModel.Email).MinimumLength(6).MaximumLength(32).NotNull().EmailAddress();
                RuleFor(m => m.InputModel.Password).MinimumLength(8).MaximumLength(64).NotNull();
            }
        }
        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly IDependencyManager _dependencyManager;
            private readonly SampleCommandContext _dbContext;
            public Handler(IDependencyManager ioc, SampleCommandContext sampleCommandContext)
            {
                _dependencyManager = ioc;
                _dbContext = sampleCommandContext;
            }
            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                Response resultObjectModel = new Response();
                ISampleMapper mapper = _dependencyManager.Resolve<ISampleMapper>();
                UserEntity userEntity = CreateUserEntity(request, mapper);
                using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        EntityEntry<UserEntity> insertItem = _dbContext.Set<UserEntity>().Add(userEntity);
                        await _dbContext.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);
                        resultObjectModel.Id = insertItem.Entity.Id;
                    }
                    catch (System.Exception ex)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        resultObjectModel.IsSuccess = false;
                        ExceptionManager exceptionManager = new ExceptionManager(ex);
                        resultObjectModel.Messages.AddRange(exceptionManager.GetMessages());
                    }
                }

                return await Task.FromResult(resultObjectModel);
            }
            private static UserEntity CreateUserEntity(Command request, ISampleMapper mapper)
            {
                UserDto dto = new UserDto
                {
                    Email = request.InputModel.Email,
                    Password = request.InputModel.Password
                };
                UserEntity userEntity = mapper.Map<UserDto, UserEntity>(dto);
                return userEntity;
            }
        }
    }
}
