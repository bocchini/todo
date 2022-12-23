using Todo.Domain.Entities;

namespace Todo.Application.Dtos;

public class TarefaDto
{

    public string Nome { get; set; }

    public bool? EstaCompleta { get; set; }

    public DateTime? DataQueFoiCompleto { get; set; }

    public DateTime DataCadastro { get; set; }

    public int CategoriaId { get; set; }

}
