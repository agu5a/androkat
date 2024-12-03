#nullable enable
using androkat.infrastructure.Model;
using FastEndpoints;
using FluentValidation;
using System;

namespace androkat.web.Endpoints.ContentDetailsModelEndpoints;

public class ContentRequestValidator : Validator<ContentRequest>
{
    public ContentRequestValidator()
    {
        RuleFor(x => x.Tipus)
            .NotNull()
            .LessThan(100)
            .GreaterThan(0)
            .WithMessage("Rossz típus");

        RuleFor(x => x.Id)
           .Must(BeAValidGuid)
           .When(x => !string.IsNullOrWhiteSpace(x.Id))
           .WithMessage("Rossz azonosító");

    }

    private static bool BeAValidGuid(string? input)
    {
        return Guid.TryParse(input, out _);
    }
}