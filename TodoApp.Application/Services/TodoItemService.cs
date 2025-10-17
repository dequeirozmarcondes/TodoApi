using Mapster;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Services;
using TodoApp.Domain.Entities;

public class TodoItemService : ITodoItemService
{
    private readonly ITodoItemRepository _repository;

    public TodoItemService(ITodoItemRepository repository)
    {
        _repository = repository;
    }

    // Método GetAllItemsAsync simplificado
    public async Task<IEnumerable<TodoItemResponseDto>> GetAllItemsAsync()
    {
        var todoItems = await _repository.GetAllAsync();

        // Mapeamento Automático: Entidade para DTO
        return todoItems.Adapt<IEnumerable<TodoItemResponseDto>>();
    }

    public async Task<TodoItemResponseDto?> GetItemByIdAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null) return null;

        // Mapeamento Automático: Entidade para DTO
        return item.Adapt<TodoItemResponseDto>();
    }

    public async Task<TodoItemResponseDto> CreateItemAsync(CreateTodoItemDto dto)
    {
        // Mapeamento Automático: DTO para Entidade
        var newItem = dto.Adapt<TodoItem>();

        await _repository.AddAsync(newItem);

        // Mapeamento Automático: Entidade para DTO de resposta
        return newItem.Adapt<TodoItemResponseDto>();
    }

    // TodoApp.Application/Services/TodoItemService.cs

    public async Task<bool> UpdateItemAsync(Guid id, UpdateTodoItemDto dto)
    {
        var todoItem = await _repository.GetByIdAsync(id);
        if (todoItem == null)
        {
            return false;
        }

        // 1. Lógica de Atualização de Nome (proteção contra null)
        if (!string.IsNullOrWhiteSpace(dto.Name) && todoItem.Name != dto.Name)
        {
            todoItem.UpdateName(dto.Name);
        }

        // 2. Lógica de Conclusão (CHAMA O MÉTODO DE DOMÍNIO)
        // Se o DTO tem IsComplete = true E a entidade ainda não está completa.
        if (dto.IsComplete.HasValue && dto.IsComplete.Value && !todoItem.IsComplete)
        {
            // 🚨 CHAMA O MÉTODO DE DOMÍNIO: Isso dispara o CompletedAt = DateTime.UtcNow
            todoItem.MarkAsComplete();
        }
        // Nota: Se IsComplete for false ou null no DTO, ele não faz nada, 
        // o que é o comportamento correto para um PATCH.

        // 3. Persistência
        await _repository.UpdateAsync(todoItem);

        return true;
    }

    public async Task<bool> DeleteItemAsync(Guid id)
    {
        var todoItem = await _repository.GetByIdAsync(id);
        if (todoItem == null)
        {
            return false;
        }

        await _repository.DeleteAsync(todoItem);
        return true;
    }
}