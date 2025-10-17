using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Services;
using TodoApp.Application.DTOs;

namespace TodoApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController(ITodoItemService todoItemService) : ControllerBase
{
    private readonly ITodoItemService _todoItemService = todoItemService;

    // GET: api/TodoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemResponseDto>>> GetTodoItems()
    {
        var items = await _todoItemService.GetAllItemsAsync();
        return Ok(items);
    }

    // GET: api/TodoItems/5
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TodoItemResponseDto>> GetTodoItem(Guid id)
    {
        var todoItem = await _todoItemService.GetItemByIdAsync(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        return Ok(todoItem);
    }

    // POST: api/TodoItems
    [HttpPost]
    public async Task<ActionResult<TodoItemResponseDto>> PostTodoItem(CreateTodoItemDto todoDTO)
    {
        // O serviço faz a criação e retorna o DTO de resposta
        var createdItem = await _todoItemService.CreateItemAsync(todoDTO);

        // Retorna 201 Created com a URL para o novo recurso
        return CreatedAtAction(
            nameof(GetTodoItem),
            new { id = createdItem.Id },
            createdItem);
    }

    // PATCH: api/TodoItems/5
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> PatchTodoItem(Guid id, UpdateTodoItemDto todoDTO)
    {
        // O serviço manipula a lógica de atualização e verifica a existência
        var success = await _todoItemService.UpdateItemAsync(id, todoDTO);

        if (!success)
        {
            return NotFound(); // Item não encontrado ou erro de concorrência
        }

        return NoContent(); // 204 Success, sem corpo de resposta
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTodoItem(Guid id)
    {
        var success = await _todoItemService.DeleteItemAsync(id);

        if (!success)
        {
            return NotFound();
        }

        return NoContent(); // 204 Success
    }
}