using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VerticalSliceArchitectureSample.WebApi.Commands.Users.User;
using VerticalSliceArchitectureSample.WebApi.Queries.Users.User;

namespace VerticalSliceArchitectureSample.WebApi.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public Task<RegisterUser.Response> Register(RegisterUser.Command registerCommand)
        {
            return _mediator.Send(registerCommand);
        }
        [HttpGet]
        public Task<GetUserById.Response> GetById(Guid id)
        {
            GetUserById.Query request = new GetUserById.Query(id);
            return _mediator.Send(request);
        }
    }
}
