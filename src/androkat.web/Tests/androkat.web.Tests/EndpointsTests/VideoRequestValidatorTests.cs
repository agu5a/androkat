using androkat.web.Endpoints.V3Endpoints;
using androkat.web.Endpoints.V3Endpoints.Model;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace androkat.web.Tests.EndpointsTests;

public class VideoRequestValidatorTests
{
    private readonly VideoRequestValidator _validator;

    public VideoRequestValidatorTests()
    {
        _validator = new VideoRequestValidator();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(25)]
    [InlineData(49)]
    [InlineData(50)]
    public void Offset_WithValidValue_ShouldNotHaveValidationError(int offset)
    {
        var model = new VideoRequest { Offset = offset };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Offset);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    [InlineData(51)]
    [InlineData(100)]
    public void Offset_WithInvalidValue_ShouldHaveValidationError(int offset)
    {
        var model = new VideoRequest { Offset = offset };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Offset);
    }

    [Fact]
    public void FullModel_WithValidData_ShouldBeValid()
    {
        var model = new VideoRequest { Offset = 10 };
        var result = _validator.TestValidate(model);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void FullModel_WithInvalidData_ShouldHaveError()
    {
        var model = new VideoRequest { Offset = -5 };
        var result = _validator.TestValidate(model);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThan(0);
    }
}
