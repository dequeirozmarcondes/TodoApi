# 📝 README.md: TodoApp

## 🎯 Sobre o Projeto

Esta é uma aplicação distribuída (Microservices) que implementa uma API RESTful para gerenciamento de Tarefas _(Todo List)_. O projeto foi construído utilizando **ASP.NET Core**, **Entity Framework Core** e **PostgreSQL.**
A orquestração e o gerenciamento da solução são realizados através do **.NET Aspire**, garantindo que cada microsserviço funcione de forma independente e mantenha seu próprio contexto de banco de dados.
Além da Arquitetura de Microservices, o _TodoApp_ adota as boas práticas de _Clean Code_ e _Clean Architecture_, seguindo os princípios recomendados por **Robert C. Martin (Uncle Bob)**.

 🛠️ Tecnologias Utilizadas

| Tecnologia | Descrição |
| :--- | :--- |
| **Framework** | .NET 10 (ASP.NET Core Web API) |
| **Orquestração** | .NET Aspire (para desenvolvimento, *debugging* e orquestração de serviços) |
| **Banco de Dados** | PostgreSQL |
| **ORM** | Entity Framework Core |
| **Padrão** | RESTful API com uso de DTOs. |

---

## 🚀 Como Executar o Projeto

Graças ao .NET Aspire, a execução é extremamente simples, pois ele gerencia automaticamente a inicialização da API e do container do PostgreSQL.

### Pré-requisitos

1.  **[.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10)** (Versão 10.x ou superior)
2.  **[Visual Studio 2026 Insiders](https://visualstudio.microsoft.com/downloads/)** com a *workload* "Desenvolvimento do ASP.NET e web" instalada.
3.  **[Docker Desktop](https://www.docker.com/products/docker-desktop/)** (Necessário para o .NET Aspire criar e rodar o container do PostgreSQL).

### Passos de Execução

1.  **Clone o Repositório:**
    ```bash
    git clone [git@github.com:dequeirozmarcondes/TodoApi.git]
    cd TodoApi/
    ```
2.  **Abra a Solução no Visual Studio:**
    Abra o arquivo de solução (`.slnx`).

3.  **Defina o Projeto de Host:**
    Clique com o botão direito no projeto **`TodoAppAPI.AppHost`** e selecione "Definir como Projeto de Inicialização" (Set as Startup Project).

4.  **Execute sem depurar (CTRL + F5):**
    Ou Pressione a tecla **F5** para executar e depurar no Visual Studio.

    * O .NET Aspire irá construir os projetos, iniciar o container do PostgreSQL e implantar a API.
    * O **Aspire Dashboard** será aberto no seu navegador, onde você pode monitorar logs, métricas e endpoints de todos os serviços.
    * A API principal (`TodoAppAPI`) estará rodando e pronta para ser acessada.

---


## 🌐 Endpoints da API

## 🔐 Microsserviço TodoAppIdentity (Autenticação e Registro)

Estes endpoints são usados para obter o JWT Token necessário para acessar os recursos protegidos.

| Método HTTP | Rota | Descrição | Corpo da Requisição | Resposta (Sucesso) |
| :--- | :--- | :--- | :--- | :--- |
| **POST** | `/api/Auth/register` | **Registro de Usuário.** Cria um novo usuário e retorna o JWT Token. | `{"email": "string", "password": "string"}` | `{"token": "JWT_STRING", "expiration": "DateTime"}` |
| **POST** | `/api/Auth/login` | **Login de Usuário.** Autentica o usuário e retorna o JWT Token. | `{"email": "string", "password": "string"}` | `{"token": "JWT_STRING", "expiration": "DateTime"}` |

---

## 📝 Microsserviço TodoApp (Gerenciamento de Tarefas)

A API de Tarefas está exposta na rota base `/api/TodoItems`. **Todos os endpoints abaixo exigem o JWT Token válido no cabeçalho `Authorization: Bearer <token>`.**

| Método HTTP | Rota | Descrição | Corpo da Requisição |
| :--- | :--- | :--- | :--- |
| **GET** | `/api/TodoItems` | Retorna todos os itens da lista. | Nenhum |
| **GET** | `/api/TodoItems/{id}` | Retorna um item específico pelo ID. | Nenhum |
| **POST** | `/api/TodoItems` | Cria um novo item de tarefa. | `{"name": "string", "isComplete": bool?}` |
| **PATCH** | `/api/TodoItems/{id}` | **Atualização Parcial.** Modifica apenas os campos enviados (`name` OU `isComplete`). | `{"name": "string"}` OU `{"isComplete": true}` |
| **DELETE** | `/api/TodoItems/{id}` | Remove um item da lista. | Nenhum |


---

## ⚙️ Configuração (Aspire e EF Core)

### Configuração do Banco de Dados

O .NET Aspire simplifica a conexão. A string de conexão é gerenciada pelo projeto `AppHost` e injetada na API através do serviço `aspire:TodoAppAPI:todosdb`.

### Aplicação das Migrations

O banco de dados é provisionado automaticamente via Docker pelo Aspire. Para garantir que o esquema (tabela) esteja criado no PostgreSQL:

1.  Verifique se o banco de dados está rodando.
2.  Execute as migrações (pode ser necessário rodá-las manualmente na primeira vez se a inicialização automática não estiver configurada):

    ```bash
    # Navegue até o diretório do projeto TodoAppAPI
    dotnet ef database update --project TodoAppAPI
    ```
---

*Feito com ❤️ por Renato de Queiroz Marcondes.*
