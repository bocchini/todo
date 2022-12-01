using Todo.Domain.Entities;

namespace Todo.Persistence.Repositories.Interfaces;

public interface ICategoriaRepository :IBase
{
    Task<Categoria[]> GetAllCategoriaAsync();
    Task<Categoria> GetUmaCategoriaAsync(int id);
}
