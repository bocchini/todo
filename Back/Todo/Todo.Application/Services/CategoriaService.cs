using Todo.Domain.Entities;
using Todo.Application.Services.Interfaces;
using Todo.Persistence.Repositories.Interfaces;
using Todo.Persistence.Repositories;

namespace Todo.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _repository;

    public CategoriaService(ICategoriaRepository repository)
    {
        _repository = repository;
    }

    public async Task<Categoria> Add(Categoria categoria)
    {
        try 
        { 
            _repository.Add(categoria);
            if (await _repository.SaveChangesAsync()) return await _repository.GetUmaCategoriaAsync(categoria.Id);
            return null;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> Delete(Categoria categoria)
    {
        try
        {
            var categoriaRepository = await _repository.GetUmaCategoriaAsync(categoria.Id);
            if(categoriaRepository == null) throw new Exception("Categoria não encontrada");
            _repository.Delete(categoriaRepository);
            return await _repository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Task<bool> DeleteRange(Categoria categoria)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Desativar(Categoria categoria)
    {
        try
        {
            var categoriaRepositoy = await _repository.GetUmaCategoriaAsync(categoria.Id);
            if(categoriaRepositoy == null) throw new Exception("Categoria não encontrada");
            categoria.Ativa = false;
            _repository.Update(categoria);
            return await _repository.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<Categoria[]> GetAllCategoriaAsync()
    {
        var categorias = await _repository.GetAllCategoriaAsync();
        if (categorias == null) throw new Exception("Categoria não encontrada");
        return categorias;
    }

    public async Task<Categoria> GetUmaCategoriaAsync(int id)
    {
        var categoria = await _repository.GetUmaCategoriaAsync(id);
        if (categoria == null) throw new Exception("Categoria não encontrada");
        return categoria;
    }

    public async Task<Categoria> Update(Categoria categoria)
    {
        try
        {
            var categoriaRepository = await _repository.GetUmaCategoriaAsync(categoria.Id);
            if (categoriaRepository == null) throw new Exception("Categoria não encontrada");
            _repository.Update(categoria);
            if(await _repository.SaveChangesAsync()) return categoria;
            throw new Exception("Erro ao salvar a atualizacao");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
