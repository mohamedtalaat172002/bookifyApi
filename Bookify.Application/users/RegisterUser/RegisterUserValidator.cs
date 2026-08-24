using FluentValidation;

namespace Bookify.Application.users.RegisterUser
{
    internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {

        public RegisterUserCommandValidator()
        {
            RuleFor(f => f.FirstName).NotEmpty();
            RuleFor(f => f.LastName).NotEmpty();
            RuleFor(f => f.Email).EmailAddress();
            RuleFor(f => f.Password).MinimumLength(5);



        }
    }
}
