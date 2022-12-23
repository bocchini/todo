using Bogus;
using AutoMapper;
using NSubstitute;
using FluentAssertions;
using Todo.Tests.Builders;
using Todo.Domain.Entities;
using Todo.API.Controllers;
using Todo.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FluentAssertions.Execution;
using Todo.Application.Services.Interfaces;

namespace Todo.Tests.API.Controllers;

public class TarefaControllerTests
{
    private readonly TarefaController _controller;
    private readonly ITarefaService _tarefaService;
    private readonly TarefaBuilder _builder;
    private readonly IMapper _mapper;
    private readonly Faker _faker;

    public TarefaControllerTests()
    {
        _tarefaService = Substitute.For<ITarefaService>();
        _mapper = Substitute.For<IMapper>();
        _builder = new TarefaBuilder();
        _faker = new Faker();
        _controller = new TarefaController(_tarefaService, _mapper);
    }

    [Fact]
    public async Task Get_Deve_Retornar_Uma_lista_de_Tarefas_PeloIdDaCategoria()
    {
        var categoriaId = _faker.Random.Int(0, 100);
        var tarefas = new List<Tarefa>();

        for (int i = 0; i < 10; i++)
        {
            tarefas.Add(new Tarefa()
            {
                CategoriaId = categoriaId,
                Nome = _faker.Person.FirstName,
                EstaCompleta = false,
                Id = _faker.Random.Int(0, 100)
            });
        }

        _tarefaService.Get(Arg.Any<int>()).Returns(tarefas.ToArray());

        var result = (OkObjectResult)await _controller.Get(categoriaId);

        using(new AssertionScope())
        {
            result.Value.Should().BeEquivalentTo(tarefas);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }      
    }

    [Fact]
    public async Task Deve_Retornar_NenhumaTarefaEncotrada_QuandoNaoEncotrarTarefa()
    {
        var categoriaId = _faker.Random.Int(0, 100);

        _tarefaService.Get(Arg.Any<int>()).Returns(Task.FromResult<Tarefa[]>(null));

        var result = (NotFoundObjectResult) await _controller.Get(categoriaId);

        using(new AssertionScope())
        {
            result.Value.Should().Be("Nenhuma tarefa encontrada");
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    public async Task Deve_Retrnar_UmaTarefa()
    {
        var tarefa = _builder.GeraTarefa(true);

        _tarefaService.GetById(Arg.Any<int>()).Returns(tarefa);

        var result = (OkObjectResult)await _controller.GetTarefa(tarefa.Id);

        using(new AssertionScope())
        {
            result.Value.Should().Be(tarefa);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }

    [Fact]
    public async Task Deve_Retornar_TarefaNaoEncontrada_Quando_NaoEncontrarOId()
    {
        var tarefaId = _faker.Random.Int(0, 100);
        _tarefaService.GetById(Arg.Any<int>()).Returns(Task.FromResult<Tarefa>(null));

        var result = (NotFoundObjectResult)await _controller.GetTarefa(tarefaId);

        using(new AssertionScope())
        {
            result.Value.Should().Be("Tarefa não encontrada");
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    public async Task Deve_AdicionarUmaTarefa()
    {
        var tarefa = _builder.GeraTarefa();
        _mapper.Map<Tarefa>(Arg.Any<TarefaDto>()).Returns(tarefa);
        tarefa.Id = _faker.Random.Int(0, 100);
        _tarefaService.Add(Arg.Any<Tarefa>()).Returns(tarefa);

        var result = (OkObjectResult)await _controller.Post(new TarefaDto());

        using(new AssertionScope())
        {
            result.Value.Should().Be(tarefa);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }

    [Fact]
    public async Task Deve_Retornar_ErroAoAdicionarUmaTarefa()
    {
        _mapper.Map<Tarefa>(Arg.Any<TarefaDto>).Returns(new Tarefa());
        _tarefaService.Add(Arg.Any<Tarefa>()).Returns(Task.FromResult<Tarefa>(null));

        var result = (NotFoundObjectResult) await _controller.Post(new TarefaDto());

        using(new AssertionScope())
        {
            result.Value.Should().Be("Erro ao adicionar uma tarefa");
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    public async Task Update_Deve_AtualizarUmaTarefa()
    {
        var tarefa = _builder.GeraTarefa(true);

        _tarefaService.GetById(Arg.Any<int>()).Returns(new Tarefa());
        _tarefaService.Update(Arg.Any<Tarefa>()).Returns(tarefa);

        var result = (OkObjectResult)await _controller.Update(tarefa);

        using(new AssertionScope())
        {
            result.Value.Should().Be(tarefa);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            _ = _tarefaService.Received().Update(tarefa);
        }
    }

    [Fact]
    public async Task Update_Deve_Retornar_NotFound_NaoConseguirSalvar()
    {
        _tarefaService.Update(Arg.Any<Tarefa>()).Returns(Task.FromResult<Tarefa>(null));

        var result = (NotFoundObjectResult)await _controller.Update(new Tarefa());

        using(new AssertionScope())
        {
            result.Value.Should().Be("Erro ao atualizar a tarefa");
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    public async Task Delete_Deve_Conseguir_Deletar_UmaTarefa()
    {
        _tarefaService.Delete(Arg.Any<Tarefa>()).Returns(true);

        var result = (OkObjectResult)await _controller.Delete(new Tarefa());

        using(new AssertionScope())
        {
            result.Value.Should().Be("Tarefa removida com sucesso");
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }

    [Fact]
    public async Task Delete_Deve_Retornar_Erro_QuandoNaoConseguirDeletar()
    {
        _tarefaService.Delete(Arg.Any<Tarefa>()).Returns(false);

        var result = (NotFoundObjectResult)await _controller.Delete(new Tarefa());

        using(new AssertionScope())
        {
            result.Value.Should().Be("Erro ao remover a tarefa");
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}
