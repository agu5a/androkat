using androkat.web.Endpoints.V3Endpoints;
using androkat.web.Endpoints.V3Endpoints.Model;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace androkat.web.Tests.EndpointsTests;

public class PrayRequestValidatorTests
{
    private readonly PrayRequestValidator _validator;

    public PrayRequestValidatorTests()
    {
        _validator = new PrayRequestValidator();
    }

    [Theory]
    [InlineData("2025-12-13")]
    [InlineData("2024-01-01")]
    [InlineData("2023.06.15")]
    [InlineData("2025/03/25")]
    public void Date_WithValidValue_ShouldNotHaveValidationError(string date)
    {
        var model = new PrayRequest { Date = date };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Date);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-a-date")]
    [InlineData("2025-13-01")] // Invalid month
    [InlineData("2025-12-32")] // Invalid day
    [InlineData("invalid")]
    [InlineData("12345")]
    public void Date_WithInvalidValue_ShouldHaveValidationError(string date)
    {
        var model = new PrayRequest { Date = date };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Date);
    }

    [Fact]
    public void Date_WithNullValue_ShouldHaveValidationError()
    {
        var model = new PrayRequest { Date = null };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Date);
    }

    [Fact]
    public void FullModel_WithValidData_ShouldBeValid()
    {
        var model = new PrayRequest { Date = "2025-12-13" };
        var result = _validator.TestValidate(model);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void FullModel_WithInvalidData_ShouldHaveError()
    {
        var model = new PrayRequest { Date = "invalid-date" };
        var result = _validator.TestValidate(model);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThan(0);
    }
}
