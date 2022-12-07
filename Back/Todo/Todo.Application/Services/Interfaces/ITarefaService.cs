using Todo.Domain.Entities;

namespace Todo.Application.Services.Interfaces;

public interface ITarefaService
{
    Task<Tarefa> Add (Tarefa tarefa);
    Task<Tarefa> Update (Tarefa tarefa);
    Task<bool> Delete (Tarefa tarefa);
    Task<Tarefa[]> Get(int categoriaId);
    Task<Tarefa> GetById(int id);
}
