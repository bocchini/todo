using Bogus;
using FluentAssertions;
using NSubstitute;
using Todo.Application.Services;
using Todo.Domain.Entities;
using Todo.Persistence.Repositories.Interfaces;
using Todo.Tests.Builders;

namespace Todo.Tests.Application.Services;

public class CategoriaServiceTests
{
    private readonly CategoriaService _service;
    private readonly ICategoriaRepository _repository;
    private readonly CategoriaBuilder _categoriaBuilder;
    private readonly Faker _faker;

    public CategoriaServiceTests()
    {
        _repository = Substitute.For<ICategoriaRepository>();
        _service = new CategoriaService(_repository);
        _categoriaBuilder = new CategoriaBuilder();
        _faker = new Faker();
    }

    [Fact]
    public async Task Add_Deve_Adicionar_CategoriaAsync()
    {
        var categoria = _categoriaBuilder.GeraCategoria();
        _repository.Add(categoria);

        _repository.SaveChangesAsync().Returns(true);
        categoria.Id = _faker.Random.Int(0, 100);

        _repository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(categoria);

        var result = await _service.Add(categoria);

        result.Should().BeEquivalentTo(categoria);
    }

    [Fact]
    public async Task Add_Deve_Retornar_Null_ErroAoSalvar()
    {
        var categoria = new Categoria();
        
        _repository.Add(categoria);
        _repository.SaveChangesAsync().Returns(false);

        var result = await _service.Add(categoria);
        result.Should().BeNull();
    }

    [Fact]
    public async Task Delete_Deve_Remover_UmaCategoria_ComSucesso()
    {
        var categoria = _categoriaBuilder.GeraCategoria(true);

        _repository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(categoria);
        _repository.Delete(categoria);
        _repository.SaveChangesAsync().Returns(true);

        var result = await _service.Delete(categoria);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_Deve_DarErro_QuandoCategoriaNaoEEncontrada()
    {
        var categoria = new Categoria();

        _repository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(Task.FromResult<Categoria>(null));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.Delete(categoria));
     
        result.Message.Should().Be("Categoria não encontrada");
    }

    [Fact]
    public async Task Delete_Deve_RetornarFalse_QaundoNaoConseguirSalvar()
    {
        var categoria = new Categoria();

        _repository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(categoria);
        _repository.Delete(categoria);
        _repository.SaveChangesAsync().Returns(false);

        var result = await _service.Delete(categoria);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Desativar_Deve_Desativa_Categoria()
    {
        var categoria = _categoriaBuilder.GeraCategoria(true);

        _repository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(categoria);
        _repository.Update(categoria);
        _repository.SaveChangesAsync().Returns(true);

        var result = await _service.Desativar(categoria);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Desativar_Deve_RetornarErro_SeCategoriaNaoEncontrada()
    {
        var categoria = new Categoria();

        _repository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(Task.FromResult<Categoria>(null));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.Desativar(categoria));

        result.Message.Should().Be("Categoria não encontrada");
    }
}
