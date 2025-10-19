using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TodoApp.Infrastructure.Data;
// Adicione esta linha se não estiver presente para usar Path.Combine
using System.IO;

namespace TodoApp.Infrastructure.Data
{
    // Implementa a interface para o dotnet ef
    public class TodoContextFactory : IDesignTimeDbContextFactory<TodoContext>
    {
        public TodoContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TodoContext>();

            // 🚨 AJUSTE O VALOR DA STRING DE CONEXÃO DE DESENVOLVIMENTO ABAIXO 🚨
            // Use uma string de conexão que funcione no seu ambiente local (PostgreSQL)
            var connectionString = "Host=localhost;Port=56688;Username=postgres;Password=a{*~8e676YdtyPFXM*ek7U;Database=TodoDb";

            // Assumindo que você está usando Npgsql (PostgreSQL)
            builder.UseNpgsql(connectionString);

            return new TodoContext(builder.Options);
        }
    }
}