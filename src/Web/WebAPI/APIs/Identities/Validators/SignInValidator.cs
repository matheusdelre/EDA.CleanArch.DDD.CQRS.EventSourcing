using FluentValidation;

namespace WebAPI.APIs.Identities.Validators;

public class SignInValidator : AbstractValidator<Requests.SignIn>
{
    public SignInValidator()
    {
        RuleFor(request => request.Email)
            .EmailAddress();

        RuleFor(request => request.Password)
            .NotNull()
            .NotEmpty();
    }
}