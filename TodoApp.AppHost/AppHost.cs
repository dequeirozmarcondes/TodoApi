var builder = DistributedApplication.CreateBuilder(args);

// 1. Configuração do Servidor PostgreSQL (Container Único)
var postgres = builder.AddPostgres("postgres-server")
    // O .WithPgAdmin() é opcional, mas útil para visualizar os DBs
    .WithPgAdmin()
    // A imagem 'postgres' é a padrão, pode omitir, mas é bom para clareza
    // .WithImage("postgres")
    .WithDataVolume("todoapp-volume"); // Volume para persistência dos dados

// --- Configuração para o TodoApp (API Principal) ---

// 2. Adiciona um banco de dados específico (TodoDb) ao servidor
var todoDb = postgres.AddDatabase("TodoDb"); // Renomeado para 'todoDb' para clareza

// 3. Atribui a referência do projeto TodoApp à variável 'todoApiService'
var todoApiService = builder.AddProject<Projects.TodoApp>("todoapp");

// 4. Vincula o projeto TodoApp ao seu recurso de banco de dados (TodoDb).
// Isso injetará a Connection String 'ConnectionStrings:TodoDb' no TodoApp.
todoApiService.WithReference(todoDb);

// --- Configuração para o TodoAppIdentity (Serviço de Identidade) ---

// 5. Adiciona o banco de dados específico para o Identity Service (IdentityDb)
var identityDb = postgres.AddDatabase("IdentityDb"); // Armazena a referência

// 6. Adiciona a referência para o Identity Service
var identityApiService = builder.AddProject<Projects.TodoAppIdentity>("todoappidentity");

// 7. Conecta o Identity Service ao seu DB (IdentityDb).
// Isso injetará a Connection String 'ConnectionStrings:IdentityDb' no TodoAppIdentity.
identityApiService.WithReference(identityDb);

// 8. Vincula o TodoApp (API) ao Identity Service para descoberta de URL (Service Discovery)
// Isso injeta a URL base do Identity Service no TodoApp
todoApiService.WithReference(identityApiService);

builder.Build().Run();