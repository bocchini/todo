using Bogus;
using AutoMapper;
using NSubstitute;
using Todo.Application;
using FluentAssertions;
using Todo.Tests.Builders;
using Todo.API.Controllers;
using Todo.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FluentAssertions.Execution;
using Todo.Application.Services.Interfaces;

namespace Todo.Tests.API.Controllers;

public class CategoriaControllerTests
{
    private readonly ICategoriaService _service;
    private readonly CategoriaController _controller;
    private readonly CategoriaBuilder _builder;
    private readonly Faker _faker;
    private readonly IMapper _mapper;

    public CategoriaControllerTests()
    {
        _service = Substitute.For<ICategoriaService>();
        _mapper = Substitute.For<IMapper>();
        _controller = new CategoriaController(_service, _mapper);
        _builder = new CategoriaBuilder();
        _faker = new Faker();
    }

    [Fact]
    public async Task Get_DeveRetorar_VariasCategorias_ComStatusResult200()
    {
        var categorias = new List<Categoria>();
        var quantiadeCategorias = _faker.Random.Int(0, 100);

        for (int i = 0; i < quantiadeCategorias; i++)
        {
            categorias.Add(_builder.GeraCategoria(true));
        }
        _service.GetAllCategoriaAsync().Returns(categorias.ToArray());
        var result = (OkObjectResult)await _controller.Get();

        using (new AssertionScope())
        {
            result.Value.Should().BeEquivalentTo(categorias);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);

        }
    }

    [Fact]
    public async Task Get_DeveRetornar_NuloEStatosCodeNaoFound()
    {
        _service.GetAllCategoriaAsync().Returns(Task.FromResult<Categoria[]>(null));
        var result = (NotFoundObjectResult)await _controller.Get();
        using (new AssertionScope())
        {
            result.Value.Should().Be("Categorias não encontradas");
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    public async Task Get_Deve_RetornarExceprion()
    {
        _service.GetAllCategoriaAsync().Returns(Task.FromException<Categoria[]>(new Exception("")));
        var result = (ObjectResult)await _controller.Get();

        using (new AssertionScope())
        {
            result.Value.Should().Be("Interno server error");
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }

    [Fact]
    public async Task GET_Id_Deve_Retornar_UmaCategoria_EstatusConde200()
    {
        var categoria = _builder.GeraCategoria(true);

        _service.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(categoria);

        var result = (OkObjectResult)await _controller.Get(categoria.Id);

        using (new AssertionScope())
        {
            result.Value.Should().BeEquivalentTo(categoria);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }

    [Fact]
    public async Task Get_Id_Deve_Retornar_NulloEStatusCodeNotFound()
    {
        var id = _faker.Random.Int(0, 100);
        _service.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(Task.FromResult<Categoria>(null));

        var result = (NotFoundObjectResult)await _controller.Get(id);

        using (new AssertionScope())
        {
            result.Value.Should().Be("Nenhuma categoria encontrada");
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    public async Task Get_Id_Deve_Retornar_ExceptionStatusCode500()
    {
        var id = _faker.Random.Int(0, 100);
        _service.GetUmaCategoriaAsync(Arg.Any<int>()).Returns(Task.FromException<Categoria>(new Exception("")));

        var result = (ObjectResult)await _controller.Get(id);

        using (new AssertionScope())
        {
            result.Value.Should().Be("Interno server error");
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }

    [Fact]
    public async Task POST_Deve_AdicionarUmaCategoria_RetornarStatusCoe200ECategoriaAdicionada()
    {
        var categoriaDto = new CategoriaDto()
        {
            Nome = _faker.Person.FirstName
        };

        var categoria = new Categoria()
        {
            Nome = categoriaDto.Nome,
        };

        var categoriaResult = new Categoria()
        {
            Id = _faker.Random.Int(0, 100),
            Nome = categoria.Nome,
            Ativa = true,
        };

        _mapper.Map<Categoria>(Arg.Any<CategoriaDto>()).Returns(categoria);

        _service.Add(Arg.Any<Categoria>()).Returns(categoriaResult);

        var result = (OkObjectResult)await _controller.Post(categoriaDto);

        using (new AssertionScope())
        {
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            _service.ReceivedWithAnyArgs().Add(categoria);
            result.Value.Should().BeEquivalentTo(categoriaResult);
        }
    }

    [Fact]
    public async Task Add_Deve_Retornar_ErroAoAdicionarCategoria()
    {
        var categoriaDto = new CategoriaDto()
        {
            Nome = _faker.Name.FirstName()
        };
        var categoria = new Categoria()
        {
            Nome = categoriaDto.Nome
        };

        _service.Add(categoria).Returns(Task.FromResult<Categoria>(null));

        var result = (NotFoundObjectResult)await _controller.Post(categoriaDto);

        using(new AssertionScope())
        {
            result.Value.Should().Be("Erro ao adicionar a categoria");
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }

    [Fact]
    public async Task Post_Deve_RetornarStatusCode500InternoServerError()
    {
        _mapper.Map<Categoria>(Arg.Any<Categoria>()).Returns(new Categoria());
        _service.Add(Arg.Any<Categoria>()).Returns(Task.FromException<Categoria>(new Exception("")));
        var result = (ObjectResult)await _controller.Post(new CategoriaDto());

        using(new AssertionScope())
        {
            result.Value.Should().Be("Interno server error");
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
