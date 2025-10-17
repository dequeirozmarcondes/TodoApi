namespace TodoApp.Application.DTOs
{
    public class UpdateTodoItemDto
    {
        // O Name e IsComplete são opcionais (nullable) para permitir operações PATCH.
        // A lógica de tempo e a ID são ignoradas/passadas via URL.
        public string? Name { get; set; }
        public bool? IsComplete { get; set; }
    }
}