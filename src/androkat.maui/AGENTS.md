# AndroKat MAUI Project - Agent Context

## Project Overview

**AndroKat** is a cross-platform mobile application built with .NET MAUI that provides Catholic religious content including prayers, books, audio content, videos, and daily readings. The app is available for both Android and iOS platforms.

### Key Information

- **Framework**: .NET 10.0 with .NET MAUI
- **Target Platforms**: Android (API 24+), iOS (15.0+)
- **Language**: C# with Hungarian UI text
- **Architecture**: MVVM pattern with dependency injection
- **Backend API**: <http://api.androkat.hu> (HTTP-only, non-SSL)

## Project Structure

```
androkat.maui/
├── androkat.maui/              # Main MAUI application
│   ├── Pages/                  # XAML pages and code-behind
│   ├── Helpers/                # Utility classes
│   ├── Platforms/              # Platform-specific code
│   │   ├── Android/           # Android-specific implementations
│   │   └── iOS/               # iOS-specific implementations
│   └── Resources/             # Images, fonts, assets
├── androkat.maui.library/     # Shared business logic
│   ├── Abstraction/           # Interfaces
│   ├── Services/              # Service implementations
│   ├── ViewModels/            # MVVM view models
│   ├── Models/                # Data models and entities
│   ├── Converters/            # XAML value converters
│   └── Data/                  # Data access layer
└── androkat.maui.unittest/    # Unit tests
```

## Coding Standards

### General Guidelines

- Use C# 12 features and nullable reference types
- Follow Microsoft's C# coding conventions
- Use `var` for local variables when type is obvious
- Prefer expression-bodied members for simple properties and methods
- Keep methods focused and small (Single Responsibility Principle)

### Naming Conventions

- **PascalCase**: Classes, methods, properties, public fields
- **camelCase**: Private fields (prefix with `_` for backing fields)
- **Hungarian notation**: Avoid except for UI controls in XAML code-behind where helpful

### XAML Guidelines

- Use `x:Name` instead of `Name` for element names
- Prefer data binding over code-behind manipulation
- Use resource dictionaries for reusable styles
- Keep XAML files clean and readable with proper indentation

### Platform-Specific Code

- Use conditional compilation directives (`#if ANDROID`, `#if IOS`) for platform-specific code
- Create platform-specific service implementations in `Platforms/` folders
- Register platform services using conditional compilation in dependency injection setup

Example:

```csharp
#if ANDROID
builder.Services.AddSingleton<IDeviceDisplayService, Platforms.Android.Services.AndroidDeviceDisplayService>();
#elif IOS
builder.Services.AddSingleton<IDeviceDisplayService, Platforms.iOS.Services.iOSDeviceDisplayService>();
#endif
```

## Key Technical Decisions

### iOS Configuration

- **RuntimeIdentifier**: Use `iossimulator-arm64` for local development, `ios-arm64` for physical devices
- **App Transport Security**: Configured to allow HTTP connections to `api.androkat.hu` domain
- **Development Team**: 9TBTF2XRAM
- **Bundle Identifier**: hu.AndroKat

### Android Configuration

- **Target SDK**: API 36
- **Minimum SDK**: API 24
- **Package Name**: hu.AndroKat
- **Permissions**: Storage permissions handled conditionally based on Android version

### Cross-Platform Compatibility

- File storage paths differ between platforms:
  - **Android**: Uses `Android.OS.Environment.GetExternalStoragePublicDirectory` for downloads
  - **iOS**: Uses `Environment.SpecialFolder.MyDocuments` for file storage
- File picker types must be specified per platform using `DevicePlatform` dictionary

## Important Patterns

### Dependency Injection

- Services are registered in `MauiProgram.cs`
- Pages registered as Singleton or Transient based on navigation requirements
- ViewModels injected into pages via constructor

### Navigation

- Shell-based navigation with `Shell.Current.GoToAsync()`
- Track navigation stack count to manage state on navigation
- Clear content lists when not returning from detail pages

