# Versioning Strategy

This document explains the automatic versioning strategy implemented in the `publish-shortcuts-results` workflow.

## Versioning Rules

The workflow automatically determines the version number based on the branch, event type, and tag format. Here's how it works:

### 1. **Tag-Based Versioning (Highest Priority)**

When a commit has a tag in the format `Shortcuts.Results-[major].[minor].[revision]`:

```
Tag: Shortcuts.Results-1.2.3
Version: 1.2.3

Tag: Shortcuts.Results-2.0.0-beta.1
Version: 2.0.0-beta.1
```

- ✅ **Valid formats**: `1.2.3`, `1.2.3-alpha.1`, `1.2.3-beta`, `1.2.3-rc.1`
- ❌ **Invalid formats**: `v1.2.3`, `1.2`, `release-1.2.3` (will fall back to auto-versioning)

### 2. **Release/Hotfix Branch Versioning**

Branches matching `release/**` or `hotfix/**`:

```
Branch: release/1.0.0
Latest version: 0.0.4
→ New version: 0.0.5 (revision +1)

Branch: hotfix/critical-fix
Latest version: 1.2.3
→ New version: 1.2.4 (revision +1)
```

- **Action**: Increments the **revision** number
- **Publishes**: ✅ Yes
- **Creates release**: ✅ Yes

### 3. **Main Branch from Merged PR**

When a PR is merged into `main`:

```
Event: Pull Request merged to main
Latest version: 1.2.3
→ New version: 1.3.0 (minor +1, revision reset to 0)
```

- **Action**: Increments the **minor** number, resets **revision** to 0
- **Publishes**: ✅ Yes
- **Creates release**: ✅ Yes

### 4. **Main Branch Direct Push**

Direct commits to `main` (not from PR):

```
Event: Direct push to main
Latest version: 1.2.3
→ New version: 1.2.4 (revision +1)
```

- **Action**: Increments the **revision** number
- **Publishes**: ✅ Yes
- **Creates release**: ✅ Yes

### 5. **Other Branches (Development)**

Any other branch (feature branches, develop, etc.):

```
Branch: feature/new-feature
Latest version: 1.2.3
→ New version: 1.2.3-dev
```

- **Action**: Uses current version with `-dev` suffix
- **Publishes**: ❌ No (build and test only)
- **Creates release**: ❌ No

## Initial Version

If no previous versions exist:

```
No tags found
→ Version: 0.0.1
```

## Workflow Triggers

The workflow runs on:

1. **Push to branches**:
   - `main`
   - `release/**` (e.g., `release/1.0.0`, `release/v2.0`)
   - `hotfix/**` (e.g., `hotfix/security-fix`)

2. **Tag push**:
   - `Shortcuts.Results-*` (e.g., `Shortcuts.Results-1.0.0`)

3. **Pull Request closed** (merged):
   - Target branch: `main`

## Examples

### Example 1: Feature Development
```bash
# On feature branch
git checkout -b feature/new-api
git commit -m "Add new API"
git push origin feature/new-api

# Workflow runs:
# ✅ Build: Success
# ✅ Test: Success
# ❌ Publish: Skipped (development branch)
# Version: 0.0.4-dev (if latest is 0.0.4)
```

### Example 2: Release Branch
```bash
# Create release branch
git checkout -b release/1.0.0
git commit -m "Prepare release 1.0.0"
git push origin release/1.0.0

# Workflow runs:
# ✅ Build: Success
# ✅ Test: Success
# ✅ Publish: Success
# Version: 0.0.5 (auto-incremented from 0.0.4)
```

### Example 3: Merge to Main
```bash
# After PR is merged to main
# Workflow runs:
# ✅ Build: Success
# ✅ Test: Success
# ✅ Publish: Success
# Version: 0.1.0 (minor incremented from 0.0.5)
```

### Example 4: Manual Tag
```bash
# Create specific version tag
git tag Shortcuts.Results-1.0.0
git push origin Shortcuts.Results-1.0.0

# Workflow runs:
# ✅ Build: Success
# ✅ Test: Success
# ✅ Publish: Success
# Version: 1.0.0 (from tag)
```

### Example 5: Hotfix
```bash
# Create hotfix branch
git checkout -b hotfix/security-patch
git commit -m "Fix security issue"
git push origin hotfix/security-patch

# Workflow runs:
# ✅ Build: Success
# ✅ Test: Success
# ✅ Publish: Success
# Version: 1.0.1 (incremented from 1.0.0)
```

## Version Comparison

| Scenario | Latest Version | New Version | Publishes |
|----------|---------------|-------------|-----------|
| Tag: `Shortcuts.Results-2.0.0` | Any | `2.0.0` | ✅ |
| Release branch | `1.2.3` | `1.2.4` | ✅ |
| Hotfix branch | `1.2.3` | `1.2.4` | ✅ |
| PR merged to main | `1.2.3` | `1.3.0` | ✅ |
| Direct push to main | `1.2.3` | `1.2.4` | ✅ |
| Feature branch | `1.2.3` | `1.2.3-dev` | ❌ |
| No previous versions | None | `0.0.1` | ✅ (if eligible) |

## Best Practices

### Recommended Workflow

1. **Feature Development**: Work on `feature/*` branches (no publish)
2. **Integration**: Merge features to `develop` or `main` via PR (minor bump)
3. **Release Preparation**: Create `release/*` branch (revision bump)
4. **Production Release**: Tag with specific version if needed
5. **Emergency Fixes**: Use `hotfix/*` branches (revision bump)

### Version Tagging

When you want to control the exact version:

```bash
# For production release
git tag Shortcuts.Results-1.0.0
git push origin Shortcuts.Results-1.0.0

# For pre-release
git tag Shortcuts.Results-1.0.0-beta.1
git push origin Shortcuts.Results-1.0.0-beta.1
```

### Branch Naming

- ✅ `release/1.0.0`, `release/v2.0`, `release/major-update`
- ✅ `hotfix/security-fix`, `hotfix/bug-123`
- ✅ `feature/new-api`, `feature/improvement`

## Troubleshooting

### Issue: Version not incrementing as expected

**Solution**: Check the latest tag:
```bash
git tag -l "Shortcuts.Results-*" | sort -V | tail -n 1
```

### Issue: Workflow not publishing

**Solution**: Ensure you're on a publishable branch/event:
- `main`, `release/**`, `hotfix/**` branches
- Valid tag format
- PR merged (not just closed)

### Issue: Wrong version calculated

**Solution**: The workflow uses the highest version tag. Clean up old tags if needed:
```bash
# List all tags
git tag -l "Shortcuts.Results-*"

# Delete local tag
git tag -d Shortcuts.Results-x.y.z

# Delete remote tag
git push origin :refs/tags/Shortcuts.Results-x.y.z
```

## Migration Notes

If migrating from the old workflow:

1. The workflow now auto-versions on release/hotfix branches
2. No manual tagging required for release branches
3. PR merges to main increment minor version
4. Tag format must be exact: `Shortcuts.Results-[major].[minor].[revision]`

## Technical Details

The version determination logic is in the `determine-version` job:

1. Checks for valid tag format
2. Fetches latest version from tags
3. Determines version based on branch/event
4. Sets `should_publish` flag
5. Outputs version for downstream jobs
