using Corex.Model.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VerticalSliceArchitectureSample.WebApi.Contexts;
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
        public record Response : CQRSResponse
        {
            public Guid Id { get; set; }
        }

        public class Validator : IValidationHandler<Command>
        {
            private readonly SampleQueryContext _dbContext;
            public Validator(SampleQueryContext dbContext) => _dbContext = dbContext;
            public Task<CQRSResponse> Validate(Command request)
            {
                if (_dbContext.Set<UserEntity>().Any(v => v.Email == request.InputModel.Email))
                {
                    return Task.FromResult(CQRSResponse.Error(new MessageItem
                    {
                        Code = "Duplicate_Email",
                        Message = "There is a registered mail."
                    }));

                }
                return Task.FromResult(CQRSResponse.Ok());
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
                Response resultObjectModel = new Response();
                using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        IResultObjectModel<UserEntity> createUserResult = UserEntity.Create(request.InputModel.Email, request.InputModel.Password);
                        if (!createUserResult.IsSuccess)
                            resultObjectModel.Messages.AddRange(createUserResult.Messages);
                        else
                        {
                            EntityEntry<UserEntity> insertItem = _dbContext.Set<UserEntity>().Add(createUserResult.Data);
                            await _dbContext.SaveChangesAsync(cancellationToken);
                            await transaction.CommitAsync(cancellationToken);
                            resultObjectModel.Id = insertItem.Entity.Id;
                        }
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

        }
    }
}
