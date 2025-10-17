using Mapster;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Mapping;
using TodoApp.Application.Services;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

// Register Mapster mappings
builder.Services.RegisterMaps();
builder.Services.AddMapster();

// Register application services and repositories
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();
builder.Services.AddScoped<ITodoItemService, TodoItemService>();

// Configure DbContext with PostgreSQL
builder.AddNpgsqlDbContext<TodoContext>("TodoDb");

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
