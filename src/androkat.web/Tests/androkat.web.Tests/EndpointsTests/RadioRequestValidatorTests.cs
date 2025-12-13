using androkat.web.Endpoints.V3Endpoints;
using androkat.web.Endpoints.V3Endpoints.Model;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace androkat.web.Tests.EndpointsTests;

public class RadioRequestValidatorTests
{
    private readonly RadiosRequestValidator _validator;

    public RadioRequestValidatorTests()
    {
        _validator = new RadiosRequestValidator();
    }

    [Theory]
    [InlineData("katolikushu")]
    [InlineData("mariaradio")]
    [InlineData("szentistvan")]
    [InlineData("KATOLIKUSHU")]
    [InlineData("MariaRadio")]
    [InlineData("SzentIstvan")]
    public void Radio_WithValidValue_ShouldNotHaveValidationError(string radio)
    {
        var model = new RadioRequest { Radio = radio };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Radio);
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid")]
    [InlineData("other")]
    [InlineData("katolikus")]
    [InlineData("maria")]
    public void Radio_WithInvalidValue_ShouldHaveValidationError(string radio)
    {
        var model = new RadioRequest { Radio = radio };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Radio);
    }

    [Fact]
    public void Radio_WithNullValue_ShouldHaveValidationError()
    {
        var model = new RadioRequest { Radio = null };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Radio);
    }

    [Fact]
    public void FullModel_WithValidData_ShouldBeValid()
    {
        var model = new RadioRequest { Radio = "katolikushu" };
        var result = _validator.TestValidate(model);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void FullModel_WithInvalidData_ShouldHaveError()
    {
        var model = new RadioRequest { Radio = "invalid" };
        var result = _validator.TestValidate(model);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThan(0);
    }
}
