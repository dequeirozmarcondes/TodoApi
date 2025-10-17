using TodoApp.Application.DTOs;

namespace TodoApp.Application.Services
{
    public interface ITodoItemService
    {
        Task<IEnumerable<TodoItemResponseDto>> GetAllItemsAsync();
        Task<TodoItemResponseDto?> GetItemByIdAsync(Guid id);
        Task<TodoItemResponseDto> CreateItemAsync(CreateTodoItemDto dto);
        Task<bool> UpdateItemAsync(Guid id, UpdateTodoItemDto dto);
        Task<bool> DeleteItemAsync(Guid id);
    }
}