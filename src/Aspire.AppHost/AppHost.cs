var builder = DistributedApplication.CreateBuilder(args);


var acaEnv = builder.AddAzureContainerAppEnvironment("aca-env");

builder.AddProject<Projects.UsersApi>("usersapi")
     .WithHttpHealthCheck("/health")
     .WithExternalHttpEndpoints();

builder.Build().Run();
