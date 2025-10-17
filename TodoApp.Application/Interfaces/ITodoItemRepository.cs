using TodoApp.Domain.Entities;

namespace TodoApp.Application.Interfaces
{
    public interface ITodoItemRepository
    {
        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task<TodoItem?> GetByIdAsync(Guid id);
        Task AddAsync(TodoItem item);
        Task UpdateAsync(TodoItem item);
        Task DeleteAsync(TodoItem item);
    }
}
