using AutoMapper;
using Todo.Application;
using Todo.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Todo.Application.Services.Interfaces;

namespace Todo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _service;
        private readonly IMapper _mapper;

        public CategoriaController(ICategoriaService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var categorias = await _service.GetAllCategoriaAsync();
                if (categorias == null) return NotFound("Categorias não encontradas");
                return Ok(categorias);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var categoria = await _service.GetUmaCategoriaAsync(id);
                if (categoria == null) return NotFound("Nenhuma categoria encontrada");
                return Ok(categoria);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoriaDto categoriaDto)
        {
            try
            {
               var categoria = _mapper.Map<Categoria>(categoriaDto); 
                var categoriaAdicionada = await _service.Add(categoria);
                if (categoriaAdicionada == null) return NotFound("Erro ao adicionar a categoria");
                return Ok(categoriaAdicionada);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Categoria categoria)
        {
            try
            {
                var categoriaAtualizada = await _service.Update(categoria);
                if (categoriaAtualizada == null) return NotFound("Erro ao atualizar a categoria");
                return Ok(categoriaAtualizada);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Categoria categoria)
        {
            try
            {
                return (await _service.Delete(categoria) ? Ok("Categoria removida com sucesso") : NotFound("Erro ao remover a categoria"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
