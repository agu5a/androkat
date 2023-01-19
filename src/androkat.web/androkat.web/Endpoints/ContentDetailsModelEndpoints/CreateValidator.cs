using FastEndpoints;
using FluentValidation;

namespace androkat.web.Endpoints.ContentDetailsModelEndpoints;

public class CreateValidator : Validator<ContentDetailsModelRequest>
{
    public CreateValidator()
    {
        RuleFor(x => x.ContentDetailsModel)
            .NotNull()            
            .WithMessage("Hiányzik az adat");
    }
}