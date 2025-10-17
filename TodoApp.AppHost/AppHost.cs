var builder = DistributedApplication.CreateBuilder(args);

// 1. Atribui a refer�ncia do projeto � vari�vel 'apiService'
var apiService = builder.AddProject<Projects.TodoApp>("todoapp");

// 2. Adiciona o servidor PostgreSQL (o container)
var postgres = builder.AddPostgres("postgres-server")
    // Imagem padr�o do PostgreSQL
    .WithImage("postgres")
     // Opcional: para persist�ncia de dados em dev, 
     // para que os dados n�o sumam a cada restart
     //.WithVolume("postgres-volume");
     .WithDataVolume("todoapp-volume");

// 3. Adiciona um banco de dados espec�fico (TodoDb) ao servidor
var db = postgres.AddDatabase("TodoDb");

// 4. Vincula o projeto API ao recurso de banco de dados. 
// Isso injetar� a Connection String necess�ria no projeto 'TodoAppAPI'
apiService.WithReference(db);

builder.Build().Run();
