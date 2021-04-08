using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VerticalSliceArchitectureSample.WebApi.Commands.Users.User;
using VerticalSliceArchitectureSample.WebApi.Queries.Users.User;
using static VerticalSliceArchitectureSample.WebApi.Domain.Users.User.UserDomain;

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
        public Task<UserRegister.Response> Register(RegisterInputModel registerInputModel)
        {
            UserRegister.Command request = new UserRegister.Command(registerInputModel);
            return _mediator.Send(request);
        }
        [HttpGet]
        public Task<UserGetById.Response> GetById(int id)
        {
            UserGetById.Query request = new UserGetById.Query(id);
            return _mediator.Send(request);
        }
    }
}
