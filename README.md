# OpenAPI Spectral Validation Demo

This project demonstrates how to use [Spectral](https://stoplight.io/open-source/spectral) to validate OpenAPI documents generated from an ASP.NET Core application, with a focus on **distributing custom rulesets via NPM packages**.

> 🚀 **Quick Start**: New to this project? Check out the [Quick Start Guide](QUICKSTART.md) to get up and running quickly!

## Project Structure

```
├── .github/
│   └── workflows/
│       ├── openapi-validation.yml    # GitHub Action for CI validation
│       └── publish-ruleset.yml       # GitHub Action to publish ruleset package
├── distributed-ruleset/
│   ├── ruleset.yaml                  # Custom Spectral ruleset
│   ├── package.json                  # NPM package configuration
│   └── README.md                     # Ruleset documentation
├── src/
│   └── UsersApi/
│       ├── Program.cs                # Application entry point
│       ├── User.cs                   # User model and DTOs
│       ├── UsersEndpoints.cs         # CRUD endpoints with TypedResults
│       └── UsersApi.csproj           # Project file with OpenAPI generation
├── .npmrc                            # NPM config for GitHub Packages
├── .spectral.yml                     # Spectral configuration (uses distributed ruleset)
├── package.json                      # NPM dependencies (includes ruleset package)
├── openapi.json                      # Generated OpenAPI document (after build)
└── README.md
```

## Features

- **.NET 10 Minimal API** with TypedResults for proper OpenAPI response documentation
- **Build-time OpenAPI generation** using `Microsoft.Extensions.ApiDescription.Server`
- **Spectral validation** for OpenAPI document linting
- **Distributed Spectral ruleset** published as an NPM package to GitHub Packages
- **GitHub Actions** for automated CI validation and package publishing

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
- [Node.js](https://nodejs.org/) (version 18 or higher)

## Getting Started

### 1. Install NPM dependencies

```bash
npm install
```

This installs Spectral and the custom distributed ruleset package.

**Note:** For local development, the `package.json` uses a local file reference (`file:./distributed-ruleset`). When the ruleset is published to GitHub Packages, update the version to use the published package: `"@sander1095/openapi-spectral-ruleset": "^1.0.0"`.

### 2. Build the project (generates OpenAPI document)

```bash
npm run build:api
```

Or directly with dotnet:

```bash
dotnet build src/UsersApi/UsersApi.csproj
```

This generates `openapi.json` in the repository root.

### 3. Validate the OpenAPI document

```bash
npm run lint
```

Or directly with Spectral:

```bash
npx spectral lint openapi.json
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

The `.spectral.yml` file references the distributed ruleset package:

```yaml
extends: ["@sander1095/openapi-spectral-ruleset"]
```

This ruleset is defined in `distributed-ruleset/ruleset.yaml` and extends the standard OpenAPI ruleset with custom rules:

```yaml
extends: ["spectral:oas"]

rules:
  operation-summary:
    description: "Operations must have a summary"
    severity: warn
    
  operation-description:
    description: "Operations should have descriptions"
    severity: info
```

### Distributed Ruleset Package

The `distributed-ruleset/` folder contains a standalone NPM package that can be published and shared across multiple projects. This demonstrates Spectral's ability to distribute rulesets via NPM.

#### Publishing the Ruleset

The ruleset is automatically published to GitHub Packages when changes are pushed to the `distributed-ruleset/` folder on the `main` branch.

To publish manually:

```bash
cd distributed-ruleset
npm publish
```

**Note:** You need to configure authentication for GitHub Packages. The `.npmrc` file in the root directory handles this configuration.

#### Using the Published Ruleset

Once published, other projects can install and use the ruleset:

1. Configure `.npmrc` to use GitHub Packages:
   ```
   @sander1095:registry=https://npm.pkg.github.com
   ```

2. Install the package:
   ```bash
   npm install @sander1095/openapi-spectral-ruleset
   ```

3. Reference it in `.spectral.yml`:
   ```yaml
   extends: ["@sander1095/openapi-spectral-ruleset"]
   ```

### GitHub Actions

This project includes two automated workflows:

#### 1. OpenAPI Validation (`openapi-validation.yml`)

Runs on every push and pull request:

1. Builds the .NET project (generating `openapi.json`)
2. Installs NPM dependencies (including Spectral and the custom ruleset)
3. Validates the OpenAPI document with Spectral
4. Fails the build if any warnings or errors are found

#### 2. Publish Ruleset Package (`publish-ruleset.yml`)

Runs when changes are pushed to the `distributed-ruleset/` folder on `main`, or manually via workflow dispatch:

1. Packages the ruleset
2. Publishes it to GitHub Packages
3. Makes it available for consumption by other projects

Both workflows can also be triggered manually using the "Run workflow" button in the GitHub Actions tab.

For detailed instructions on publishing and consuming the ruleset package, see [PUBLISHING.md](PUBLISHING.md).

## License

MIT