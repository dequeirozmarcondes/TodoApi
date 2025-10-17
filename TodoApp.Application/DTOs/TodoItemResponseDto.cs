namespace TodoApp.Application.DTOs
{
    public class TodoItemResponseDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
        public DateTime CreatedAt { get; set; }
        public TimeSpan? TimeSpent { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}