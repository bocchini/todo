using NSubstitute;
using Todo.Application.Services;
using Todo.Persistence.Repositories.Interfaces;

namespace Todo.Tests.Application.Services;

public class CategoriaServiceTests
{
    private readonly CategoriaService _service;
    private readonly ICategoriaRepository _repository;

    public CategoriaServiceTests()
    {
        _repository = Substitute.For<ICategoriaRepository>();
        _service = new CategoriaService(_repository);
    }

    [Fact]
    public void Deve_Adicionar_Categoria()
    {

    }
}