### Data Binding

- ViewModels implement property change notification
- Use `BindingContext` for view-model binding
- Prefer `{Binding}` syntax in XAML

### Error Handling

- Wrap API calls in try-catch blocks
- Display user-friendly messages in Hungarian
- Use `DisplayAlert` or Toast notifications for user feedback
- Note: `DisplayAlert` is deprecated, prefer `DisplayAlertAsync`

## Common Tasks

### Adding a New Page

1. Create XAML and code-behind in `Pages/` folder
2. Create ViewModel in `androkat.maui.library/ViewModels/`
3. Register page and ViewModel in `PagesExtensions.ConfigurePages()`
4. Add navigation route if needed

### Implementing Platform-Specific Features

1. Define interface in `androkat.maui.library/Abstraction/`
2. Create implementations in `Platforms/Android/Services/` and `Platforms/iOS/Services/`
3. Register with conditional compilation in `PagesExtensions.cs`

### Working with API

- API base URL: `http://api.androkat.hu`
- API returns JSON data
- Handle HTTP requests with proper error handling
- API endpoints are version-prefixed (e.g., `/v3/contents`, `/v2/ser`)

## Testing Guidelines

- Unit tests located in `androkat.maui.unittest/`
- Focus on testing ViewModels and Services
- Mock dependencies using interfaces
- Test business logic, not UI

## Build Commands

### Android

```bash
dotnet build -f net10.0-android -c Debug
dotnet build -f net10.0-android -c Release
```

### iOS Simulator

```bash
dotnet build -f net10.0-ios -c Debug
dotnet build -t:Run -f net10.0-ios -c Debug
```

### iOS Device (requires provisioning profiles)

```bash
dotnet build -f net10.0-ios -p:RuntimeIdentifier=ios-arm64 -c Debug
```

## Known Issues and Workarounds

### iOS Image Loading Errors

- Many image files show "fileExists == false" errors in iOS
- These are warnings, not critical errors
- Images need proper file extension and registration in project

### Deprecated APIs

- Several `DisplayAlert` calls should be migrated to `DisplayAlertAsync`
- This is a low-priority refactoring task

### Font Registration

- "Font already exists" warnings on iOS can be ignored
- Occurs when fonts are registered multiple times by the system

## Hungarian UI Text

- All user-facing text is in Hungarian
- Common terms:
  - "Kedvencek" = Favorites
  - "Törlés" = Delete
  - "Mentés" = Save
  - "Bezárás" = Close
  - "Hiba" = Error
  - "Sikeres" = Success

## Additional Context

### App Features

1. **Content Lists**: Various religious content categories (prayers, readings, etc.)
2. **Favorites**: Save and manage favorite content with import/export
3. **Audio Player**: Play audio content with playback controls
4. **Video Collection**: Browse and watch religious videos
5. **Web Radio**: Stream Catholic radio stations
6. **Confession Guide**: Multi-step guided confession process
7. **Calendar**: Religious calendar (Igenaptár) with daily readings
8. **Settings**: Theme (dark/light), font size, keep screen on

### Dependencies

- **CommunityToolkit.Maui**: For Toast notifications and additional UI controls
- **SonarAnalyzer.CSharp**: Code quality analysis
- **Microsoft.Maui.Controls**: Core MAUI framework
- **Microsoft.Maui.Essentials**: Device features and permissions

### Development Tips

- Use `dotnet clean` before switching between Android and iOS builds
- Delete `bin/` and `obj/` folders if experiencing build issues
- iOS simulator requires macOS with Xcode installed
- Physical iOS device deployment requires Apple Developer account and provisioning profiles

## Contact and Support

- App identifier: `hu.AndroKat`
- Version format: `ApplicationDisplayVersion` (e.g., "03.58") and `ApplicationVersion` (build number)
- Current version: 03.58 (Build 328)
