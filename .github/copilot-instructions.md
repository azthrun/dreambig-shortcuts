# GitHub Copilot Instructions

## Project Architecture

This is a **multi-project .NET solution** focusing on utility libraries. Currently contains one project (`DreamBig.Shortcuts.Results`) with plans for additional utility libraries following the same structure.

### Key Patterns

**Project Structure Convention:**
```
DreamBig.Shortcuts.{LibraryName}/
├── src/DreamBig.Shortcuts.{LibraryName}/     # Main library code
├── tests/DreamBig.Shortcuts.{LibraryName}.Tests/  # xUnit tests
└── README.md                                 # Project-specific documentation
```

**Result Pattern Implementation:**
- Use `Result<T>` for explicit error handling instead of exceptions
- Support both error messages and exceptions in `ErrorInfo`
- Include optional HTTP status codes for web API scenarios
- Follow immutable design with `init` properties
- Validate constructor arguments to prevent invalid states

## Development Workflow

**Building and Testing:**
```bash
# Build the entire solution
dotnet build DreamBig.Shortcuts.sln

# Run tests for specific project
dotnet test DreamBig.Shortcuts.Results/tests/DreamBig.Shortcuts.Results.Tests/

# Build specific project
dotnet build DreamBig.Shortcuts.Results/src/DreamBig.Shortcuts.Results/
```

**Package Publishing:**
- Uses GitHub Actions workflow triggered by `release/**` or `hotfix/**` branches
- Automatic version determination from branch names and tags
- NuGet package includes project README.md
- Symbol packages (`.snupkg`) are generated for debugging

## Code Conventions

**Testing Standards:**
- Use **xUnit** with descriptive `DisplayName` attributes
- **Shouldly** for fluent assertions (`result.IsSuccess.ShouldBeTrue()`)
- **NSubstitute** for mocking (when needed)
- Test method naming: `{Method}_{Scenario}_{ExpectedOutcome}`
- Comprehensive test coverage for all public APIs

**C# Conventions:**
- Target **.NET 9.0** with nullable reference types enabled
- Use `sealed` classes for value-like types
- Prefer `init` properties for immutable objects
- Include XML documentation for public APIs
- Use implicit operators judiciously (see `ErrorInfo` implicit conversion)

**Project Configuration:**
- Enable `ImplicitUsings` and `Nullable` in all projects
- Test projects marked as `IsPackable=false`
- Include symbol packages with `IncludeSymbols=true`
- Use environment variables in workflows (`GITHUB_REPOSITORY`)

## Key Files to Reference

- `Result.cs` - Core Result pattern implementation with validation examples
- `ResultTests.cs` - Comprehensive testing patterns with Shouldly assertions
- `publish-shortcuts-results.yml` - CI/CD workflow for versioning and publishing
- Project `.csproj` files - Package metadata and dependency management examples

When adding new utility libraries, follow the established project structure and maintain consistency with existing patterns for testing, packaging, and documentation.