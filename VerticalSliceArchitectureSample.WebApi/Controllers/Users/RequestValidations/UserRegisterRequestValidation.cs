using FluentValidation;
using VerticalSliceArchitectureSample.WebApi.Commands.Users.User;

namespace VerticalSliceArchitectureSample.WebApi.Controllers.Users.RequestValidations
{
    public class UserRegisterRequestValidation : AbstractValidator<UserRegister.Command>
    {
        public UserRegisterRequestValidation()
        {
            RuleFor(m => m.InputModel.Email).MinimumLength(6).MaximumLength(32).NotNull().EmailAddress();
            RuleFor(m => m.InputModel.Password).MinimumLength(8).MaximumLength(64).NotNull();
        }
    }
}
