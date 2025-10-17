using Microsoft.EntityFrameworkCore;

namespace TodoApp.Infrastructure.Data
{
    public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
    {
        public DbSet<TodoApp.Domain.Entities.TodoItem> TodoItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoApp.Domain.Entities.TodoItem>(entity =>
            {
                // 🚨 CONFIGURAÇÃO DE CHAVE PRIMÁRIA GUID
                // Garante que o EF Core saiba que Guid é a chave primária
                entity.HasKey(t => t.Id);

                // 🚨 CONFIGURAÇÃO DE TEMPO
                // Garante que CreatedAt seja definido e não nulo
                entity.Property(t => t.CreatedAt)
                    .IsRequired();

                // CompletedAt pode ser nulo no banco de dados (DateTime?)
                entity.Property(t => t.CompletedAt)
                    .IsRequired(false);

                // O campo TimeSpent (TimeSpan?) é ignorado por padrão por ser somente leitura (get)
                // e não precisa ser mapeado no banco de dados.
            });
        }
    }
}