using Microsoft.EntityFrameworkCore;
using TodoApi.Src.Core.Entities;


namespace TodoApi.Infrastructure.Database;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}