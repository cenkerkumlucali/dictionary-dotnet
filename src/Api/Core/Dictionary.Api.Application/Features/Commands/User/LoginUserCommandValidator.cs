using Common.Models.Queries;
using Common.Models.RequestModels;
using FluentValidation;

namespace Dictionary.Api.Application.Features.Commands.User;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(c => c.Email).NotNull()
            .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
            .WithMessage("{ProperyName} not a valid email address");
        RuleFor(c => c.Password).NotNull().MinimumLength(6).WithMessage("{PropertyName} should at least be {MinLenght} characters");
    }
}