# AGENTS.md - AndroKat Web Application

## Project Overview

**AndroKat** is a .NET web application (version 2.133.0) serving religious content including daily readings, prayers, videos, and books. The application is built with ASP.NET Core and uses FastEndpoints for API routing.

### Tech Stack

- **.NET 10.0** (net10.0)
- **FastEndpoints** - Minimal API alternative to controllers
- **Entity Framework Core 10.0** with SQLite
- **Autofac** - Dependency Injection container
- **Serilog** - Structured logging with Logz.io integration
- **Google Authentication** - OAuth integration
- **Rate Limiting** - IP-based request throttling
- **ETag/HTTP Caching** - Efficient bandwidth management

---

## Architecture & Project Structure

This is a **Clean Architecture** solution with four main projects:

### 1. `androkat.domain` (Domain Layer)

- **Pure domain models** with no external dependencies
- Contains: Configuration, Enums, Models, Interfaces (IRepository patterns)
- Models organized in subdirectories:
  - `Model/` - Core domain entities (AudioModel, ContentModel, ImaModel, VideoModel, etc.)
  - `Model/WebResponse/` - API response DTOs
  - `Model/ContentCache/` - Cache-specific models (MainCache, BookRadioSysCache, VideoCache, ImaCache)
  - `Configuration/` - Configuration POCOs
  - `Enum/` - Enumerations (CacheKey, Forras)

### 2. `androkat.application` (Application Layer)

- **Business logic and service interfaces**
- Contains: Services, Interfaces, DI configuration
- Key services:
  - `ApiService` - Core business logic for content retrieval
  - `ApiServiceCacheDecorate` - Decorator pattern for caching with ETag generation
  - `CacheService` - Cache population and management
  - `ContentService`, `WarmupService`, `CronService`
- **Decorator Pattern**: `ApiServiceCacheDecorate` wraps `ApiService` with caching using Autofac

### 3. `androkat.infrastructure` (Infrastructure Layer)

- **Data access and external integrations**
- Entity Framework Core with SQLite (`androkat.db`)
- Contains: DataManager, Mapper, Configuration, Model
- Repository implementations for domain interfaces

### 4. `androkat.web` (Presentation Layer)

- **ASP.NET Core Web Application**
- FastEndpoints for API routing
- Razor Pages for web UI
- Contains: Endpoints, Controllers, Pages, ViewModels, Services, Infrastructure

---

## Coding Standards & Conventions

### General Principles

1. **Warnings as Errors**: `TreatWarningsAsErrors` is enabled - all warnings must be fixed
2. **SonarAnalyzer**: Static code analysis is enforced
3. **Code Coverage**: Test coverage is important (Tests/ directory exists)
4. **File-Scoped Namespaces**: Use modern C# file-scoped namespace syntax
5. **Nullable Reference Types**: Enabled project-wide

### Naming Conventions

- **Interfaces**: Prefix with `I` (e.g., `IApiService`, `ICacheRepository`)
- **Private Fields**: Use `_camelCase` with underscore prefix
- **Methods**: Use PascalCase with descriptive names
- **Async Methods**: Suffix with `Async` (e.g., `HandleAsync`)
- **Hungarian Notation**: Avoid except for interfaces

### Code Style

```csharp
// ✅ Good - File-scoped namespace
namespace androkat.application.Service;

public class ApiService : IApiService
{
    private readonly IClock _iClock;

    public ApiService(IClock iClock)
    {
        _iClock = iClock;
    }

    // Use expression-bodied members when appropriate
    public IReadOnlyCollection<VideoResponse> GetVideos(int offset, VideoCache cache) =>
        cache.Video.OrderByDescending(o => o.Date).Skip(offset).Take(5).ToList();
}
```

### Dependency Injection

- **Autofac** is the DI container (not built-in .NET DI)
- Register services in `DefaultCoreModule` (application layer)
- Use constructor injection exclusively
- Lifetime scopes:
  - `InstancePerLifetimeScope()` - Similar to Scoped
  - `SingleInstance()` - Singleton
- **Decorator Pattern**: Use `builder.RegisterDecorator<TDecorator, TService>()` for cross-cutting concerns

### Configuration Management

