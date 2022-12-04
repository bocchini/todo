﻿using System.ComponentModel.DataAnnotations;

namespace Todo.Domain.Entities;

public class Categoria
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    public bool? Ativa { get; set; }
}
