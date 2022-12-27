using Todo.Persistence.Context;
using Todo.Persistence.Repositories.Interfaces;

namespace Todo.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private TodoContext _context;
    private CategoriaRepositoy _categoriaRepositoy;
    private TarefaRepository _tarefaRepository;

    public UnitOfWork(TodoContext context)
    {
        _context = context;
    }

    public ICategoriaRepository CategoriaRepository
    {
        get
        {
            return _categoriaRepositoy = _categoriaRepositoy ?? new CategoriaRepositoy(_context);
        }
    }

    public ITarefaRepository TarefaRepository
    {
        get
        {
return  _tarefaRepository = _tarefaRepository ?? new TarefaRepository(_context);
        }
    }

    public async Task<bool> Commit () => await _context.SaveChangesAsync() > 0;

    public void Dispose()
    {
        _context.Dispose();
    }
}