- **Environment Variables Only** - No appsettings.json for secrets
- Prefix: `ANDROKAT_*` for all environment variables
- Categories:
  - `ANDROKAT_ENDPOINT_*` - API URLs
  - `ANDROKAT_CREDENTIAL_*` - API keys, tokens
  - `ANDROKAT_GENERAL_*` - General settings
  - `ANDROKAT_GOOGLE_*` - OAuth credentials
- Use `IOptions<T>` pattern with validation:

  ```csharp
  builder.Services.AddOptions<EndPointConfiguration>()
      .Configure(options => { /* bind from env vars */ })
      .ValidateDataAnnotations()
      .ValidateOnStart();
  ```

---

## Caching Strategy

### Multi-Level Caching Architecture

1. **MemoryCache** (`IMemoryCache`) - ASP.NET Core in-memory cache
2. **Cache Decorator Pattern** - `ApiServiceCacheDecorate` wraps business logic
3. **Cache Models** - Dedicated cache POCOs in `androkat.domain.Model.ContentCache`
4. **Sliding Expiration** - 30 minutes default

### Cache Keys (Enum-based)

```csharp
// From androkat.domain.Enum.CacheKey
ContentResponseCacheKey
EgyebOlvasnivaloResponseCacheKey
VideoResponseCacheKey
ImaResponseCacheKey
MainCacheKey
BookRadioSysCacheKey
VideoCacheKey
ImaCacheKey
```

### ETag Implementation

- **ETags are generated and cached alongside response data**
- ETag format: MD5 hash of JSON serialized response wrapped in quotes (`"hash"`)
- Cache key pattern: `{baseKey}_etag`
- Generated in `ApiServiceCacheDecorate.GenerateETag<T>()`
- Flow:
  1. First request: Generate response → Serialize → MD5 → Cache both response & ETag
  2. Subsequent requests: Check `If-None-Match` header → Return 304 if match
  3. ETag stored with same lifetime as response (30 min sliding)

```csharp
// ETag generation pattern
private static string GenerateETag<T>(T content)
{
    var json = JsonSerializer.Serialize(content);
    var hash = MD5.HashData(Encoding.UTF8.GetBytes(json));
    return $"\"{Convert.ToHexString(hash).ToLowerInvariant()}\"";
}
```

---

## FastEndpoints Usage

### Endpoint Structure

```csharp
public class Contents : Endpoint<ContentRequest, IEnumerable<ContentResponse>>
{
    private readonly IApiService _apiService;
    private readonly ILogger<Contents> _logger;

    public Contents(IApiService apiService, ILogger<Contents> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public override void Configure()
    {
        Get("/v3/contents"); // Route from environment variable
        AllowAnonymous();
        Options(x => x.RequireRateLimiting("fixed-by-ip"));
    }

    public override async Task HandleAsync(ContentRequest request, CancellationToken ct)
    {
        // Implementation with ETag support
        var response = _apiService.GetContentByTipusAndId(request.Tipus, guid, default, default);
        var etag = _apiService.GetETag(request.Tipus, guid);
        
        // Check If-None-Match for 304 response
        if (!string.IsNullOrEmpty(clientETag) && clientETag == etag)
        {
            HttpContext.Response.Headers.ETag = etag;
            await Send.ResponseAsync(null, StatusCodes.Status304NotModified, ct);
            return;
        }
        
        // Return 200 with ETag
        HttpContext.Response.Headers.ETag = etag;
        await Send.ResponseAsync(response, StatusCodes.Status200OK, ct);
    }
}
```

### Rate Limiting

- Policy: `"fixed-by-ip"` - 10 requests per 10 seconds per IP
- Honors `X-Forwarded-For` header (behind nginx proxy)
- Returns 429 status on limit exceeded

---

## Logging & Monitoring

### Serilog Configuration

- **Dual Sink**: Console + Logz.io (remote aggregation)
- **Structured Logging**: Use template parameters, not string interpolation
- **Log Levels**: Information by default, enriched with context
- **Bootstrap Logger**: Creates early logger before DI container

