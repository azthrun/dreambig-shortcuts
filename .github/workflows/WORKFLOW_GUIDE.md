# GitHub Actions CI/CD Workflow for DreamBig.Shortcuts.Results

This document explains how to use the GitHub Actions workflow to build, test, and publish the DreamBig.Shortcuts.Results NuGet package.

## Workflow Overview

The workflow (`publish-shortcuts-results.yml`) automatically:
1. Builds the DreamBig.Shortcuts.Results project
2. Runs all unit tests
3. Creates a NuGet package (when triggered by a tag)
4. Publishes the package to NuGet.org (when triggered by a tag)
5. Creates a GitHub Release with the package attached

## Trigger Conditions

The workflow runs when code is pushed to:
- The `main` branch
- The `release` branch
- Any tag matching the pattern `Shortcuts.Results-*`

### Behavior by Trigger Type

- **Branch push (main/release)**: Builds and tests the project, but does NOT publish to NuGet
- **Tag push**: Builds, tests, creates package, publishes to NuGet.org, and creates a GitHub Release

## Required GitHub Secrets

You need to add the following secret to your GitHub repository:

### NUGET_API_KEY
Your NuGet.org API key for publishing packages.

**How to obtain:**
1. Go to https://www.nuget.org/
2. Log in to your account
3. Navigate to your profile → API Keys
4. Create a new API key with "Push" permissions
5. Copy the generated key

**How to add to GitHub:**
1. Go to your repository on GitHub
2. Navigate to Settings → Secrets and variables → Actions
3. Click "New repository secret"
4. Name: `NUGET_API_KEY`
5. Value: Paste your NuGet API key
6. Click "Add secret"

## How to Publish a New Version

### Step 1: Ensure your code is ready
Make sure all your changes are committed and tests pass locally:
```bash
dotnet test
```

### Step 2: Create and push a version tag
The tag must follow the format: `Shortcuts.Results-<version>`

Examples:
```bash
# For a stable release
git tag Shortcuts.Results-1.0.0
git push origin Shortcuts.Results-1.0.0

# For a beta release
git tag Shortcuts.Results-1.0.0-beta.1
git push origin Shortcuts.Results-1.0.0-beta.1

# For an alpha release
git tag Shortcuts.Results-1.2.0-alpha.3
git push origin Shortcuts.Results-1.2.0-alpha.3

# For a release candidate
git tag Shortcuts.Results-2.0.0-rc.1
git push origin Shortcuts.Results-2.0.0-rc.1
```

### Step 3: Monitor the workflow
1. Go to your repository on GitHub
2. Click on the "Actions" tab
3. You should see the workflow running
4. Click on it to view detailed logs

### Step 4: Verify publication
After the workflow completes successfully:
- Check NuGet.org for your published package: https://www.nuget.org/packages/DreamBig.Shortcuts.Results/
- Check the "Releases" section in your GitHub repository for the created release

## Version Naming Convention

The version number extracted from the tag will be used for:
- NuGet package version
- Assembly version
- File version

### Supported Version Formats
- Stable: `1.0.0`, `2.3.4`
- Beta: `1.0.0-beta.1`, `1.0.0-beta.2`
- Alpha: `1.0.0-alpha.1`, `1.0.0-alpha.2`
- Release Candidate: `1.0.0-rc.1`, `1.0.0-rc.2`
- Any pre-release: `1.0.0-preview.1`, `1.0.0-dev.1`

Versions containing `beta`, `alpha`, or `rc` will be marked as pre-release in GitHub Releases.

## Package Metadata

The following metadata is included in the NuGet package (configured in `DreamBig.Shortcuts.Results.csproj`):

- **PackageId**: DreamBig.Shortcuts.Results
- **Description**: A lightweight Result pattern implementation for handling success and failure outcomes in .NET applications
- **Tags**: result, result-pattern, functional, error-handling, railway-oriented
- **License**: MIT
- **Target Framework**: .NET 9.0

You can customize these values by editing the `.csproj` file.

## Workflow Features

### Build Summary
After each run, the workflow creates a summary showing:
- Branch or tag name
- Commit SHA
- Package version (if tagged)
- Publication status

### Test Results
Test results are automatically published and visible in the workflow run summary.

### Artifacts
NuGet packages (.nupkg and .snupkg) are uploaded as workflow artifacts and retained for 30 days.

### GitHub Release
When a tagged version is published:
- A GitHub Release is automatically created
- Release notes are auto-generated from commit messages
- The NuGet package is attached to the release
- Pre-release versions are marked accordingly

## Troubleshooting

### Workflow fails at "Publish to NuGet.org"
- Verify your `NUGET_API_KEY` secret is set correctly
- Check that the API key has "Push" permissions
- Ensure the package version doesn't already exist on NuGet.org

### Tests fail
- Run tests locally first: `dotnet test`
- Check the test logs in the workflow run details

### Package already exists
The workflow uses `--skip-duplicate` flag, so it won't fail if the package version already exists. However, you should avoid re-pushing the same version.

### Invalid version format
Ensure your tag follows the format: `Shortcuts.Results-<version>`
The version part should follow semantic versioning (e.g., 1.0.0, 1.0.0-beta.1)

## Local Testing

You can test package creation locally:

```bash
# Build in Release mode
dotnet build DreamBig.Shortcuts.Results/src/DreamBig.Shortcuts.Results/DreamBig.Shortcuts.Results.csproj --configuration Release

# Run tests
dotnet test DreamBig.Shortcuts.Results/tests/DreamBig.Shortcuts.Results.Tests/DreamBig.Shortcuts.Results.Tests.csproj --configuration Release

# Create package
dotnet pack DreamBig.Shortcuts.Results/src/DreamBig.Shortcuts.Results/DreamBig.Shortcuts.Results.csproj \
  --configuration Release \
  -p:Version=1.0.0-local \
  --output ./packages
```

## Future Projects

The tag format `Shortcuts.Results-*` is specifically for this project. When you add new projects to this repository, you can:

1. Create new workflow files with different tag patterns (e.g., `Shortcuts.OtherProject-*`)
2. Or modify this workflow to handle multiple projects based on the tag prefix

This allows you to version and publish different packages independently within the same repository.
