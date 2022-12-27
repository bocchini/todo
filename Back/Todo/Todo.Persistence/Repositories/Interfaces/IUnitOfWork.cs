namespace Todo.Persistence.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoriaRepository CategoriaRepository { get; }
        ITarefaRepository TarefaRepository { get; }
        Task<bool> Commit();
    }
}
