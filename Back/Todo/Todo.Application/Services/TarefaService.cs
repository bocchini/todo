using Todo.Application.Services.Interfaces;
using Todo.Domain.Entities;
using Todo.Persistence.Repositories.Interfaces;

namespace Todo.Application.Services;

public class TarefaService : ITarefaService
{
    private readonly IUnitOfWork _repositorio;

    public TarefaService(IUnitOfWork repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<Tarefa> Add(Tarefa tarefa)
    {
        try
        {
            _repositorio.TarefaRepository.Add(tarefa);
            if(await _repositorio.Commit()) return await _repositorio.TarefaRepository.GetUmaTarefaAsync(tarefa.Id);
            return null;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> Delete(Tarefa tarefa)
    {
        try
        {
            var tarefaRepositorio = await _repositorio.TarefaRepository.GetUmaTarefaAsync(tarefa.Id);
            if (tarefaRepositorio == null) throw new Exception("Tarefa não encontrada");
            _repositorio.TarefaRepository.Delete(tarefa);
            return await _repositorio.Commit();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }            
    }

    public async Task<Tarefa[]> Get(int categoriaId)
    {
        try
        {
            var categorias = await _repositorio.TarefaRepository.GetTarefasIdCategoriaAsync(categoriaId);
            if (categorias == null) throw new Exception("Nenhuma tarefa encontrada nesta categoria");
            return categorias;
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<Tarefa> GetById(int id)
    {
        try
        {
            var tarefa = await _repositorio.TarefaRepository.GetUmaTarefaAsync(id);
            if (tarefa == null) throw new Exception("Tarefa não encontrada");
            return tarefa;
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<Tarefa> Update(Tarefa tarefa)
    {
        try
        {
            var tarefaRepositorio = await _repositorio.TarefaRepository.GetUmaTarefaAsync(tarefa.Id);
            if (tarefaRepositorio == null) throw new Exception("Tarefa não encontrada");
            _repositorio.TarefaRepository.Update(tarefa);
            if (await _repositorio.Commit()) return tarefa;
            throw new Exception("Erro ao salvar a atualização");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
