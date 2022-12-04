using System.ComponentModel.DataAnnotations;

namespace Todo.Application;

public class CategoriaDto
{
    public int? Id { get; set; }

    public string Nome { get; set; }

    public bool? Ativa { get; set; }
}
