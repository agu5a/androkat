using androkat.domain.Configuration;
using androkat.web.Service;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace androkat.web.Tests.Services;

public class ApiKeyValidatorTests
{
    [Fact]
    public void IsValid_WithMatchingApiKey_ReturnsTrue()
    {
        var credentials = Options.Create(new CredentialConfiguration
        {
            CronApiKey = "valid-api-key-123"
        });

        var validator = new ApiKeyValidator(credentials);

        var result = validator.IsValid("valid-api-key-123");

        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithNonMatchingApiKey_ReturnsFalse()
    {
        var credentials = Options.Create(new CredentialConfiguration
        {
            CronApiKey = "valid-api-key-123"
        });

        var validator = new ApiKeyValidator(credentials);

        var result = validator.IsValid("wrong-api-key");

        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptyApiKey_ReturnsFalse()
    {
        var credentials = Options.Create(new CredentialConfiguration
        {
            CronApiKey = "valid-api-key-123"
        });

        var validator = new ApiKeyValidator(credentials);

        var result = validator.IsValid("");

        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithNullApiKey_ReturnsFalse()
    {
        var credentials = Options.Create(new CredentialConfiguration
        {
            CronApiKey = "valid-api-key-123"
        });

        var validator = new ApiKeyValidator(credentials);

        var result = validator.IsValid(null);

        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptyConfiguredKey_AndEmptyInput_ReturnsTrue()
    {
        var credentials = Options.Create(new CredentialConfiguration
        {
            CronApiKey = ""
        });

        var validator = new ApiKeyValidator(credentials);

        var result = validator.IsValid("");

        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_IsCaseSensitive()
    {
        var credentials = Options.Create(new CredentialConfiguration
        {
            CronApiKey = "ApiKey123"
        });

        var validator = new ApiKeyValidator(credentials);

        var result = validator.IsValid("apikey123");

        result.Should().BeFalse();
    }
}
