using FluentValidation;
using VerticalSliceArchitectureSample.WebApi.Commands.Users.User;

namespace VerticalSliceArchitectureSample.WebApi.Controllers.Users.RequestValidations
{
    public class UserRegisterRequestValidation : AbstractValidator<RegisterUser.Command>
    {
        public UserRegisterRequestValidation()
        {
            RuleFor(m => m.Email).MinimumLength(6).MaximumLength(32).NotNull().EmailAddress();
            RuleFor(m => m.Password).MinimumLength(8).MaximumLength(64).NotNull();
        }
    }
}
