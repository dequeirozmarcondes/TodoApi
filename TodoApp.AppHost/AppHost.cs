var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.TodoApp>("todoapp");

builder.Build().Run();
