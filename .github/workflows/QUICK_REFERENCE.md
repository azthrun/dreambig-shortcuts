# Quick Reference: Publishing DreamBig.Shortcuts.Results to NuGet

## Prerequisites
1. Add `NUGET_API_KEY` secret to GitHub repository settings

## Publishing a New Version

### 1. Tag Format
```
Shortcuts.Results-<version>
```

### 2. Example Commands
```bash
# Stable release
git tag Shortcuts.Results-1.0.0
git push origin Shortcuts.Results-1.0.0

# Beta release
git tag Shortcuts.Results-1.0.0-beta.1
git push origin Shortcuts.Results-1.0.0-beta.1
```

## Workflow Pipeline

### Job Flow (Sequential)
```
1. Extract Version
        ↓
2. Build Project ✅
        ↓
3. Run Tests ✅ (only if build succeeds)
        ↓
4. Publish Package ✅ (only if tests pass & tagged)
        ↓
5. Summary 📊
```

### What Each Job Does

**Extract Version**
- Parses tag to get version number
- Runs for all triggers

**Build**
- Compiles project in Release mode
- Uploads build artifacts
- ⚠️ If this fails, pipeline stops

**Test**
- Downloads build artifacts
- Runs all 48 unit tests
- Publishes test results
- ⚠️ If any test fails, pipeline stops

**Publish** (Tags only)
- Creates NuGet package (.nupkg + .snupkg)
- Publishes to NuGet.org
- Creates GitHub Release
- Only runs if tests pass

**Summary**
- Shows status of all jobs
- Provides package link if published

## Branch Push (main/release)
- Extract Version ✅
- Build ✅
- Test ✅
- Publish ❌ (skipped)
- Summary ✅

## Tag Push (Shortcuts.Results-*)
- Extract Version ✅
- Build ✅
- Test ✅
- Publish ✅ (if build & tests succeed)
- Summary ✅

## Failure Behavior

**Build fails** → Test and Publish are skipped
**Test fails** → Publish is skipped
**Publish fails** → Package not released (can retry)

## Files Modified/Created
- `.github/workflows/publish-shortcuts-results.yml` - Multi-job workflow
- `.github/workflows/WORKFLOW_GUIDE.md` - Detailed documentation
- `DreamBig.Shortcuts.Results.csproj` - NuGet metadata

## GitHub Secret Required
**Name:** `NUGET_API_KEY`  
**How to get:** https://www.nuget.org/ → Profile → API Keys → Create new key

## Monitoring
View workflow runs: Repository → Actions tab

Each job shown separately for easy debugging.

## Package Location
After publication: https://www.nuget.org/packages/DreamBig.Shortcuts.Results/

## Key Benefits of Multi-Job Workflow
✅ Clear separation of build, test, and publish phases  
✅ Easy to identify which phase failed  
✅ Tests only run if build succeeds  
✅ Publish only runs if tests pass  
✅ Can add manual approval before publish (via environments)  
✅ Better artifact management
