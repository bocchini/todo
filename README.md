# todo

App de coisas a fazer

Adicionar as migrations
dotnet ef migrations add inicial -p .\Todo.Persistence\ -s .\Todo.API\

Criar o banco
dotnet ef database update -p .\Todo.Persistence\ -s .\Todo.Api
