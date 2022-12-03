using Microsoft.AspNetCore.Mvc;
using Todo.Application.Services.Interfaces;


namespace Todo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _service;

        public CategoriaController(ICategoriaService service)
        {
            _service = service;
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
            var categoria = await _service.GetUmaCategoriaAsync(id);
            if(categoria == null) return NotFound("Nenhuma categoria encontrada");
            return Ok(categoria);
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
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
