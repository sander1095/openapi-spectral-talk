# OpenAPI Spectral Validation Demo

This project demonstrates how to use [Spectral](https://stoplight.io/open-source/spectral) to validate OpenAPI documents generated from an ASP.NET Core application, with a focus on **distributed ruleset publishing** using NPM and GitHub Packages.

## Project Structure

```
├── .github/
│   └── workflows/
│       ├── openapi-validation.yml    # GitHub Action for CI validation
│       └── publish-ruleset.yml       # GitHub Action to publish ruleset
├── distributed-ruleset/              # NPM package for distributed Spectral ruleset
│   ├── .spectral.yml                 # Spectral ruleset definition
│   ├── package.json                  # NPM package configuration
│   ├── publish.sh                    # Manual publish script
│   └── README.md                     # Ruleset documentation
├── src/
│   └── UsersApi/
│       ├── Program.cs                # Application entry point
│       ├── User.cs                   # User model and DTOs
│       ├── UsersEndpoints.cs         # CRUD endpoints with TypedResults
│       └── UsersApi.csproj           # Project file with OpenAPI generation
├── .spectral.yml                     # Spectral config (references distributed ruleset)
├── package.json                      # Root package.json (installs distributed ruleset)
├── openapi.json                      # Generated OpenAPI document (after build)
└── README.md
```

## Features

- **.NET 10 Minimal API** with TypedResults for proper OpenAPI response documentation
- **Build-time OpenAPI generation** using `Microsoft.Extensions.ApiDescription.Server`
- **Spectral validation** for OpenAPI document linting
- **Distributed ruleset** packaged and published to GitHub Packages
- **GitHub Actions** for automated CI validation and ruleset publishing

## API Endpoints

| Method | Endpoint | Description | Responses |
|--------|----------|-------------|-----------|
| GET | `/users` | Get all users | 200 |
| GET | `/users/{id}` | Get user by ID | 200, 404 |
| POST | `/users` | Create a new user | 201, 400, 409 |
| PUT | `/users/{id}` | Update an existing user | 200, 400, 404, 409 |
| DELETE | `/users/{id}` | Delete a user | 204, 404 |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js](https://nodejs.org/) (for Spectral and NPM)
- [Spectral CLI](https://github.com/stoplightio/spectral#-installation)

## Getting Started

### 1. Build the project (generates OpenAPI document)

```bash
dotnet build src/UsersApi/UsersApi.csproj
```

This generates `openapi.json` in the repository root.

### 2. Install dependencies (including distributed ruleset)

```bash
npm install
```

This installs Spectral CLI and the distributed ruleset package from GitHub Packages.

**Note:** To install the distributed ruleset from GitHub Packages, you need to authenticate with npm. See the "Publishing the Distributed Ruleset" section below.

### 3. Validate the OpenAPI document

```bash
npm run validate
# or
spectral lint openapi.json
```

### 4. Run the API (optional)

```bash
dotnet run --project src/UsersApi/UsersApi.csproj
```

The API will be available at `http://localhost:5000`. Access the OpenAPI document at `/openapi/v1.json`.

## How It Works

### Build-Time OpenAPI Generation

The project uses `Microsoft.Extensions.ApiDescription.Server` to generate the OpenAPI document during build:

```xml
<PropertyGroup>
  <OpenApiGenerateDocuments>true</OpenApiGenerateDocuments>
  <OpenApiDocumentsDirectory>$(MSBuildProjectDirectory)/../../</OpenApiDocumentsDirectory>
  <OpenApiGenerateDocumentsOptions>--file-name openapi</OpenApiGenerateDocumentsOptions>
</PropertyGroup>
```

### Spectral Configuration

The `.spectral.yml` file references the distributed ruleset from the NPM package:

```yaml
# Use the distributed ruleset from the local folder for now
# After publishing to GitHub Packages, you can use:
# extends: ["@sander1095/openapi-spectral-ruleset"]
extends: ["./distributed-ruleset/.spectral.yml"]
```

After the ruleset is published to GitHub Packages, you can update this to:

```yaml
extends: ["@sander1095/openapi-spectral-ruleset"]
```

### GitHub Actions

The project includes two GitHub Actions workflows:

1. **openapi-validation.yml**: Runs on every push and pull request
   - Builds the .NET project (generating `openapi.json`)
   - Installs Spectral
   - Validates the OpenAPI document
   - Fails the build if any warnings or errors are found

2. **publish-ruleset.yml**: Publishes the distributed ruleset to GitHub Packages
   - Triggers on changes to the `distributed-ruleset/` folder
   - Can be manually triggered via workflow_dispatch
   - Publishes the ruleset as an NPM package to GitHub Packages

## Distributed Ruleset

This project showcases **Spectral's distributed ruleset feature** by packaging the ruleset as an NPM package and publishing it to GitHub Packages.

### Publishing the Distributed Ruleset

#### Automated Publishing (via GitHub Actions)

The ruleset is automatically published when:
- Changes are pushed to the `main` branch that affect `distributed-ruleset/`
- You can also manually trigger the workflow from the Actions tab

#### Manual Publishing

To publish the ruleset manually:

1. Navigate to the `distributed-ruleset` directory:
   ```bash
   cd distributed-ruleset
   ```

2. Set your GitHub token (with `write:packages` permission):
   ```bash
   export GITHUB_TOKEN=your_github_token_here
   ```

3. Run the publish script:
   ```bash
   ./publish.sh
   ```

### Installing the Distributed Ruleset

To use the published ruleset in another project:

1. Create an `.npmrc` file in your project:
   ```
   @sander1095:registry=https://npm.pkg.github.com
   ```

2. Authenticate with GitHub Packages (for private packages):
   ```bash
   echo "//npm.pkg.github.com/:_authToken=YOUR_GITHUB_TOKEN" >> .npmrc
   ```

3. Install the ruleset:
   ```bash
   npm install @sander1095/openapi-spectral-ruleset
   ```

4. Reference it in your `.spectral.yml`:
   ```yaml
   extends: ["@sander1095/openapi-spectral-ruleset"]
   ```

### Ruleset Rules

The distributed ruleset includes these custom rules:

- **operation-summary**: All operations must have a summary (warning)
- **operation-description**: All operations should have a description (info)
- **operation-operationId**: All operations must have an operationId (error)

These rules extend the base `spectral:oas` ruleset.

## License

MIT