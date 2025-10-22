using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Mapping;
using TodoApp.Application.Services;
using TodoApp.Infrastructure.Data.TodoAppContext;
using TodoApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Adiciona as configura��es padr�o do Aspire (log, health checks, etc.)
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

// 1. Adiciona o servi�o de autentica��o JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtKey = builder.Configuration["Jwt:Key"];

        // Tenta obter o emissor din�mico do Aspire primeiro
        var aspireIssuer = builder.Configuration["Services:todoappidentity:https__default"];

        // Se o Aspire n�o injetou o valor, usa o valor est�tico do appsettings.json
        var issuer = !string.IsNullOrEmpty(aspireIssuer)
                     ? aspireIssuer
                     : builder.Configuration["Jwt:Issuer"];

        // Se ainda for nulo, lan�a a exce��o (isto cobre o caso de n�o rodar com Aspire E n�o ter Jwt:Issuer)
        if (string.IsNullOrEmpty(issuer))
            throw new InvalidOperationException("O emissor JWT n�o est� configurado (Verifique Services:todoappidentity:https__default e Jwt:Issuer).");

        if (string.IsNullOrEmpty(jwtKey))
            throw new InvalidOperationException("A chave JWT n�o est� configurada (Jwt:Key).");


        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

            ValidateIssuer = true,
            ValidIssuer = issuer, // Agora usa o valor din�mico OU o est�tico

            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// 2. Adiciona o servi�o de autoriza��o (necess�rio para o [Authorize])
builder.Services.AddAuthorization();

// Adiciona controllers
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    //Aplica migra��es no banco de dados de Tarefas durante o startup em dev
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

//Middleware de Autentica��o DEVE vir antes do UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();