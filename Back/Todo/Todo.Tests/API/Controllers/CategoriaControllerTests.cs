using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;
using Todo.Tests.Builders;
using Todo.Domain.Entities;
using Todo.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.Services.Interfaces;
using Bogus;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;

namespace Todo.Tests.API.Controllers;

public class CategoriaControllerTests
{
    private readonly ICategoriaService _service;
    private readonly CategoriaController _controller;
    private readonly CategoriaBuilder _builder;
    private readonly Faker _faker;

    public CategoriaControllerTests()
    {
        _service = Substitute.For<ICategoriaService>();
        _controller = new CategoriaController(_service);
        _builder = new CategoriaBuilder();
        _faker = new Faker();
    }

    [Fact]
    public async Task Get_DeveRetorar_VariasCategorias_ComStatusResult200()
    {
        var categorias = new List<Categoria>();
        var quantiadeCategorias = _faker.Random.Int(0, 100);
        
        for(int i =0; i < quantiadeCategorias; i++)
        {
            categorias.Add(_builder.GeraCategoria(true));
        }
        _service.GetAllCategoriaAsync().Returns(categorias.ToArray());
        var result = (OkObjectResult) await _controller.Get();

        using (new AssertionScope())
        {
            result.Value.Should().BeEquivalentTo(categorias);
            result.StatusCode.Should().Be(200);

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
            result.StatusCode.Should().Be(404);
        }
    }

    [Fact]
    public async Task Get_Deve_RetornarExceprion()
    {
        _service.GetAllCategoriaAsync().Returns(Task.FromException<Categoria[]>(new Exception("")));
        var result = (ObjectResult)await _controller.Get();

        using(new AssertionScope())
        {
            result.Value.Should().Be("Interno server error");
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
