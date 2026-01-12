# OpenAPI Spectral Validation Demo

This project demonstrates how to use [Spectral](https://stoplight.io/open-source/spectral) to validate OpenAPI documents generated from an ASP.NET Core application.

## Project Structure

```
├── .github/
│   └── workflows/
│       └── openapi-validation.yml    # GitHub Action for CI validation
├── src/
│   └── UsersApi/
│       ├── Program.cs                # Application entry point
│       ├── User.cs                   # User model and DTOs
│       ├── UsersEndpoints.cs         # CRUD endpoints with TypedResults
│       └── UsersApi.csproj           # Project file with OpenAPI generation
├── .spectral.yml                     # Spectral linting rules
├── openapi.json                      # Generated OpenAPI document (after build)
└── README.md
```

## Features

- **.NET 10 Minimal API** with TypedResults for proper OpenAPI response documentation
- **Build-time OpenAPI generation** using `Microsoft.Extensions.ApiDescription.Server`
- **Spectral validation** for OpenAPI document linting
- **GitHub Actions** for automated CI validation

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
- [Node.js](https://nodejs.org/) (for Spectral)
- [Spectral CLI](https://github.com/stoplightio/spectral#-installation)

## Getting Started

### 1. Build the project (generates OpenAPI document)

```bash
dotnet build src/UsersApi/UsersApi.csproj
```

This generates `openapi.json` in the repository root.

### 2. Install Spectral

```bash
npm install -g @stoplight/spectral-cli
```

### 3. Validate the OpenAPI document

```bash
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

The `.spectral.yml` file extends the default OpenAPI ruleset:

```yaml
extends: ["spectral:oas"]
```

You can customize rules or add your own. See [Spectral documentation](https://docs.stoplight.io/docs/spectral/ZG9jOjYyMDc0NA-overview) for more options.

### GitHub Actions

The workflow (`.github/workflows/openapi-validation.yml`) runs on every push and pull request:

1. Builds the .NET project (generating `openapi.json`)
2. Installs Spectral
3. Validates the OpenAPI document
4. Fails the build if any warnings or errors are found

## License

MIT