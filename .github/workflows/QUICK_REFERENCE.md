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

### 3. What Happens Automatically
✅ Code checkout  
✅ .NET SDK setup  
✅ Build project in Release mode  
✅ Run all 48 unit tests  
✅ Create NuGet package (.nupkg + .snupkg)  
✅ Publish to NuGet.org  
✅ Create GitHub Release  
✅ Upload package as artifact  

## Branch Push (main/release)
- Builds ✅
- Tests ✅
- Does NOT publish ❌

## Tag Push (Shortcuts.Results-*)
- Builds ✅
- Tests ✅
- Publishes to NuGet ✅
- Creates GitHub Release ✅

## Files Modified/Created
- `.github/workflows/publish-shortcuts-results.yml` - Main workflow
- `.github/workflows/WORKFLOW_GUIDE.md` - Detailed documentation
- `DreamBig.Shortcuts.Results.csproj` - Added NuGet metadata

## GitHub Secret Required
**Name:** `NUGET_API_KEY`  
**How to get:** https://www.nuget.org/ → Profile → API Keys → Create new key

## Monitoring
View workflow runs: Repository → Actions tab

## Package Location
After publication: https://www.nuget.org/packages/DreamBig.Shortcuts.Results/
