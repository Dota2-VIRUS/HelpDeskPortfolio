# HelpDesk Portfolio

Central de chamados (help desk) full stack, criada para demonstrar as minhas habilidades em **.NET** e **Angular**.

**Autor:** Dhionatan Medeiros — Desenvolvedor Full Stack (C# · ASP.NET Core · Angular · SQL Server)

---

## Sobre o projeto

Aplicação simples e completa: abrir, listar, filtrar, atualizar o status e excluir chamados de suporte.
O tema vem do meu dia a dia: trabalho com sistemas corporativos, integração com help desk e ERP, APIs RESTful e manutenção de sistemas legados na Vicunha Têxtil.

```
Angular 18 (SPA)  ──HTTP/JSON──▶  ASP.NET Core 8 Web API  ──EF Core──▶  SQL Server
```

## Habilidades demonstradas

| Área | O que aparece neste repositório |
|---|---|
| **C# / .NET 8** | Projeto Web API, injeção de dependência, async/await, records, nullable reference types |
| **ASP.NET Core** | Controllers REST, códigos HTTP corretos (200/201/204/404), validação com DataAnnotations, CORS, Swagger/OpenAPI |
| **Entity Framework Core** | `DbContext`, mapeamento por Fluent API, índice, consultas com `AsNoTracking`, filtros dinâmicos com `IQueryable` |
| **SQL Server** | Provider `Microsoft.EntityFrameworkCore.SqlServer`, LocalDB em desenvolvimento |
| **Arquitetura** | Separação em Controllers → Services → Data/Domain, DTOs separados da entidade |
| **Testes** | xUnit + EF Core InMemory cobrindo a regra de negócio do serviço |
| **Angular / TypeScript** | Standalone components, Signals, Reactive Forms, `HttpClient`, serviço tipado, control flow (`@if` / `@for`), proxy de desenvolvimento |
| **API RESTful / JSON** | Contrato único entre front e back, enums serializados como texto |
| **Git / GitHub** | Repositório organizado, `.gitignore` para .NET e Node |
| **Metodologias** | Scrum e Kanban no trabalho diário |

### Outras tecnologias com que trabalho (fora deste projeto)

VB.NET · Windows Forms · PHP/Laravel · MySQL · JavaScript · Bootstrap · VBScript · Postman · Visual Studio · VS Code · integração de sistemas (ex.: batch VB.NET entre help desk e ERP Totvs EMS/Progress OpenEdge).

## Estrutura

```
HelpDeskPortfolio.sln
├── src/
│   ├── HelpDesk.Api/          # ASP.NET Core Web API (net8.0)
│   │   ├── Controllers/       # TicketsController
│   │   ├── Services/          # ITicketService / TicketService (regras de negócio)
│   │   ├── Data/              # AppDbContext (EF Core)
│   │   ├── Domain/            # Entidade Ticket + enums
│   │   └── Dtos/              # Create/Update/Response
│   └── helpdesk-web/          # Angular 18
└── tests/
    └── HelpDesk.Tests/        # xUnit
```

## Como executar

**Pré-requisitos:** [.NET 8 SDK](https://dotnet.microsoft.com/download), Node.js 20+ e SQL Server LocalDB (instalado junto com o Visual Studio) ou outra instância de SQL Server.

### 1. API

```bash
cd src/HelpDesk.Api
dotnet run --launch-profile http
```

Swagger: <http://localhost:5080/swagger>

Em desenvolvimento o banco é criado automaticamente na primeira execução (`EnsureCreated`).
Para usar outra instância, altere `ConnectionStrings:Default` em `appsettings.json`.

Pelo Visual Studio: abra `HelpDeskPortfolio.sln`, defina `HelpDesk.Api` como projeto de inicialização e pressione **F5**.

### 2. Front-end

```bash
cd src/helpdesk-web
npm install
npm start
```

Acesse <http://localhost:4200>. O `proxy.conf.json` encaminha `/api` para `http://localhost:5080`.

### 3. Testes

```bash
dotnet test
```

## Endpoints

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/tickets?status=Open&search=vpn` | Lista com filtros opcionais |
| GET | `/api/tickets/{id}` | Detalhe |
| POST | `/api/tickets` | Cria (status inicial: `Open`) |
| PUT | `/api/tickets/{id}` | Atualiza |
| DELETE | `/api/tickets/{id}` | Exclui |

## Migrations (produção)

Em vez de `EnsureCreated`, gere e aplique migrations:

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add Initial --project src/HelpDesk.Api
dotnet ef database update --project src/HelpDesk.Api
```

## Próximos passos

- [ ] Autenticação JWT e perfis (usuário / técnico)
- [ ] Paginação e ordenação na API
- [ ] Testes de integração com `WebApplicationFactory`
- [ ] Testes do front-end (Jasmine/Karma)
- [ ] Pipeline de CI (GitHub Actions)
- [ ] Docker Compose (API + SQL Server + front)
