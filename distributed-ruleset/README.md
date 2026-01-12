# OpenAPI Spectral Ruleset

This is a shared Spectral ruleset for OpenAPI validation that can be distributed via NPM.

## Features

This ruleset extends the standard Spectral OpenAPI ruleset (`spectral:oas`) and adds custom rules:

- **operation-summary**: Ensures all operations have a summary (warning level)
- **operation-description**: Encourages operations to have descriptions (info level)

## Usage

Install the package:

```bash
npm install @sander1095/openapi-spectral-ruleset
```

Reference it in your `.spectral.yml`:

```yaml
extends: ["@sander1095/openapi-spectral-ruleset"]
```

## Publishing

This package is published to GitHub Packages and can be consumed by configuring your `.npmrc` file to use the GitHub Package Registry.
