# Versioning Strategy

This document describes the versioning and release strategy for the DreamBig.Shortcuts libraries.

## Version Format

We follow [Semantic Versioning 2.0.0](https://semver.org/) with the format:

```
MAJOR.MINOR.PATCH[-PRERELEASE]
```

- **MAJOR**: Incompatible API changes
- **MINOR**: New functionality in a backwards compatible manner
- **PATCH** (also called REVISION): Backwards compatible bug fixes
- **PRERELEASE**: Optional suffix for pre-release versions (e.g., `-beta.1`)

## Git Tags

All published versions are tagged in Git with the format:

```
Shortcuts.Results-{version}
```

For example: `Shortcuts.Results-1.2.0` or `Shortcuts.Results-1.3.0-beta.5`

## CI/CD Versioning Rules

The CI/CD pipeline determines versions automatically based on branch names and Git history. Here are the rules:

### Rule 1: Release Branch Commits → Beta Versions

**Trigger**: Commit pushed to a `release/**` branch with version in the branch name

**Branch Format**: `release/{major}.{minor}.{patch}` (e.g., `release/1.1.0`)

**Versioning Logic**:
- Extract the version number from the branch name
- Find the highest beta tag for this version (e.g., `1.1.0-beta.24`)
- If no beta tag exists, start with `-beta.1`
- If beta tags exist, increment the highest beta number by 1

**Examples**:
- Branch: `release/1.1.0`, No previous betas → Version: `1.1.0-beta.1`
- Branch: `release/1.1.0`, Highest beta: `1.1.0-beta.24` → Version: `1.1.0-beta.25`
- Branch: `release/2.0.0`, Highest beta: `2.0.0-beta.3` → Version: `2.0.0-beta.4`

**Actions**:
- ✅ Build
- ✅ Test
- ✅ Publish to NuGet
- ✅ Create Git tag

### Rule 2: Hotfix Branch Commits → Beta Versions

**Trigger**: Commit pushed to a `hotfix/**` branch with version in the branch name

**Branch Format**: `hotfix/{major}.{minor}.{patch}` (e.g., `hotfix/1.0.1`)

**Versioning Logic**: Same as Rule 1 (release branches)

**Examples**:
- Branch: `hotfix/1.0.1`, No previous betas → Version: `1.0.1-beta.1`
- Branch: `hotfix/1.0.1`, Highest beta: `1.0.1-beta.5` → Version: `1.0.1-beta.6`

**Actions**:
- ✅ Build
- ✅ Test
- ✅ Publish to NuGet
- ✅ Create Git tag

### Rule 3: Release/Hotfix PR Merged to Main → Stable Version

**Trigger**: PR from `release/{version}` or `hotfix/{version}` merged into `main`

**Branch Format**: Branch name contains valid version (e.g., `release/1.2.0`)

**Versioning Logic**:
- Extract the version from the branch name
- Use the version as-is **without** beta suffix

**Examples**:
- Branch: `release/1.2.0` → Version: `1.2.0`
- Branch: `hotfix/1.1.1` → Version: `1.1.1`

**Actions**:
- ✅ Build
- ✅ Test
- ✅ Publish to NuGet
- ✅ Create Git tag
- ✅ Create GitHub Release

### Rule 4: Invalid Release/Hotfix Branch → Auto-increment Patch

**Trigger**: PR from `release/**` or `hotfix/**` merged into `main`, but branch name doesn't contain a valid version

**Branch Format**: Branch name doesn't match `{major}.{minor}.{patch}` (e.g., `release/some-feature`)

**Versioning Logic**:
- Find the highest non-beta tag
- Increment the PATCH version by 1

**Examples**:
- Branch: `release/some-feature`, Highest tag: `2.1.0` → Version: `2.1.1`
- Branch: `hotfix/urgent-fix`, Highest tag: `3.4.2` → Version: `3.4.3`

**Actions**:
- ✅ Build
- ✅ Test
- ✅ Publish to NuGet
- ✅ Create Git tag
- ✅ Create GitHub Release

### Rule 5: Other PRs to Main → Auto-increment Patch

**Trigger**: PR from any non-release/hotfix branch merged into `main`

**Branch Format**: Any branch except `release/**` or `hotfix/**` (e.g., `feature/new-api`)

**Versioning Logic**: Same as Rule 4

**Examples**:
- Branch: `feature/new-api`, Highest tag: `1.5.0` → Version: `1.5.1`
- Branch: `bugfix/fix-null-ref`, Highest tag: `2.3.1` → Version: `2.3.2`

**Actions**:
- ✅ Build
- ✅ Test
- ✅ Publish to NuGet
- ✅ Create Git tag
- ✅ Create GitHub Release

### Rule 6: Other Scenarios → Build & Test Only

**Trigger**: Any commit/PR that doesn't match the above rules

**Examples**:
- Direct commits to `main` (not from PR merge)
- Commits to feature branches
- Commits to other branches

**Actions**:
- ✅ Build
- ✅ Test
- ❌ Do NOT publish to NuGet
- ❌ Do NOT create tags or releases

## Workflow Triggers

The CI/CD workflow is triggered on:

1. **Push to `release/**` branches** → Beta versioning (Rules 1)
2. **Push to `hotfix/**` branches** → Beta versioning (Rules 2)
3. **PR merged to `main`** → Stable versioning (Rules 3, 4, 5)

**Important**: The workflow will **only run** if the commit includes changes to:
- Files within the `DreamBig.Shortcuts.Results/` directory
- The workflow file itself (`.github/workflows/publish-shortcuts-results.yml`)

This ensures that changes to other projects in the solution don't trigger unnecessary builds and publications for `DreamBig.Shortcuts.Results`.

## Summary Table

| Scenario | Branch Pattern | Version Strategy | Publish | Tag | Release |
|----------|---------------|------------------|---------|-----|---------|
| Release branch commit | `release/{version}` | `{version}-beta.{n}` | ✅ | ✅ | ❌ |
| Hotfix branch commit | `hotfix/{version}` | `{version}-beta.{n}` | ✅ | ✅ | ❌ |
| Valid release/hotfix PR to main | `release/{version}` or `hotfix/{version}` | `{version}` | ✅ | ✅ | ✅ |
| Invalid release/hotfix PR to main | `release/**` or `hotfix/**` (invalid name) | Auto-increment patch | ✅ | ✅ | ✅ |
| Other PR to main | Any other branch | Auto-increment patch | ✅ | ✅ | ✅ |
| Other commits | Any | N/A | ❌ | ❌ | ❌ |

## Best Practices

### Creating a New Release

1. **Create a release branch** with the target version:
   ```bash
   git checkout -b release/1.2.0
   ```

2. **Push commits** to test and publish beta versions:
   ```bash
   git push origin release/1.2.0
   # This creates version 1.2.0-beta.1
   ```

3. **Additional commits** automatically increment beta number:
   ```bash
   # Make fixes, then push
   git push origin release/1.2.0
   # This creates version 1.2.0-beta.2
   ```

4. **Merge to main** when ready for stable release:
   ```bash
   # Create PR from release/1.2.0 to main
   # After merge, this creates version 1.2.0 (no beta suffix)
   ```

### Creating a Hotfix

Same process as release, but use `hotfix/**` branch:

```bash
git checkout -b hotfix/1.1.1
# Make fixes, push for beta versions
git push origin hotfix/1.1.1
# Merge to main for stable version
```

### Quick Patch from Main

For minor fixes that don't need beta testing:

```bash
git checkout -b fix/quick-bugfix main
# Make changes
git push origin fix/quick-bugfix
# Create PR to main
# After merge, version auto-increments (e.g., 1.5.0 → 1.5.1)
```

## Notes

- All beta versions are published to NuGet and can be consumed by adding `--prerelease` flag
- GitHub Releases are only created for stable versions (merged to main)
- Tags are created automatically after successful NuGet publication
- Version numbers must follow semantic versioning format in branch names
