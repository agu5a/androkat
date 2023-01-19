using FastEndpoints;
using FluentValidation;

namespace androkat.web.Endpoints.RadioMusorModelEndpoints;

public class UpdateValidator : Validator<RadioMusorModelRequest>
{
    public UpdateValidator()
    {
        RuleFor(x => x.Source)
            .NotEmpty()
            .WithMessage("Hiányzik az adat");

        RuleFor(x => x.Musor)
            .NotEmpty()
            .WithMessage("Hiányzik az adat");

        RuleFor(x => x.Inserted)
            .NotEmpty()
            .WithMessage("Hiányzik az adat");
    }
}
