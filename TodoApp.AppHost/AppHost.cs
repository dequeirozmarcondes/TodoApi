var builder = DistributedApplication.CreateBuilder(args);

// 1. Atribui a referência do projeto à variável 'apiService'
var apiService = builder.AddProject<Projects.TodoApp>("todoapp");

// 2. Adiciona o servidor PostgreSQL (o container)
var postgres = builder.AddPostgres("postgres-server")
    // Imagem padrão do PostgreSQL
    .WithImage("postgres")
     // Opcional: para persistência de dados em dev, 
     // para que os dados não sumam a cada restart
     //.WithVolume("postgres-volume");
     .WithDataVolume("todoapp-volume");

// 3. Adiciona um banco de dados específico (TodoDb) ao servidor
var db = postgres.AddDatabase("TodoDb");

// 4. Vincula o projeto API ao recurso de banco de dados. 
// Isso injetará a Connection String necessária no projeto 'TodoAppAPI'
apiService.WithReference(db);

builder.Build().Run();
