#nullable enable
using androkat.web.Endpoints.V3Endpoints.Model;
using FastEndpoints;
using FluentValidation;

namespace androkat.web.Endpoints.V3Endpoints;

public class VideoRequestValidator : Validator<VideoRequest>
{
    public VideoRequestValidator()
    {
        RuleFor(x => x.Offset)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(50)
            .WithMessage("Offset hiba");
    }
}