```csharp
// ✅ Good - Structured logging
_logger.LogInformation("{Name} was called", nameof(MainCacheFillUp));
_logger.LogError(ex, "Exception: {Name}", nameof(HandleAsync));

// ❌ Bad - String interpolation
_logger.LogInformation($"{nameof(MainCacheFillUp)} was called");
```

### Health Checks

- Entity Framework Core health check enabled
- Custom health check endpoint via `SetHealthCheckEndpoint()`

---

## Security

### Headers

- **NetEscapades.AspNetCore.SecurityHeaders** - Security header middleware
- Disabled: `AddServerHeader = false` (hides Kestrel version)
- CORS: Restricted to `https://androkat.hu`

### Authentication

- **Google OAuth** - Configured via environment variables
- **API Key** - Custom `ApiKeyAuthorizationFilter` for admin endpoints
- Cookie-based session management

### Authorization Attributes

- Custom attributes in `androkat.web/Attributes/`
- `ApiKeyAuthorizationFilter` validates API keys for protected endpoints

---

## Database

### Entity Framework Core with SQLite

- **Database**: `androkat.db` (copied to output directory)
- **Migrations**: EF Core migrations in infrastructure project
- **Health Checks**: `AddDbContextCheck<TContext>()` for monitoring

### Repository Pattern

- Interfaces defined in `androkat.domain` (e.g., `IAdminRepository`, `IApiRepository`, `ICacheRepository`)
- Implementations in `androkat.infrastructure.DataManager`

---

## Testing

### Test Project Structure

```
Tests/
├── androkat.application.Tests/
├── androkat.infrastructure.Tests/
└── androkat.web.Tests/
    └── EndpointsTests/
        └── ContentsTests.cs
```

### Testing Conventions

- Use **xUnit** framework
- Use **FluentAssertions** for readable assertions
- Use **Ardalis.HttpClientTestExtensions** for HTTP testing
- Test factories: `ContentsWebApplicationFactory<IWebMarker>`
- Collection fixtures for shared setup

### Example Test Pattern

```csharp
[Collection("SharedWebAppCollection")]
public class ContentsTests : IClassFixture<ContentsWebApplicationFactory<IWebMarker>>
{
    [Fact]
    public async Task Test01_API_GetContents_Returns_ETag_Header()
    {
        // Arrange
        var response = await _client.GetAsync($"{_url}?tipus={tipus}&id=");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.Should().Contain(h => h.Key == "ETag");
    }
}
```

---

## Common Patterns

### 1. Decorator Pattern (Caching)

```csharp
// Registration in Autofac
builder.RegisterType<ApiService>().As<IApiService>();
builder.RegisterDecorator<ApiServiceCacheDecorate, IApiService>();

// Decorator adds caching behavior transparently
public class ApiServiceCacheDecorate : IApiService
{
    private readonly IApiService _apiService;
    private readonly IMemoryCache _memoryCache;

    public IReadOnlyCollection<ContentResponse> GetContent(int tipus, Guid id, ...)
    {
        var key = CacheKey.ContentResponseCacheKey + "_" + tipus + "_" + id;
        var cached = GetCache<IReadOnlyCollection<ContentResponse>>(key);
        if (cached is not null) return cached;

        var result = _apiService.GetContent(tipus, id, ...);
        _memoryCache.Set(key, result, TimeSpan.FromMinutes(30));
        
        // Also cache ETag
        var etagKey = key + "_etag";
        var etag = GenerateETag(result);
        _memoryCache.Set(etagKey, etag, TimeSpan.FromMinutes(30));
        
        return result;
    }
}
```

### 2. Options Pattern with Environment Variables

```csharp
public class EndPointConfiguration
{
    public string GetContentsApiUrl { get; set; }
    // ... other properties
}

// In DependencyInjection.cs
builder.Services.AddOptions<EndPointConfiguration>()
    .Configure(options =>
    {
        options.GetContentsApiUrl = Environment.GetEnvironmentVariable("ANDROKAT_ENDPOINT_GET_CONTENTS_API_URL");
    })
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Inject with IOptions<T>
public class Contents
{
    public Contents(IOptions<EndPointConfiguration> config)
    {
        _route = config.Value.GetContentsApiUrl;
    }
}
```

### 3. Read-Only Collections

