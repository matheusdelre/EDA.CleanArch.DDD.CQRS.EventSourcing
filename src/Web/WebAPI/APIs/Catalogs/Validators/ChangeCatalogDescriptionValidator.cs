using FluentValidation;

namespace WebAPI.APIs.Catalogs.Validators;

public class ChangeCatalogDescriptionValidator : AbstractValidator<Requests.ChangeCatalogDescription>
{
    public ChangeCatalogDescriptionValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.CatalogId)
            .NotEmpty();
    }
}
