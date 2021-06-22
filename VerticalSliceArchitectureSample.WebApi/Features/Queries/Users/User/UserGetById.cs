using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VerticalSliceArchitectureSample.WebApi.Contexts;
using VerticalSliceArchitectureSample.WebApi.Dependency;
using VerticalSliceArchitectureSample.WebApi.Domain.Users.User;
using VerticalSliceArchitectureSample.WebApi.ExceptionHandler;
using VerticalSliceArchitectureSample.WebApi.Results;
using VerticalSliceArchitectureSample.WebApi.Validation;

namespace VerticalSliceArchitectureSample.WebApi.Queries.Users.User
{
    public static class UserGetById
    {
        public record Query(Guid Id) : IRequest<Response>;

        public class Response : ResultModel
        {
            public Guid Id { get; set; }
            public string Email { get; set; }
        }

        public class ValidatorHandler : IValidationHandler<Query>
        {
            public Task<ResultModel> Validate(Query request)
            {
                //GetById için validation gerekirse buraya yazabiliriz.
                return Task.FromResult(ResultModel.Ok());
            }
        }
        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly SampleQueryContext _dbContext;
            public Handler(SampleQueryContext context)
            {
                _dbContext = context;
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                {

                    UserEntity userEntity = _dbContext.Set<UserEntity>().Where(s => s.Id == request.Id).FirstOrDefault();
                    return await Task.FromResult(new Response
                    {
                        IsSuccess = true,
                        Id = userEntity.Id,
                        Email = userEntity.Email
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
