using Todo.Domain.Entities;
using Todo.Application.Services.Interfaces;
using Todo.Persistence.Repositories.Interfaces;

namespace Todo.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly IUnitOfWork _repository;

    public CategoriaService(IUnitOfWork repository)
    {
        _repository = repository;
    }

    public async Task<Categoria> Add(Categoria categoria)
    {
        try 
        {
            categoria.Ativa = true;
            _repository.CategoriaRepository.Add(categoria);
            if (await _repository.Commit()) return await _repository.CategoriaRepository.GetUmaCategoriaAsync(categoria.Id);
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
            var categoriaRepository = await _repository.CategoriaRepository.GetUmaCategoriaAsync(categoria.Id);
            if(categoriaRepository == null) throw new Exception("Categoria não encontrada");
            _repository.CategoriaRepository.Delete(categoriaRepository);
            return await _repository.Commit();
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
            var categoriaRepositoy = await _repository.CategoriaRepository.GetUmaCategoriaAsync(categoria.Id);
            if(categoriaRepositoy == null) throw new Exception("Categoria não encontrada");
            categoria.Ativa = false;
            _repository.CategoriaRepository.Update(categoria);
            return await _repository.Commit();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<Categoria[]> GetAllCategoriaAsync()
    {
        var categorias = await _repository.CategoriaRepository.GetAllCategoriaAsync();
        if (categorias == null) throw new Exception("Categoria não encontrada");
        return categorias;
    }

    public async Task<Categoria> GetUmaCategoriaAsync(int id)
    {
        var categoria = await _repository.CategoriaRepository.GetUmaCategoriaAsync(id);
        if (categoria == null) throw new Exception("Categoria não encontrada");
        return categoria;
    }

    public async Task<Categoria> Update(Categoria categoria)
    {
        try
        {
            var categoriaRepository = await _repository.CategoriaRepository.GetUmaCategoriaAsync(categoria.Id);
            if (categoriaRepository == null) throw new Exception("Categoria não encontrada");
            _repository.CategoriaRepository.Update(categoria);
            if(await _repository.Commit()) return categoria;
            throw new Exception("Erro ao salvar a atualizacao");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
