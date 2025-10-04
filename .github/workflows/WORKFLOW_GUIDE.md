# GitHub Actions CI/CD Workflow for DreamBig.Shortcuts.Results

This document explains how to use the GitHub Actions workflow to build, test, and publish the DreamBig.Shortcuts.Results NuGet package.

## Workflow Overview

The workflow (`publish-shortcuts-results.yml`) is organized into separate jobs for better control and visibility:

### Job Pipeline

1. **Extract Version** - Extracts version information from git tags
2. **Build** - Compiles the project (depends on: extract-version)
3. **Test** - Runs all unit tests (depends on: build)
4. **Publish** - Creates and publishes NuGet package (depends on: test, only runs for tags)
5. **Summary** - Generates workflow summary (depends on: all jobs)

### Job Dependencies

```
extract-version
       ↓
     build
       ↓
     test
       ↓
    publish (only for tags)
       ↓
    summary
```

Each job will only execute if the previous job completes successfully. If the build fails, testing won't run. If tests fail, publishing won't happen.

## Trigger Conditions

The workflow runs when code is pushed to:
- The `main` branch
- The `release` branch
- Any tag matching the pattern `Shortcuts.Results-*`

### Behavior by Trigger Type

- **Branch push (main/release)**: Runs extract-version → build → test → summary (no publish)
- **Tag push**: Runs extract-version → build → test → publish → summary (full pipeline)

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
3. You should see the workflow running with separate jobs
4. Click on it to view detailed logs for each job

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

## Job Details

### Extract Version Job
- Runs on: All triggers
- Purpose: Parses git tag to extract version number
- Outputs: `version` and `is_tagged` flags for other jobs

### Build Job
- Runs on: All triggers
- Dependencies: extract-version
- Purpose: Compiles the project in Release configuration
- Artifacts: Build output (retained for 1 day)
- Failure impact: Stops the entire pipeline

### Test Job
- Runs on: All triggers (only if build succeeds)
- Dependencies: extract-version, build
- Purpose: Runs all 48 unit tests with code coverage
- Artifacts: Test results and coverage reports (retained for 30 days)
- Failure impact: Prevents publishing
- Features:
  - Test result reporting
  - Code coverage collection
  - Fails if any test fails

### Publish Job
- Runs on: Only for tagged commits (if test succeeds)
- Dependencies: extract-version, test
- Environment: nuget-production (for deployment protection)
- Purpose: Creates and publishes NuGet package
- Artifacts: NuGet packages (retained for 30 days)
- Actions:
  - Creates .nupkg and .snupkg files
  - Publishes to NuGet.org
  - Creates GitHub Release

### Summary Job
- Runs on: Always (even if previous jobs fail)
- Dependencies: All other jobs
- Purpose: Generates comprehensive workflow summary
- Shows: Job statuses, version info, publication status

## Workflow Features

### Separate Job Execution
Each phase (build, test, publish) runs in its own job for:
- Better visibility in the Actions UI
- Easier debugging
- Clear failure points
- Parallel artifact handling

### Build Artifacts
Build output is shared between jobs via GitHub Actions artifacts, ensuring:
- Consistent binaries across jobs
- No need to rebuild for testing
- Faster workflow execution

### Test Results
Test results are automatically published and visible in the workflow run summary with:
- Pass/fail status for each test
- Code coverage information
- Test execution time

### Environment Protection
The publish job uses a GitHub environment (`nuget-production`) which allows you to:
- Add required reviewers before publishing
- Set deployment protection rules
- Track deployment history

### Comprehensive Summary
The summary job provides a complete overview showing:
- All job statuses (build, test, publish)
- Version information
- Direct link to NuGet package
- Publication status

## Package Metadata

The following metadata is included in the NuGet package (configured in `DreamBig.Shortcuts.Results.csproj`):

- **PackageId**: DreamBig.Shortcuts.Results
- **Description**: A lightweight Result pattern implementation for handling success and failure outcomes in .NET applications
- **Tags**: result, result-pattern, functional, error-handling, railway-oriented
- **License**: MIT
- **Target Framework**: .NET 9.0

You can customize these values by editing the `.csproj` file.

## Troubleshooting

### Build job fails
- Check the build logs in the Actions tab
- Verify the code compiles locally: `dotnet build --configuration Release`
- Ensure all dependencies are properly referenced

### Test job fails
- Review test results in the workflow summary
- Run tests locally: `dotnet test`
- Check test logs for specific failure reasons
- Only passing tests allow the workflow to continue

### Publish job doesn't run
- Verify you pushed a tag (not just a branch)
- Ensure tag matches pattern: `Shortcuts.Results-*`
- Check that build and test jobs completed successfully

### Publish job fails at "Publish to NuGet.org"
- Verify your `NUGET_API_KEY` secret is set correctly
- Check that the API key has "Push" permissions
- Ensure the package version doesn't already exist on NuGet.org

### Tests pass locally but fail in workflow
- Check for environment-specific dependencies
- Verify all test files are committed
- Look for timing-related test issues

## Local Testing

You can test the workflow steps locally:

```bash
# Build
dotnet build DreamBig.Shortcuts.Results/src/DreamBig.Shortcuts.Results/DreamBig.Shortcuts.Results.csproj --configuration Release

# Test
dotnet test DreamBig.Shortcuts.Results/tests/DreamBig.Shortcuts.Results.Tests/DreamBig.Shortcuts.Results.Tests.csproj --configuration Release

# Package
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

## Advanced Configuration

### Adding Environment Protection Rules
1. Go to Settings → Environments
2. Select or create `nuget-production` environment
3. Add protection rules:
   - Required reviewers
   - Wait timer
   - Deployment branches

### Customizing Job Timeouts
Add timeout configuration to any job:
```yaml
jobs:
  build:
    timeout-minutes: 15
```

### Enabling Matrix Builds
To test against multiple .NET versions:
```yaml
strategy:
  matrix:
    dotnet-version: ['8.0.x', '9.0.x']
```
