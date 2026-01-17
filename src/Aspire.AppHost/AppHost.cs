var builder = DistributedApplication.CreateBuilder(args);


builder.AddProject<Projects.UsersApi>("usersapi");


builder.Build().Run();
