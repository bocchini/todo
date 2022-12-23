using AutoMapper;
using Todo.Domain.Entities;
using Todo.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Todo.Application.Services.Interfaces;

namespace Todo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;
        private readonly IMapper _mapper;

        public TarefaController(ITarefaService tarefaService, IMapper mapper)
        {
            _tarefaService = tarefaService;
            _mapper = mapper;
        }

        [HttpGet("categoriaId")]
        public async Task<IActionResult> Get(int categoriaId)
        {
            try
            {
                var tarefas = await _tarefaService.Get(categoriaId);
                if (tarefas is null) return NotFound("Nenhuma tarefa encontrada");
                return Ok(tarefas);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error");
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetTarefa(int id)
        {
            try
            {
                var tarefa= await _tarefaService.GetById(id);
                if (tarefa is null) return NotFound("Tarefa não encontrada");
                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(TarefaDto tarefaDto)
        {
            try
            {
                var tarefa = _mapper.Map<Tarefa>(tarefaDto);
                var tarefaAdicionada = await _tarefaService.Add(tarefa);
                if (tarefaAdicionada is null) return NotFound("Erro ao adicionar uma tarefa");
                return Ok(tarefaAdicionada);

            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Internal server error"); 
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Tarefa tarefa)
        {
            try
            {
                if (await _tarefaService.GetById(tarefa.Id) is null) return NotFound("Tarefa não localizada");
                var tarefaAtualizada = await _tarefaService.Update(tarefa);
                if (tarefaAtualizada is null) return NotFound("Erro ao atualizar a tarefa");
                return Ok(tarefaAtualizada);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Tarefa tarefa)
        {
            try
            {
                return (await _tarefaService.Delete(tarefa) ? Ok("Tarefa removida com sucesso") : NotFound("Erro ao remover a tarefa"));
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
                throw;
            }
        }
    }
}
