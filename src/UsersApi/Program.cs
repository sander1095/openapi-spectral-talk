using System.Reflection;
using UsersApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi(x =>
{
    x.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Version = "1.0.0";
        return Task.CompletedTask;
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapOpenApi();

app.MapUsersEndpoints();

app.Run();
