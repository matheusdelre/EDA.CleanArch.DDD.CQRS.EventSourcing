using FluentValidation;

namespace WebAPI.APIs.Identities.Validators;

public class ChangeEmailValidator : AbstractValidator<Requests.ChangeEmail>
{
    public ChangeEmailValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty();

        RuleFor(request => request.Email)
            .EmailAddress();
    }
}