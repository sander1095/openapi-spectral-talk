# Quick Start Guide

This guide helps you quickly get started with the distributed Spectral ruleset feature.

## What's Been Set Up

This repository now demonstrates Spectral's ruleset distribution feature using NPM packages. The setup includes:

✅ **Distributed Ruleset Package** (`distributed-ruleset/`)
- Custom Spectral rules for OpenAPI validation
- Packaged as an NPM module for distribution
- Ready to be published to GitHub Packages

✅ **Automated Publishing Workflow**
- GitHub Actions workflow to automatically publish the ruleset
- Triggers on changes to `distributed-ruleset/` folder
- Can also be run manually

✅ **Local Development Setup**
- Package.json configured to use the local ruleset
- Spectral CLI installed as a dependency
- NPM scripts for building and linting

✅ **Documentation**
- Updated README with comprehensive information
- PUBLISHING.md guide for publishing and consuming the package
- Clear instructions for both automated and manual workflows

## Getting Started

### 1. Test Locally

The project is ready to use with the local ruleset:

```bash
# Install dependencies (includes Spectral and local ruleset)
npm install

# Lint the OpenAPI document
npm run lint

# Or build and lint together
npm run validate
```

### 2. Publish the Ruleset

You have two options:

**Option A: Automatic (Recommended)**
1. Merge this PR to the `main` branch
2. Any future changes to `distributed-ruleset/` will automatically trigger publishing

**Option B: Manual**
1. Run the GitHub Actions workflow manually from the Actions tab
2. Or publish from your local machine (see PUBLISHING.md for authentication setup)

### 3. After Publishing

Once published, you can update the `package.json` to use the published version:

```json
{
  "devDependencies": {
    "@stoplight/spectral-cli": "^6.11.0",
    "@sander1095/openapi-spectral-ruleset": "^1.0.0"
  }
}
```

Change from:
```json
"@sander1095/openapi-spectral-ruleset": "file:./distributed-ruleset"
```

To:
```json
"@sander1095/openapi-spectral-ruleset": "^1.0.0"
```

### 4. Use in Other Projects

Other projects can now install and use your ruleset:

```bash
# In another project
npm install @sander1095/openapi-spectral-ruleset
```

Then in their `.spectral.yml`:
```yaml
extends: ["@sander1095/openapi-spectral-ruleset"]
```

## What the Ruleset Does

The distributed ruleset extends the standard OpenAPI ruleset (`spectral:oas`) and adds two custom rules:

1. **operation-summary** (warning): Ensures all operations have a summary
2. **operation-description** (info): Encourages operations to have descriptions

You can see these rules in action by running:
```bash
npx spectral lint openapi.json
```

## Next Steps

1. **Customize the Ruleset**: Edit `distributed-ruleset/ruleset.yaml` to add your own rules
2. **Update Documentation**: Modify `distributed-ruleset/README.md` to document your rules
3. **Version Management**: Update `distributed-ruleset/package.json` version when making changes
4. **Test Before Publishing**: Always run `npm run lint` to test your changes locally

## Files Overview

```
distributed-ruleset/
├── ruleset.yaml       # The Spectral ruleset definition
├── package.json       # NPM package configuration
└── README.md          # Ruleset documentation

.github/workflows/
├── publish-ruleset.yml      # Publishes ruleset to GitHub Packages
└── openapi-validation.yml   # Validates OpenAPI with the ruleset

.npmrc                  # NPM registry configuration for GitHub Packages
.spectral.yml          # References the distributed ruleset
package.json           # Root package with dependencies
PUBLISHING.md          # Detailed publishing guide
README.md              # Main project documentation
```

## Troubleshooting

If you encounter any issues:

1. **Local testing fails**: Make sure you ran `npm install`
2. **Publishing fails**: Check GitHub Actions logs and ensure permissions are set
3. **Can't consume package**: See the troubleshooting section in PUBLISHING.md

For more details, see:
- [PUBLISHING.md](PUBLISHING.md) - Publishing and consumption guide
- [README.md](README.md) - Main project documentation
- [distributed-ruleset/README.md](distributed-ruleset/README.md) - Ruleset documentation
