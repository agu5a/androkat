using androkat.infrastructure.Model;
using androkat.web.Endpoints.ContentDetailsModelEndpoints;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace androkat.web.Tests.EndpointsTests;

public class ContentRequestValidatorTests
{
    private readonly ContentRequestValidator _validator;

    public ContentRequestValidatorTests()
    {
        _validator = new ContentRequestValidator();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(99)]
    public void Tipus_WithValidValue_ShouldNotHaveValidationError(int tipus)
    {
        var model = new ContentRequest { Tipus = tipus };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Tipus);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100)]
    [InlineData(101)]
    public void Tipus_WithInvalidValue_ShouldHaveValidationError(int tipus)
    {
        var model = new ContentRequest { Tipus = tipus };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Tipus);
    }

    [Fact]
    public void Id_WithValidGuid_ShouldNotHaveValidationError()
    {
        var model = new ContentRequest { Tipus = 1, Id = "281cd115-1289-11ea-8aa1-cbeb38570c35" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Id_WithEmptyString_ShouldNotHaveValidationError()
    {
        var model = new ContentRequest { Tipus = 1, Id = "" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Id_WithNullValue_ShouldNotHaveValidationError()
    {
        var model = new ContentRequest { Tipus = 1, Id = null };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Theory]
    [InlineData("invalid-guid")]
    [InlineData("x281cd115128911ea8aa1cbeb38570c35")]
    [InlineData("281cd115-1289-11ea-8aa1-cbeb38570c3")] // missing one char
    [InlineData("not-a-guid-at-all")]
    public void Id_WithInvalidGuid_ShouldHaveValidationError(string id)
    {
        var model = new ContentRequest { Tipus = 1, Id = id };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void FullModel_WithValidData_ShouldBeValid()
    {
        var model = new ContentRequest
        {
            Tipus = 10,
            Id = "a81cd115-1289-11ea-8aa1-cbeb38570c35"
        };
        var result = _validator.TestValidate(model);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void FullModel_WithInvalidTipusAndId_ShouldHaveMultipleErrors()
    {
        var model = new ContentRequest
        {
            Tipus = 0,
            Id = "invalid"
        };
        var result = _validator.TestValidate(model);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThan(1);
    }
}
