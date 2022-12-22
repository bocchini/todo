using Todo.Domain.Entities;
using Todo.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Todo.Persistence.Repositories.Interfaces;

namespace Todo.Persistence.Repositories;

public class CategoriaRepositoy : ICategoriaRepository
{
    private readonly TodoContext _context;

    public CategoriaRepositoy(TodoContext todoContext)
    {
        _context = todoContext;
    }

    public void Add<T>(T entity) where T : class
    {
        _context.Add(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
        _context.Remove(entity);
    }

    public void DeleteRange<T>(T entity) where T : class
    {
        _context.RemoveRange(entity);
    }

    public async Task<Categoria[]> GetAllCategoriaAsync()
    {
        IQueryable<Categoria> query = _context.Categorias;
        return await query
                        .Where(c=> c.Ativa == true)
                        .OrderBy(c => c.Nome)
                        .ToArrayAsync();
    }

    public async Task<Categoria> GetUmaCategoriaAsync(int id) => 
        await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

    public void Update<T>(T entity) where T : class
    {
        _context.Update(entity);
    }
}
