using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VerticalSliceArchitectureSample.WebApi.Commands.Users.User;
using VerticalSliceArchitectureSample.WebApi.Domain.Users.User.Dtos;
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
        public Task<UserRegister.Response> Register(UserRegisterInputModel registerInputModel)
        {
            UserRegister.Command request = new UserRegister.Command(registerInputModel);
            return _mediator.Send(request);
        }
        [HttpGet]
        public Task<UserGetById.Response> GetById(Guid id)
        {
            UserGetById.Query request = new UserGetById.Query(id);
            return _mediator.Send(request);
        }
    }
}
