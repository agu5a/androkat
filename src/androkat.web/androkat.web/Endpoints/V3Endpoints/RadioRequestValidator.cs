#nullable enable
using androkat.web.Endpoints.V3Endpoints.Model;
using FastEndpoints;
using FluentValidation;

namespace androkat.web.Endpoints.V3Endpoints;

public class RadiosRequestValidator : Validator<RadioRequest>
{
    public RadiosRequestValidator()
    {
        RuleFor(x => x.Radio)
            .NotEmpty()
            .WithMessage("Radio hiba");

        RuleFor(x => x.Radio)
            .Must(radio => !string.IsNullOrEmpty(radio) && radio.ToLower() is "katolikushu" or "mariaradio" or "szentistvan")
            .WithMessage("Radio hiba");
    }
}