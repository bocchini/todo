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
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Interno server error");
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
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Interno server error");
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
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Interno server error");
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
