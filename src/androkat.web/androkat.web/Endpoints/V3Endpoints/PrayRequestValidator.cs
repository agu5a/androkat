#nullable enable
using androkat.web.Endpoints.V3Endpoints.Model;
using FastEndpoints;
using FluentValidation;
using System;
using System.Globalization;

namespace androkat.web.Endpoints.V3Endpoints;

public class PrayRequestValidator : Validator<PrayRequest>
{
    public PrayRequestValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Dátum hiba");

        RuleFor(x => x.Date)
            .Must(dateString => DateTime.TryParse(dateString, CultureInfo.CreateSpecificCulture("hu-HU"), out _))
            .WithMessage("Dátum hiba");
    }
}
