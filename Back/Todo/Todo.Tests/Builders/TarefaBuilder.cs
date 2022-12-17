using AutoBogus;
using Todo.Domain.Entities;

namespace Todo.Tests.Builders;

public class TarefaBuilder:AutoFaker<Tarefa>
{

    public Tarefa GeraTarefa(bool comId =false)
    {
        return comId ? ComId().ComNome().ComEstaCompleta().ComDataCadastro(DateTime.Now.Year, DateTime.Now).ComCategoriaId().Generate() : ComNome().ComEstaCompleta().ComDataCadastro(DateTime.Now.Year, DateTime.Now).ComCategoriaId().Generate();
    }

    public TarefaBuilder ComId()
    {
        RuleFor(t => t.Id, ta => ta.Random.Int(0, 100));
        return this;
    }

    public TarefaBuilder ComNome()
    {
        RuleFor(t => t.Nome, ta => ta.Person.FirstName);
        return this;
    }

    public TarefaBuilder ComEstaCompleta()
    {
        RuleFor(t=> t.EstaCompleta, ta => ta.Random.Bool());
        return this;
    }

    public TarefaBuilder ComDataQueFoiCompleto(int year, DateTime? daysFuture)
    {
        RuleFor(t => t.DataQueFoiCompleto, ta => ta.Date.Future(year, daysFuture));
        return this;
    }

    public TarefaBuilder ComDataCadastro(int year, DateTime? daysFuture)
    {
        RuleFor(t => t.DataCadastro, ta => ta.Date.Future(year, daysFuture));
        return this;
    }

    public TarefaBuilder ComCategoriaId()
    {
        RuleFor(t => t.CategoriaId, ta => ta.Random.Int(0,100));
        return this;
    }
}
