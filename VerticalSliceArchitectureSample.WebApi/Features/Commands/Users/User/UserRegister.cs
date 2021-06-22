using Corex.Model.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VerticalSliceArchitectureSample.WebApi.Contexts;
using VerticalSliceArchitectureSample.WebApi.Controllers.Users.RequestValidations;
using VerticalSliceArchitectureSample.WebApi.Domain.Users.User;
using VerticalSliceArchitectureSample.WebApi.Domain.Users.User.Dtos;
using VerticalSliceArchitectureSample.WebApi.ExceptionHandler;
using VerticalSliceArchitectureSample.WebApi.Results;
using VerticalSliceArchitectureSample.WebApi.Validation;

namespace VerticalSliceArchitectureSample.WebApi.Commands.Users.User
{
    public static class UserRegister
    {
        public record Command(UserRegisterInputModel InputModel) : IRequest<Response>;
        public class Response : ResultModel
        {
            public Guid Id { get; set; }
        }

        public class Validator : IValidationHandler<Command>
        {
            private readonly SampleQueryContext _dbContext;
            public Validator(SampleQueryContext dbContext) => _dbContext = dbContext;
            public Task<ResultModel> Validate(Command request)
            {
                //Request Validation..
                UserRegisterRequestValidation validationRules = new UserRegisterRequestValidation();
                FluentValidation.Results.ValidationResult validateResult = validationRules.Validate(request);
                if (!validateResult.IsValid)
                {
                    return Task.FromResult(ResultModel.Error(validateResult.Errors.Select(v =>
                    new MessageItem
                    {
                        Code = v.ErrorCode,
                        Message = v.ErrorMessage
                    }).ToList()));
                }

                //Business Validation..
                if (_dbContext.Set<UserEntity>().Any(v => v.Email == request.InputModel.Email))
                {
                    return Task.FromResult(ResultModel.Error(new MessageItem
                    {
                        Code = "Duplicate_Email",
                        Message = "There is a registered mail."
                    }));

                }
                return Task.FromResult(ResultModel.Ok());
            }
        }
        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly SampleCommandContext _dbContext;
            public Handler(SampleCommandContext sampleCommandContext)
            {
                _dbContext = sampleCommandContext;
            }
            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    IResultObjectModel<UserEntity> createUserResult = UserEntity.Create(request.InputModel.Email, request.InputModel.Password);
                    if (!createUserResult.IsSuccess)
                    {
                        return await Task.FromResult((Response)ResultModel.Error(createUserResult.Messages));
                    }

                    EntityEntry<UserEntity> insertItem = _dbContext.Set<UserEntity>().Add(createUserResult.Data);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return await Task.FromResult( new Response
                    {
                          IsSuccess=true,
                          Id= insertItem.Entity.Id
                    });
                }
                catch (System.Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    ExceptionManager exceptionManager = new ExceptionManager(ex);
                    return await Task.FromResult((Response)ResultModel.Error(exceptionManager.GetMessages()));
                }
            }
        }
    }
}
