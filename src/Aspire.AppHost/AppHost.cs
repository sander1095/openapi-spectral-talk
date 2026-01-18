var builder = DistributedApplication.CreateBuilder(args);

// Add Azure API Center infrastructure
var apiCenter = builder.AddBicepTemplate(
    "api-center",
    "../../infra/api-center.bicep")
    .WithParameter("location", builder.AddParameter("location", () => "westeurope"));

// Add Azure Container Apps Environment
var acaEnv = builder.AddAzureContainerAppEnvironment("aca-env");

builder.AddProject<Projects.UsersApi>("usersapi")
  .WithHttpHealthCheck("/health")
  .WithExternalHttpEndpoints();

builder.Build().Run();
