using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Todo.Domain.Entities;

public class Tarefa
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public bool? EstaCompleta { get; set; }

    public DateTime? DataQueFoiCompleto { get; set; }

    public DateTime DataCadastro { get; set; }

    public int CategoriaId { get; set; }

    [JsonIgnore]
    public Categoria? Categoria { get; set; }

}
