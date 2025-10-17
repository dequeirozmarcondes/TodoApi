using Mapster;
using TodoApp.Domain.Entities;
using TodoApp.Application.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace TodoApp.Application.Mapping
{
    public static class MappingConfig
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            // Mapeamento da Entidade de Domínio para o DTO de Resposta da Aplicação
            // Mapeamento mantido, pois mapeia as novas propriedades de tempo e UUID.
            TypeAdapterConfig<TodoItem, TodoItemResponseDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.IsComplete, src => src.IsComplete)
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.CompletedAt, src => src.CompletedAt)
                .Map(dest => dest.TimeSpent, src => src.TimeSpent);

            // Mapster (DTO de Criação para Entidade)
            TypeAdapterConfig<CreateTodoItemDto, TodoItem>
                .NewConfig()
                // 🚨 CORREÇÃO: Usa ConstructUsing para chamar o construtor: public TodoItem(string name)
                // O src.Name é passado como argumento.
                .ConstructUsing(src => new TodoItem(src.Name))

                // Mapeia a propriedade IsComplete (que não é definida no construtor)
                .Map(dest => dest.IsComplete, src => src.IsComplete)

                // Removemos o mapeamento de Name, pois ele já foi definido no construtor.
                // O Mapster será mais eficiente ao fazer o mapeamento restante.
                .PreserveReference(true);
        }
    }
}