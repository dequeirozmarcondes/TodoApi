// TodoApp.IdentityService/Data/ApplicationDbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TodoAppIdentity.Context;

// O Factory diz ao 'dotnet ef' como criar o contexto
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        // Esta string só é usada pelo comando 'dotnet ef' e deve apontar para o seu servidor Postgres.
        // O Host, Database, Username e Password devem corresponder ao seu ambiente local.
        var connectionString = "Host=localhost;Port=57685;Username=postgres;Password=a{*~8e676YdtyPFXM*ek7U;Database=IdentityDb";


        // Assume-se o uso do Npgsql (PostgreSQL)
        builder.UseNpgsql(connectionString);

        // Retorna a instância do Contexto
        return new ApplicationDbContext(builder.Options);
    }
}