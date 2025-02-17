using FluentValidation;

namespace WebAPI.APIs.Catalogs.Validators;

public class CreateCatalogValidator : AbstractValidator<Requests.CreateCatalog>
{
    public CreateCatalogValidator()
    {
        RuleFor(request => request.Description)
            .NotNull()
            .NotEmpty();

        RuleFor(request => request.Title)
            .NotNull()
            .NotEmpty();

        RuleFor(request => request.CatalogId)
            .NotNull()
            .NotEmpty();
    }
}