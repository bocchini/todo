using Todo.Domain.Entities;

namespace Todo.Persistence.Repositories.Interfaces;

public interface ITarefaRepository : IBase
{
  Task<Tarefa> GetUmaTarefaAsync(int id);
    Task<Tarefa[]> GetTarefasIdCategoriaAsync(int categoriaId);
}
