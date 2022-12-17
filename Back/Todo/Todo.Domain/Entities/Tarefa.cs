using System.ComponentModel.DataAnnotations;

namespace Todo.Domain.Entities;

public class Tarefa
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    public bool? EstaCompleta { get; set; }

    public DateTime? DataQueFoiCompleto { get; set; }

    public DateTime DataCadastro { get; set; }

    public int CategoriaId { get; set; }

    public Categoria? Categoria { get; set; }

}
