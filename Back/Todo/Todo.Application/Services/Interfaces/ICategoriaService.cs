using Todo.Domain.Entities;

namespace Todo.Application.Services.Interfaces;

public interface ICategoriaService
{
    Task<Categoria> Add(Categoria categoria);
    Task<Categoria> Update(Categoria categoria);
    Task<Categoria> GetUmaCategoriaAsync(int id);
    Task<Categoria[]> GetAllCategoriaAsync();
    Task<bool> Delete(Categoria categoria);
    Task<bool> DeleteRange(Categoria categoria);
    Task<bool> Desativar(Categoria categoria);
    
}
