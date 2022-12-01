using Microsoft.EntityFrameworkCore;

namespace Todo.Persistence.Context;

public  class TodoContext : DbContext
{
	public TodoContext(DbContextOptions<TodoContext> options) : base(options)
	{}
}
