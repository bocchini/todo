using Todo.Domain.Entities;
using Todo.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Todo.Persistence.Repositories.Interfaces;

namespace Todo.Persistence.Repositories;

public class TarefaRepository : ITarefaRepository
{
    private readonly TodoContext _context;

    public TarefaRepository(TodoContext context)
    {
        this._context = context;
        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
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

    public async Task<Tarefa[]> GetTarefasIdCategoriaAsync(int categoriaId)
    {
        IQueryable<Tarefa> query = _context.Tarefas;
        return await query.Where(t => t.CategoriaId == categoriaId && t.EstaCompleta == false)
            .Include(c => c.Categoria)
            .OrderBy(t => t.Nome)
            .ToArrayAsync();
    }

    public async Task<Tarefa> GetUmaTarefaAsync(int id)
   => await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == id);

    public async Task<bool> SaveChangesAsync()
     =>
      await _context.SaveChangesAsync() > 0;
    

    public void Update<T>(T entity) where T : class
    {
       _context.Update(entity);
    }
}
