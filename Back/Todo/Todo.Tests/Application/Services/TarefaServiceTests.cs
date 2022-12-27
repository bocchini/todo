using Bogus;
using FluentAssertions;
using NSubstitute;
using Todo.Application.Services;
using Todo.Domain.Entities;
using Todo.Persistence.Repositories.Interfaces;
using Todo.Tests.Builders;

namespace Todo.Tests.Application.Services;

public class TarefaServiceTests
{
    private readonly IUnitOfWork _repository;
    private readonly TarefaService _service;
    private readonly Faker _fake;
    private readonly TarefaBuilder _builder;

    public TarefaServiceTests()
    {
        _repository = Substitute.For<IUnitOfWork>();
        _fake = new Faker();
        _builder = new TarefaBuilder();
        _service = new TarefaService(_repository);
    }

    [Fact]
    public async Task Add_Deve_AdicionarUmaTarefa()
    {
        var tarefa = _builder.GeraTarefa(true);

        _repository.TarefaRepository.Add(Arg.Any<Tarefa>());
        _repository.Commit().Returns(true);
        _repository.TarefaRepository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(tarefa);

        var result = await _service.Add(tarefa);

        result.Should().BeEquivalentTo(tarefa);
    }

    [Fact]
    public async Task Add_Deve_RetornarNull_QuandoNaoConseguirSalvar()
    {
        var tarefa = _builder.GeraTarefa();
        _repository.TarefaRepository.Add(Arg.Any<Tarefa>());
        _repository.Commit().Returns(false);

        var result = await _service.Add(tarefa);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Add_Deve_RetornarException_QuandoSalvrRetornaException()
    {
        var tarefa = _builder.GeraTarefa();

        _repository.TarefaRepository.Add(Arg.Any<Tarefa>());
        _repository.Commit().Returns(true);
        _repository.TarefaRepository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(Task.FromException<Tarefa>(new Exception("")));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.Add(tarefa));

        result.Message.Should().BeEmpty();
    }

    [Fact]
    public async Task Delete_Deve_RemoverUmaTarefa()
    {
        var tarefa = _builder.GeraTarefa();

        _repository.TarefaRepository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(tarefa);
        _repository.TarefaRepository.Delete(Arg.Any<Tarefa>());
        _repository.Commit().Returns(true);

        var result = await _service.Delete(tarefa);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_Deve_RetornarExceptionQuandoNaoLocalizarATarefa()
    {
        var tarefa = new Tarefa();
        _repository.TarefaRepository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(Task.FromResult<Tarefa>(null));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.Delete(tarefa));

        result.Message.Should().Be("Tarefa não encontrada");
    }

    [Fact]
    public async Task Delete_Deve_RetornarExceptionQuando_GetUmaTarefaRetornarExceptionComErroaoBuscarTarefa()
    {
        var tarefa = new Tarefa();
        _repository.TarefaRepository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(Task.FromException<Tarefa>(new Exception("Erro ao buscar tarefa")));
        var result = await Assert.ThrowsAsync<Exception>(() => _service.Delete(tarefa));

        result.Message.Should().Be("Erro ao buscar tarefa");
    }

    [Fact]
    public async Task GetById_Deve_BuscarUmaTarefa()
    {
        var tarefa = _builder.GeraTarefa(true);

        _repository.TarefaRepository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(tarefa);

        var result = await _service.GetById(tarefa.Id);

        result.Should().BeEquivalentTo(tarefa);
    }

    [Fact]
    public async Task GetById_Deve_RetornarException_QuandoATarefaRetornarNull()
    {
        var id = _fake.Random.Int(0, 100);
        _repository.TarefaRepository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(Task.FromResult<Tarefa>(null));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.GetById(id));

        result.Message.Should().Be("Tarefa não encontrada");
    }

    [Fact]
    public async Task GetById_Deve_Retornar_QuandoGetUmaTarefaAsyncRetornarException()
    {
        var id = _fake.Random.Int(0, 100);
        _repository.TarefaRepository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(Task.FromException<Tarefa>(new Exception("Erro de conexao")));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.GetById(id));

        result.Message.Should().Be("Erro de conexao");
    }

    [Fact]
    public async Task Update_Deve_AtualizarUmaCategoria()
    {
        var tarefa = _builder.GeraTarefa(true);

        _repository.TarefaRepository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(tarefa);
        _repository.TarefaRepository.Update(Arg.Any<Tarefa>);
        _repository.Commit().Returns(true);

        var result = await _service.Update(tarefa);
        result.Should().BeEquivalentTo(tarefa);
    }

    [Fact]
    public async Task Update_Deve_RetornarException_QuandoATarefaRetornarNull()
    {
        var tarefa = _builder.GeraTarefa();
        _repository.TarefaRepository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(Task.FromResult<Tarefa>(null));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.Update(tarefa));

        result.Message.Should().Be("Tarefa não encontrada");
    }

    [Fact]
    public async Task Update_Deve_Retornar_ExceptionQuandoNaoConseguirSalvar()
    {
        var tarefa = _builder.GeraTarefa(true);

        _repository.TarefaRepository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(tarefa);
        _repository.TarefaRepository.Update(Arg.Any<Tarefa>());
        _repository.Commit().Returns(false);

        var result = await Assert.ThrowsAsync<Exception>(() => _service.Update(tarefa));

        result.Message.Should().Be("Erro ao salvar a atualização");
    }

    [Fact]
    public async Task Update_Deve_Retornar_ExceptionQuandotiverErroAoBuscar()
    {
        var tarefa = _builder.GeraTarefa(true);

        _repository.TarefaRepository.GetUmaTarefaAsync(Arg.Any<int>()).Returns(Task.FromException<Tarefa>(new Exception("Erro ao buscar")));

        var result = await Assert.ThrowsAsync<Exception>(() => _service.Update(tarefa));

        result.Message.Should().Be("Erro ao buscar");
    }
}
