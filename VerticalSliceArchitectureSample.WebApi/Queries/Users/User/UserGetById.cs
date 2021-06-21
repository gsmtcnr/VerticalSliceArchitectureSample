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

        public record Response : CQRSResponse
        {
            public Guid Id { get; set; }
            public string Email { get; set; }
        }

        public class ValidatorHandler : IValidationHandler<Query>
        {
            public Task<CQRSResponse> Validate(Query request)
            {
                CQRSResponse resultModel = new CQRSResponse();
                //GetById için validation gerekirse buraya yazabiliriz.
                return Task.FromResult(resultModel);
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
                Response responseModel = new Response();
                using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
                {
                    try
                    {
                        UserEntity userEntity = _dbContext.Set<UserEntity>().Where(s => s.Id == request.Id).FirstOrDefault();
                        responseModel.Email = userEntity.Email;
                        responseModel.Id = userEntity.Id;
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
