using Bogus;
using NSubstitute;
using Todo.Tests.Builders;
using Todo.Domain.Entities;
using Todo.Application.Services;
using Todo.Persistence.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace Todo.Tests.Application.Services;

public class TarefaServiceTests
{
    private readonly ITarefaRepository _repository;
    private readonly TarefaService _service;
    private readonly Faker _fake;
    private readonly TarefaBuilder _builder;

    public TarefaServiceTests()
    {
        _repository = Substitute.For<ITarefaRepository>();
        _fake = new Faker();
        _builder = new TarefaBuilder();
        _service = new TarefaService(_repository);
    }

    [Fact]
    public async Task Add_Deve_AdicionarUmaTarefa()
    {
        var tarefa = _builder.GeraTarefa(true);

        _repository.Add(Arg.Any<Tarefa>());
        _repository.SaveChangesAsync().Returns(true);
        _repository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(tarefa);

        var result = await _service.Add(tarefa);

        result.Should().BeEquivalentTo(tarefa);
    }

    [Fact]
    public async Task Add_Deve_RetornarNull_QuandoNaoConseguirSalvar()
    {
        var tarefa = _builder.GeraTarefa();
        _repository.Add(Arg.Any<Tarefa>());
        _repository.SaveChangesAsync().Returns(false);

        var result = await _service.Add(tarefa);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Add_Deve_RetornarException_QuandoSalvrRetornaException()
    {
        var tarefa = _builder.GeraTarefa();

        _repository.Add(Arg.Any<Tarefa>());
        _repository.SaveChangesAsync().Returns(true);
        _repository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(Task.FromException<Tarefa>(new Exception("")));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.Add(tarefa));

        result.Message.Should().BeEmpty();
    }

    [Fact]
    public async Task Delete_Deve_RemoverUmaTarefa()
    {
        var tarefa = _builder.GeraTarefa();

        _repository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(tarefa);
        _repository.Delete(Arg.Any<Tarefa>());
        _repository.SaveChangesAsync().Returns(true);

        var result = await _service.Delete(tarefa);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_Deve_RetornarExceptionQuandoNaoLocalizarATarefa()
    {
        var tarefa = new Tarefa();
        _repository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(Task.FromResult<Tarefa>(null));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.Delete(tarefa));

        result.Message.Should().Be("Tarefa não encontrada");
    }

    [Fact]
    public async Task Delete_Deve_RetornarExceptionQuando_GetUmaTarefaRetornarExceptionComErroaoBuscarTarefa()
    {
        var tarefa = new Tarefa();
        _repository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(Task.FromException<Tarefa>(new Exception("Erro ao buscar tarefa")));
        var result = await Assert.ThrowsAsync<Exception>(() => _service.Delete(tarefa));

        result.Message.Should().Be("Erro ao buscar tarefa");
    }

    [Fact]
    public async Task GetById_Deve_BuscarUmaTarefa()
    {
        var tarefa = _builder.GeraTarefa(true);

        _repository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(tarefa);

        var result = await _service.GetById(tarefa.Id);

        result.Should().BeEquivalentTo(tarefa);
    }

    [Fact]
    public async Task GetById_Deve_RetornarException_QuandoATarefaRetornarNull()
    {
        var id = _fake.Random.Int(0, 100);
        _repository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(Task.FromResult<Tarefa>(null));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.GetById(id));

        result.Message.Should().Be("Tarefa não encontrada");
    }

    [Fact]
    public async Task GetById_Deve_Retornar_QuandoGetUmaTarefaAsyncRetornarException()
    {
        var id = _fake.Random.Int(0, 100);
        _repository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(Task.FromException<Tarefa>(new Exception("Erro de conexao")));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.GetById(id));

        result.Message.Should().Be("Erro de conexao");
    }

    [Fact]
    public async Task Update_Deve_AtualizarUmaCategoria()
    {
        var tarefa = _builder.GeraTarefa(true);

        _repository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(tarefa);
        _repository.Update(Arg.Any<Tarefa>);
        _repository.SaveChangesAsync().Returns(true);

        var result = await _service.Update(tarefa);
        result.Should().BeEquivalentTo(tarefa);
    }

    [Fact]
    public async Task Update_Deve_RetornarException_QuandoATarefaRetornarNull()
    {
        var tarefa = _builder.GeraTarefa();
        _repository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(Task.FromResult<Tarefa>(null));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.Update(tarefa));

        result.Message.Should().Be("Tarefa não encontrada");
    }
}
