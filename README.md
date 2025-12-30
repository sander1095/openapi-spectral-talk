# OpenAPI and Spectral Talk Demo

This repository contains a demonstration project for a talk about OpenAPI and Spectral. It showcases how to use .NET 10 with ASP.NET Core to generate OpenAPI documents and validate them using Spectral.

## 🎯 Features

- **✅ .NET 10 ASP.NET Core Web API** with controllers
- **✅ Automatic OpenAPI 3.1.1 document generation** using `Microsoft.AspNetCore.OpenAPI`
- **✅ Build-time OpenAPI generation** with `Microsoft.Extensions.ApiDescription.Server`
- **✅ ProblemDetails** for standardized error responses (RFC 7807)
- **✅ Spectral validation** with industry-standard rules
- **✅ Custom Spectral rule** to enforce ProblemDetails on all error responses
- **✅ CI/CD pipeline** with GitHub Actions
- **✅ VS Code CodeTour** for guided walkthrough

## 🚀 Quick Start

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js](https://nodejs.org/) (v20 or later)
- [VS Code](https://code.visualstudio.com/) with [CodeTour extension](https://marketplace.visualstudio.com/items?itemName=vsls-contrib.codetour) (optional)

### Build and Run

1. **Clone the repository**
   ```bash
   git clone https://github.com/sander1095/openapi-spectral-talk.git
   cd openapi-spectral-talk
   ```

2. **Install Node.js dependencies**
   ```bash
   npm install
   ```

3. **Build the .NET project** (this also generates the OpenAPI document)
   ```bash
   cd src/OpenApiSpectralDemo
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the OpenAPI document**
   - Runtime: http://localhost:5000/openapi/v1.json
   - Build-time generated: `/openapi/OpenApiSpectralDemo.json`

### Validate with Spectral

```bash
npm run lint:openapi
```

## 📚 Project Structure

```
.
├── .github/
│   └── workflows/
│       └── openapi-spectral.yml    # CI/CD pipeline
├── .tours/
│   └── openapi-spectral-demo.tour  # VS Code CodeTour
├── openapi/
│   └── OpenApiSpectralDemo.json    # Generated OpenAPI document
├── src/
│   └── OpenApiSpectralDemo/
│       ├── Controllers/
│       │   └── WeatherForecastController.cs  # Example API controller
│       ├── Program.cs              # Application entry point
│       └── OpenApiSpectralDemo.csproj
├── .spectral.yaml                  # Spectral configuration
└── package.json                    # Node.js dependencies
```

## 🎓 Learning Resources

### CodeTour

This repository includes an interactive CodeTour that guides you through the key components:

1. Install the [CodeTour extension](https://marketplace.visualstudio.com/items?itemName=vsls-contrib.codetour) in VS Code
2. Open the repository in VS Code
3. Open the Command Palette (Ctrl/Cmd + Shift + P)
4. Select "CodeTour: Start Tour"

### Key Concepts

#### OpenAPI Document Generation

The project uses `Microsoft.AspNetCore.OpenAPI` and `Microsoft.Extensions.ApiDescription.Server` to automatically generate OpenAPI documents:

- **Runtime generation**: The `/openapi/v1.json` endpoint serves the OpenAPI document
- **Build-time generation**: The document is generated in `/openapi/` during build

#### ProblemDetails

All error responses (4xx, 5xx) use [RFC 7807 Problem Details](https://tools.ietf.org/html/rfc7807) for consistent error handling:

```csharp
return BadRequest(new ProblemDetails
{
    Title = "Invalid day parameter",
    Detail = "Day must be between 1 and 5",
    Status = StatusCodes.Status400BadRequest
});
```

#### Spectral Validation

Spectral validates the OpenAPI document against:

1. **Base OAS rules** (`spectral:oas`) - Industry-standard OpenAPI best practices
2. **Custom rule** - Enforces ProblemDetails for all error responses

The custom rule ensures API consistency:

```yaml
error-responses-use-problem-details:
  description: All error responses (4xx and 5xx) should return ProblemDetails
  severity: error
  given: $.paths[*][*].responses[?(@property.match(/^[45]\d{2}$/))].content.application/json.schema
  then:
    - field: $ref
      function: pattern
      functionOptions:
        match: ".*ProblemDetails.*"
```

## 🔄 CI/CD Pipeline

The GitHub Actions workflow automatically:

1. ✅ Builds the .NET application
2. ✅ Generates the OpenAPI document
3. ✅ Validates it with Spectral
4. ✅ Uploads the OpenAPI document as an artifact

The pipeline runs on every push and pull request to ensure the API documentation remains valid.

## 🧪 Testing the API

### Using the .http file

Open `src/OpenApiSpectralDemo/OpenApiSpectralDemo.http` in VS Code with the REST Client extension:

```http
GET https://localhost:7000/api/WeatherForecast
```

### Using curl

```bash
# Get all weather forecasts
curl https://localhost:7000/api/WeatherForecast

# Get forecast for day 3
curl https://localhost:7000/api/WeatherForecast/3

# Trigger a 400 error (invalid day)
curl https://localhost:7000/api/WeatherForecast/99
```

## 📖 Additional Resources

- [Microsoft.AspNetCore.OpenAPI documentation](https://learn.microsoft.com/aspnet/core/fundamentals/openapi/aspnetcore-openapi)
- [Spectral documentation](https://stoplight.io/open-source/spectral)
- [RFC 7807 Problem Details](https://tools.ietf.org/html/rfc7807)
- [OpenAPI Specification](https://spec.openapis.org/oas/v3.1.0)

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
