# Testing the Distributed Ruleset Flow

This guide walks you through testing the complete distributed ruleset functionality.

## Step 1: Test Local Ruleset

Before publishing, verify the ruleset works locally:

```bash
# Build the .NET project to generate openapi.json
dotnet build src/UsersApi/UsersApi.csproj

# Test the distributed ruleset directly
spectral lint openapi.json --ruleset ./distributed-ruleset/.spectral.yml

# Test using the main .spectral.yml (which references the distributed ruleset)
spectral lint openapi.json
```

## Step 2: Publish the Ruleset

### Option A: GitHub Actions (Recommended)

1. Push changes to the `main` branch
2. The `publish-ruleset.yml` workflow will automatically publish the package
3. Or manually trigger the workflow from the Actions tab

### Option B: Manual Publishing

```bash
cd distributed-ruleset

# Set your GitHub token with write:packages permission
export GITHUB_TOKEN=ghp_your_token_here

# Run the publish script
./publish.sh
```

**Note:** The package will be published to GitHub Packages at:
`https://github.com/sander1095/openapi-spectral-talk/packages`

## Step 3: Install the Published Ruleset

Once published, install it in your project:

```bash
# Authenticate with GitHub Packages
# Add to ~/.npmrc or create .npmrc in project root:
echo "//npm.pkg.github.com/:_authToken=YOUR_GITHUB_TOKEN" >> ~/.npmrc

# Install the package
npm install @sander1095/openapi-spectral-ruleset
```

## Step 4: Use the Published Ruleset

Update `.spectral.yml` to use the published package:

```yaml
extends: ["@sander1095/openapi-spectral-ruleset"]
```

Then run Spectral validation:

```bash
spectral lint openapi.json
```

## Step 5: Verify GitHub Actions

Push a change and verify both workflows run successfully:

1. **openapi-validation.yml**: Validates the OpenAPI document
2. **publish-ruleset.yml**: Publishes the ruleset (if distributed-ruleset changed)

## Troubleshooting

### Authentication Issues

If you get authentication errors when installing the package:

1. Make sure you have a GitHub token with `read:packages` permission
2. Configure npm to use GitHub Packages for the @sander1095 scope:
   ```bash
   npm config set @sander1095:registry https://npm.pkg.github.com
   ```
3. Set your auth token:
   ```bash
   npm config set //npm.pkg.github.com/:_authToken YOUR_GITHUB_TOKEN
   ```

### Package Not Found

If the package is not found:

1. Verify the package was published successfully in GitHub Packages
2. Make sure the package name in `distributed-ruleset/package.json` matches what you're trying to install
3. Check that the publishConfig points to the correct registry

### Ruleset Not Loading

If Spectral can't find the ruleset:

1. Make sure you ran `npm install` after adding the dependency
2. Verify the package is in `node_modules/@sander1095/openapi-spectral-ruleset`
3. Check that the `.spectral.yml` extends syntax is correct

## Local Development Workflow

For local development without publishing:

```bash
# Use the local ruleset directly
spectral lint openapi.json --ruleset ./distributed-ruleset/.spectral.yml

# Or keep .spectral.yml pointing to the local folder:
# extends: ["./distributed-ruleset/.spectral.yml"]
```

## Publishing New Versions

To publish a new version of the ruleset:

1. Update the version in `distributed-ruleset/package.json`
2. Commit and push the changes
3. The GitHub Action will automatically publish the new version
4. Or run `./distributed-ruleset/publish.sh` manually

## Testing in Other Projects

To test the ruleset in a different project:

1. Create a new project or use an existing one
2. Add `.npmrc` with GitHub Packages configuration
3. Install the ruleset: `npm install @sander1095/openapi-spectral-ruleset`
4. Create `.spectral.yml`:
   ```yaml
   extends: ["@sander1095/openapi-spectral-ruleset"]
   ```
5. Run: `spectral lint your-openapi.json`
