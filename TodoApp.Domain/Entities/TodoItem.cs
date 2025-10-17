namespace TodoApp.Domain.Entities
{
    public class TodoItem
    {
        public TodoItem(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("O nome do item não pode ser vazio.");
            }

            Id = Guid.NewGuid();
            Name = name;
            IsComplete = false;

            // 1. Inicializa a data de criação
            CreatedAt = DateTime.UtcNow;
        }

        private TodoItem() { }

        public Guid Id { get; private set; }
        public string? Name { get; private set; }
        public bool IsComplete { get; private set; }

        // Novos Atributos de Tempo
        public DateTime CreatedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; } // Pode ser nulo se não estiver completa

        // 3. Propriedade Computada
        // Não é persistida no banco de dados (padrão)
        public TimeSpan? TimeSpent
        {
            get
            {
                // Se estiver completa, calcula a diferença entre o final e o início
                if (IsComplete && CompletedAt.HasValue)
                {
                    return CompletedAt.Value - CreatedAt;
                }
                return null;
            }
        }

        public void MarkAsComplete()
        {
            if (!IsComplete)
            {
                IsComplete = true;

                // 2. Registra a data de conclusão
                CompletedAt = DateTime.UtcNow;
            }
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("O nome não pode ser nulo.");
            }
            Name = newName;
        }
    }
}