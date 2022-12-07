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

    }
}
