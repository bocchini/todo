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
    private readonly IUnitOfWork _repository;
    private readonly CategoriaBuilder _categoriaBuilder;
    private readonly Faker _faker;

    public CategoriaServiceTests()
    {
        _repository = Substitute.For<IUnitOfWork>();
        _service = new CategoriaService(_repository);
        _categoriaBuilder = new CategoriaBuilder();
        _faker = new Faker();
    }

    [Fact]
    public async Task Add_Deve_Adicionar_CategoriaAsync()
    {
        var categoria = _categoriaBuilder.GeraCategoria();
        _repository.CategoriaRepository.Add(categoria);

        _repository.Commit().Returns(true);
        categoria.Id = _faker.Random.Int(0, 100);

        _repository.CategoriaRepository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(categoria);

        var result = await _service.Add(categoria);

        result.Should().BeEquivalentTo(categoria);
    }

    [Fact]
    public async Task Add_Deve_Retornar_Null_ErroAoSalvar()
    {
        var categoria = new Categoria();

        _repository.CategoriaRepository.Add(categoria);
        _repository.Commit().Returns(false);

        var result = await _service.Add(categoria);
        result.Should().BeNull();
    }

    [Fact]
    public async Task Delete_Deve_Remover_UmaCategoria_ComSucesso()
    {
        var categoria = _categoriaBuilder.GeraCategoria(true);

        _repository.CategoriaRepository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(categoria);
        _repository.CategoriaRepository.Delete(categoria);
        _repository.Commit().Returns(true);

        var result = await _service.Delete(categoria);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_Deve_DarErro_QuandoCategoriaNaoEEncontrada()
    {
        var categoria = new Categoria();

        _repository.CategoriaRepository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(Task.FromResult<Categoria>(null));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.Delete(categoria));

        result.Message.Should().Be("Categoria não encontrada");
    }

    [Fact]
    public async Task Delete_Deve_RetornarFalse_QuandoNaoConseguirSalvar()
    {
        var categoria = new Categoria();

        _repository.CategoriaRepository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(categoria);
        _repository.CategoriaRepository.Delete(categoria);
        _repository.Commit().Returns(false);

        var result = await _service.Delete(categoria);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Desativar_Deve_Desativa_Categoria()
    {
        var categoria = _categoriaBuilder.GeraCategoria(true);

        _repository.CategoriaRepository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(categoria);
        _repository.CategoriaRepository.Update(categoria);
        _repository.Commit().Returns(true);

        var result = await _service.Desativar(categoria);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Desativar_Deve_RetornarErro_SeCategoriaNaoEncontrada()
    {
        var categoria = new Categoria();

        _repository.CategoriaRepository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(Task.FromResult<Categoria>(null));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.Desativar(categoria));

        result.Message.Should().Be("Categoria não encontrada");
    }

    [Fact]
    public async Task Desativa_Deve_RetornarFalse_QaundoNaoConseguirSalvar()
    {
        var categoria = new Categoria();

        _repository.CategoriaRepository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(categoria);
        _repository.CategoriaRepository.Update(categoria);
        _repository.Commit().Returns(false);

        var result = await _service.Desativar(categoria);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllCategoriaAsync_Deve_RetornarUmaLista()
    {
        var categorias = new List<Categoria>();

        for (int i = 0; i < 10; i++)
        {
            categorias.Add(_categoriaBuilder.GeraCategoria(true));
        }

        _repository.CategoriaRepository.GetAllCategoriaAsync().Returns(categorias.ToArray());
         var result = await _service.GetAllCategoriaAsync();
        result.Should().BeEquivalentTo(categorias);
    }

    [Fact]
    public async Task GetAllCategoriaAsync_Deve_RetornarNenhumaCategoriaEncontrada()
    {
        _repository.CategoriaRepository.GetAllCategoriaAsync().Returns(Task.FromResult<Categoria[]>(null));
        var result = await Assert.ThrowsAsync<Exception>(() => _service.GetAllCategoriaAsync());

        result.Message.Should().Be("Categoria não encontrada");
    }

    [Fact]
    public async Task GetUmaCategoria_DeveRetornar_UmaCategoria()
    {
        var categoria = _categoriaBuilder.GeraCategoria(true);

        _repository.CategoriaRepository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(categoria);

        var result = await _service.GetUmaCategoriaAsync(categoria.Id);

        result.Should().BeEquivalentTo(categoria);
    }

    [Fact]
    public async Task GetUmaCategoriaAsync_DeveRetornar_CategoriaNaoEncontrada()
    {
        var id = _faker.Random.Int();

        _repository.CategoriaRepository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(Task.FromResult<Categoria>(null));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.GetUmaCategoriaAsync(id));

        result.Message.Should().Be("Categoria não encontrada");
    }

    [Fact]
    public async Task Update_Deve_AtualizarUmaCategoria()
    {
        var categoria = _categoriaBuilder.GeraCategoria(true);

        var categoriaAtualizada = new Categoria
        {
            Id = categoria.Id,
            Nome = _faker.Person.FirstName,
            Ativa = categoria.Ativa
        };

        _repository.CategoriaRepository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(categoria);

        _repository.CategoriaRepository.Update(categoriaAtualizada);
        _repository.Commit().Returns(true);
        var result = await _service.Update(categoriaAtualizada);

        result.Should().BeEquivalentTo(categoriaAtualizada);
    }

    [Fact]
    public async Task Update_Deve_RetornarCategoriaNaoEncontrada()
    {
        _repository.CategoriaRepository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(Task.FromResult<Categoria>(null));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.Update(new Categoria()));

        result.Message.Should().Be("Categoria não encontrada");
    }

    [Fact]
    public async Task Update_Deve_RetornarErroAoSalvarAtualizacao()
    {
        var categoria = _categoriaBuilder.GeraCategoria();

        _repository.CategoriaRepository.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(categoria);
        _repository.CategoriaRepository.Update(categoria);
        _repository.Commit().Returns(false);

        var result = await Assert.ThrowsAsync<Exception>(() => _service.Update(categoria));

        result.Message.Should().Be("Erro ao salvar a atualizacao");
    }
}
