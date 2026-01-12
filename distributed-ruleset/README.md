# OpenAPI Spectral Ruleset

This is a distributed Spectral ruleset for OpenAPI validation, designed to be published to GitHub Packages and consumed by other projects.

## Features

This ruleset extends the standard `spectral:oas` ruleset and adds custom rules for:

- **operation-summary**: Ensures all operations have a summary (warning)
- **operation-description**: Ensures all operations have a description (info)
- **operation-operationId**: Ensures all operations have an operationId (error)

## Installation

```bash
npm install @sander1095/openapi-spectral-ruleset
```

## Usage

In your `.spectral.yml` file:

```yaml
extends: ["@sander1095/openapi-spectral-ruleset"]
```

Or extend it along with other rulesets:

```yaml
extends: 
  - "spectral:oas"
  - "@sander1095/openapi-spectral-ruleset"
```

## Publishing

This package is automatically published to GitHub Packages via GitHub Actions when changes are merged to the main branch.

You can also manually trigger the publish workflow from the Actions tab.
