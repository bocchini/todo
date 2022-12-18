using AutoMapper;
using NSubstitute;
using Todo.API.Controllers;
using Todo.Application.Services.Interfaces;
using Todo.Tests.Builders;

namespace Todo.Tests.API.Controllers;

public class TarefaControllerTests
{
    private readonly TarefaController _controller;
    private readonly ITarefaService _tarefaService;
    private readonly TarefaBuilder _builder;
    private readonly IMapper _mapper;

    public TarefaControllerTests()
    {
        _tarefaService = Substitute.For<ITarefaService>();
        _mapper = Substitute.For<IMapper>();
        _builder = new TarefaBuilder();
        _controller = new TarefaController(_tarefaService, _mapper);
    }
}
