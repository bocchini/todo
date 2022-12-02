using AutoBogus;
using Todo.Domain.Entities;

namespace Todo.Tests.Builders;

public class CategoriaBuilder : AutoFaker<Categoria>
{
    public Categoria GeraCategoria(bool comId = false)
    {
        return comId ? 
            ComId().ComNome().ComEstaAtiva().Generate() :
            ComNome().ComEstaAtiva().Generate();
    }
    public CategoriaBuilder ComId()
    {
        RuleFor(c => c.Id, ca => ca.Random.Int(0, 100));
        return this;
    }

    public CategoriaBuilder ComNome()
    {
        RuleFor(c => c.Nome, ca => ca.Name.FirstName());
        return this;
    }

    public CategoriaBuilder ComEstaAtiva()
    {
        RuleFor(c => c.Ativa, ca => ca.Random.Bool());
        return this;
    }
}
