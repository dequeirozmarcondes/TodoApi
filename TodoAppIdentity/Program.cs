using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoAppIdentity.Context;
using TodoAppIdentity.Model;
using TodoAppIdentity.Services;

var builder = WebApplication.CreateBuilder(args);

//  Adiciona as configura��es padr�o do Aspire (telemetria, health checks, etc.)
builder.AddServiceDefaults();

// --- Conectar ao Banco de Dados Identity ---
builder.AddNpgsqlDbContext<ApplicationDbContext>("IdentityDb");

// --- Configurar o ASP.NET Core Identity ---
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// --- Configurar a Emiss�o e Valida��o do JWT ---
var jwtKey = builder.Configuration["Jwt:Key"]
             ?? throw new InvalidOperationException("JWT Key n�o configurada.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer n�o configurado.");

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,

        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,

        ValidateAudience = false, // Pode habilitar se quiser validar a audi�ncia

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero // Sem toler�ncia de expira��o
    };
});

// --- Servi�os de aplica��o ---
builder.Services.AddSingleton(signingKey);
builder.Services.AddScoped<TokenService>();

builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddOpenApi();

// --- Monta o pipeline ---
var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Aplica migra��es automaticamente (somente DEV)
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
