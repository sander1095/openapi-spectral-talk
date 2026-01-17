# Copilot Instructions for OpenAPI Spectral Validation Demo

## Project Overview

This is a **.NET 10 Aspire** project demonstrating **build-time OpenAPI generation** and validation with **Spectral**, featuring a distributed ruleset published to GitHub Packages. The solution consists of:

- **UsersApi**: Minimal API with TypedResults for precise OpenAPI response documentation
- **Aspire.AppHost**: Orchestration layer for Azure deployment to Container Apps
- **Aspire.ServiceDefaults**: Shared observability, health checks, and service discovery
- **distributed-ruleset/**: NPM package containing custom Spectral validation rules

## Architecture & Build Flow

**Critical Build Dependency**: OpenAPI document generation happens during `dotnet build` via `Microsoft.Extensions.ApiDescription.Server`, outputting [openapi.json](../openapi.json) to the repository root. The build must complete before Spectral validation can run.

**Aspire Deployment Model**: [AppHost.cs](../src/Aspire.AppHost/AppHost.cs) orchestrates deployment by:

1. Provisioning Azure API Center via [api-center.bicep](../infra/api-center.bicep)
2. Creating Azure Container Apps Environment (`aca-env`)
3. Deploying UsersApi with health checks and external endpoints

## Developer Workflows

### Build & Validate Workflow

```bash
# 1. Generate OpenAPI document (required first step)
dotnet build src/UsersApi/UsersApi.csproj

# 2. Install Spectral and distributed ruleset from GitHub Packages
npm install

# 3. Validate against custom rules
npm run validate
```

### Aspire Deployment

Use [deploy.ps1](../infra/deploy.ps1) which:

1. Runs `azd up` to deploy infrastructure
2. Registers OpenAPI spec in Azure API Center
3. Outputs deployment URLs and API Center details

### Local Development

```bash
# Run with Aspire orchestration (includes dashboard)
dotnet run --project src/Aspire.AppHost

# Standalone API (no observability)
dotnet run --project src/UsersApi
```

## Code Conventions

### Endpoint Documentation Pattern

**Always use TypedResults** to ensure accurate OpenAPI response schema generation:

```csharp
// ✅ CORRECT: Each response type documented via union type
private static Results<Ok<User>, NotFound<ProblemDetails>> GetUserById(int id)
{
    return user is null
        ? TypedResults.NotFound(new ProblemDetails { ... })
        : TypedResults.Ok(user);
}

// ❌ WRONG: Generic IResult loses OpenAPI schema information
private static IResult GetUserById(int id) { ... }
```

### OpenAPI Metadata Requirements

All endpoints must include (enforced by Spectral ruleset):

- `.WithSummary()`: Required for operation summary
- `.WithDescription()`: Recommended for detailed docs
- `.WithName()`: Required for operationId generation

See [UsersEndpoints.cs](../src/UsersApi/UsersEndpoints.cs#L21-L24) for reference implementation.

### Aspire Service Integration

Services must:

1. Reference [Aspire.ServiceDefaults](../src/Aspire.ServiceDefaults/Aspire.ServiceDefaults.csproj)
2. Call `builder.AddServiceDefaults()` in Program.cs (adds OpenTelemetry, health checks, service discovery)
3. Call `app.MapDefaultEndpoints()` to expose `/health` and `/alive` endpoints

## Spectral Validation Rules

**Distributed Ruleset**: Custom rules in [distributed-ruleset/.spectral.yml](../distributed-ruleset/.spectral.yml) enforce:

- `error-response-schema`: 4xx/5xx responses MUST define `application/json` schema with ProblemDetails
- Standard OAS rules + operation metadata (summary, description, operationId)

**Local Extension**: [.spectral.yml](../.spectral.yml) extends the distributed ruleset (commented rules show available customizations).

## Integration Points

### OpenAPI Generation Configuration

Set in [UsersApi.csproj](../src/UsersApi/UsersApi.csproj#L9-L11):

- `OpenApiGenerateDocuments`: Triggers build-time generation
- `OpenApiDocumentsDirectory`: Output to repo root for Spectral access
- `OpenApiGenerateDocumentsOptions`: Custom filename/format

### Azure API Center Bicep

[api-center.bicep](../infra/api-center.bicep) provisions:

- Free tier API Center (`apic-api-linting-free`)
- Default workspace for API organization
- Outputs for registration in deploy.ps1

### GitHub Actions

- **openapi-validation.yml**: CI validation on PRs
- **publish-ruleset.yml**: Publishes distributed ruleset to GitHub Packages on release

## Common Pitfalls

1. **Validating before build**: Spectral needs openapi.json; always run `dotnet build` first
2. **Missing TypedResults**: Using `IResult` or `Task<IResult>` breaks response schema generation
3. **npm auth**: Installing `@sander1095/openapi-spectral-ruleset` requires GitHub PAT with `read:packages` scope
4. **Aspire branch**: Currently on `aspire-deployment` branch for Azure deployment features

## Key Files Reference

- [UsersEndpoints.cs](../src/UsersApi/UsersEndpoints.cs): Endpoint implementation patterns
- [Program.cs](../src/UsersApi/Program.cs): Service registration and OpenAPI configuration
- [AppHost.cs](../src/Aspire.AppHost/AppHost.cs): Deployment orchestration
- [Extensions.cs](../src/Aspire.ServiceDefaults/Extensions.cs): Shared Aspire service configuration
