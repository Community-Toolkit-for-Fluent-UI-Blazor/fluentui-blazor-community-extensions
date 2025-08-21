# NuGet Deployment Workflow

This document explains how to use the GitHub Actions workflow to deploy the `Community.Toolkit.For.Blazor` package to NuGet.org.

## Prerequisites

### NuGet API Key Setup

1. Create a NuGet.org account at https://nuget.org
2. Generate an API key with permissions to push packages for `Community.Toolkit.For.Blazor`
3. Add the API key as a repository secret:
   - Go to your repository settings
   - Navigate to **Secrets and variables** → **Actions**
   - Click **New repository secret**
   - Name: `NUGET_API_KEY`
   - Value: Your NuGet API key

## Deployment Methods

### Method 1: Release Tags (Recommended)

The workflow automatically triggers when you push a version tag to the repository:

```bash
# Create and push a release tag
git tag v1.0.1
git push origin v1.0.1
```

**Tag Format:** Use semantic versioning with a `v` prefix (e.g., `v1.0.0`, `v1.2.3`, `v2.0.0-beta.1`)

### Method 2: Manual Deployment

You can manually trigger the deployment from GitHub Actions:

1. Go to the **Actions** tab in your repository
2. Select the **Deploy to NuGet** workflow
3. Click **Run workflow**
4. Enter the version number (e.g., `1.0.1`)
5. Choose if it's a prerelease (optional)
6. Click **Run workflow**

## Workflow Process

The deployment workflow performs the following steps:

1. **Version Validation** - Ensures the version follows semantic versioning
2. **Environment Setup** - Installs .NET 9.0 SDK
3. **Version Update** - Updates `Directory.Build.props` with the target version
4. **Build** - Restores dependencies and builds the solution in Release mode
5. **Test** - Runs unit tests (continues even if some tests fail)
6. **Package** - Creates NuGet packages with symbols
7. **Upload Artifacts** - Saves packages as GitHub Actions artifacts
8. **Publish** - Pushes packages to NuGet.org
9. **Summary** - Provides deployment summary and links

## Generated Packages

The workflow creates two packages:

- **Main Package**: `Community.Toolkit.For.Blazor.{version}.nupkg`
- **Symbol Package**: `Community.Toolkit.For.Blazor.{version}.snupkg`

Both packages are uploaded to NuGet.org for public consumption.

## Version Management

- The workflow automatically updates the version in `Directory.Build.props`
- Version format must follow semantic versioning (e.g., `1.0.0`, `1.2.3-beta.1`)
- For tagged releases, the version is extracted from the tag name (removing the `v` prefix)
- For manual runs, you specify the version as input

## Monitoring Deployments

- Check the **Actions** tab for workflow execution status
- Packages are available as artifacts even before NuGet publication
- The workflow provides a summary with direct links to the published package
- Failed deployments will show detailed error messages

## Security Notes

- The `NUGET_API_KEY` secret is required and should be kept secure
- The workflow uses the `NuGet` environment for additional protection
- Only repository maintainers with appropriate permissions can trigger deployments

## Troubleshooting

### Common Issues

1. **Missing API Key**: Ensure `NUGET_API_KEY` is set in repository secrets
2. **Invalid Version**: Check that version follows semantic versioning format
3. **Duplicate Package**: NuGet.org doesn't allow overwriting existing versions
4. **Permission Errors**: Ensure the API key has permissions for the package

### Getting Help

- Check the workflow logs in the Actions tab for detailed error messages
- Review the [NuGet.org documentation](https://docs.microsoft.com/en-us/nuget/) for package management
- Open an issue in the repository for workflow-related problems