- Use `IReadOnlyCollection<T>` for return types
- Use `.AsReadOnly()` to wrap lists
- Prevents external modification of collections

---

## Build & Deployment

### Build Configuration

- **Single File Publish**: `PublishSingleFile=true`
- **Satellite Languages**: English only (`SatelliteResourceLanguages=en`)
- **Post-Publish**: Removes compressed files (.br, .gz)
- **Version**: Centrally managed in `.csproj` (currently 2.133.0)

### Ignored Warnings

- `IDE0290`, `IDE0028` - Code style preferences
- `NU1903`, `NU1904` - NuGet package warnings
- `S6967` - SonarQube specific rule

### Static Assets

- **Cache-Control**: 7 days for static files
- **Custom MIME types**: `.epub` → `application/epub+zip`
- **Data files**: `IdezetData.json`, `androkat.db` copied to output

---

## API Endpoints

### Content Endpoint Pattern

```
GET /v3/contents?tipus={int}&id={guid}
```

**Parameters:**

- `tipus` - Content type ID (from `Forras` enum)
- `id` - Optional GUID, empty for first item

**Response Headers:**

- `ETag` - MD5 hash of response content
- `Cache-Control: private, max-age=300` (5 minutes)

**Behavior:**

- Returns collection of `ContentResponse`
- Supports 304 Not Modified via `If-None-Match`
- Rate limited: 10 req/10s per IP

### Special Content Types

- **Book (Forras.book = 11)** - EPUB files
- **Humor (tipus = 58)** - Today's humor content
- **Ajandek (tipus = 25)** - Gift/daily special content
- **Blog/News** - `AndrokatConfiguration.BlogNewsContentTypeIds()`

---

## When Contributing Code

### Before Creating New Code

1. **Check existing patterns** - Use decorator for cross-cutting concerns
2. **Use environment variables** - Never hardcode secrets
3. **Add logging** - Use structured Serilog logging
4. **Write tests** - Add to appropriate `*.Tests` project
5. **Consider caching** - High-traffic endpoints should be cached
6. **Handle ETags** - New endpoints returning collections should support ETags

### Code Review Checklist

- [ ] No warnings or errors (warnings are errors)
- [ ] Follows Clean Architecture layers
- [ ] Uses dependency injection (Autofac)
- [ ] Has appropriate logging
- [ ] Has unit/integration tests
- [ ] Environment variables documented
- [ ] Caching strategy considered
- [ ] ETag support for collection endpoints
- [ ] Rate limiting configured
- [ ] Security headers appropriate

---

## Common Pitfalls to Avoid

1. **❌ Don't bypass the cache decorator**
   - Always inject `IApiService` (not `ApiService` directly)
   - Let Autofac resolve the decorated instance

2. **❌ Don't generate ETags on every request**
   - Cache ETags alongside response data
   - Use same cache key pattern with `_etag` suffix

3. **❌ Don't use `Guid.Empty` for cache keys**
   - Empty GUID means "first item" in business logic
   - Include in cache key: `CacheKey + "_" + tipus + "_" + guid`

4. **❌ Don't forget SonarAnalyzer suppressions**
   - Add `#pragma warning disable` with justification if needed
   - Common: `S1075` for hardcoded URIs in test/logging code

5. **❌ Don't access configuration directly**
   - Use `IOptions<T>` pattern
   - Validate configuration on startup

6. **❌ Don't return 304 with response body**
   - 304 responses must have empty body
   - Only set ETag header, no content

---

## Useful Commands

```bash
# Build solution
dotnet build androkat.web.sln

# Run tests
dotnet test

# Publish single file
dotnet publish -c Release

# Run application
dotnet run --project androkat.web/androkat.web.csproj

# EF Core migrations
dotnet ef migrations add MigrationName --project androkat.infrastructure
dotnet ef database update --project androkat.infrastructure
```

---

## Resources

- **FastEndpoints Docs**: <https://fast-endpoints.com/>
- **Autofac Docs**: <https://autofac.org/>
- **Serilog**: <https://serilog.net/>
- **ETag RFC**: RFC 7232 (HTTP/1.1 Conditional Requests)

---

*Last Updated: December 2025*
*Project Version: 2.133.0*
