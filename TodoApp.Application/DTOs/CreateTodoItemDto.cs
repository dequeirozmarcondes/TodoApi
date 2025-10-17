namespace TodoApp.Application.DTOs
{
    public class CreateTodoItemDto
    {
        // Name e IsComplete são as únicas informações necessárias para a criação.
        // O ID, CreatedAt e CompletedAt são gerenciados pela Entidade/Domínio.
        public string Name { get; set; } = string.Empty;
        public bool IsComplete { get; set; } = false;
    }
}