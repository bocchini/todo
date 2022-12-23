using AutoMapper;
using Todo.Domain.Entities;

namespace Todo.Application.Dtos.Mappings;

public class MappingProfile: Profile
{
	public MappingProfile()
	{
		CreateMap<Categoria, CategoriaDto>().ReverseMap();
		CreateMap<Tarefa, TarefaDto>().ReverseMap();
	}
}
