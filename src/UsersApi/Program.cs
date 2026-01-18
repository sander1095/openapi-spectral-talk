using System.Reflection;
using Scalar.AspNetCore;
using UsersApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi(x =>
{

});

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapOpenApi();

app.MapScalarApiReference();

app.MapUsersEndpoints();

app.Run();
