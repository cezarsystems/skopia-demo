# 🚀 Demo API Projects - Skopia

API RESTful desenvolvida como parte de um desafio técnico para gerenciamento de projetos e tarefas. O sistema permite que usuários criem projetos, adicionem tarefas, realizem atualizações e acompanhem históricos de alterações.

---

## 🧰 Tecnologias Utilizadas

- .NET 8
- Entity Framework Core + SQLite
- AutoMapper
- FluentValidation
- xUnit + Moq + FluentAssertions
- Coverlet + ReportGenerator
- Docker

---

## 🧪 Executando os Testes

### ✅ Testes Unitários

```bash
dotnet test Skopia.Tests
```

### 📊 Cobertura de Testes

```bash
# Gerar cobertura com Coverlet e ReportGenerator
dotnet test ./Skopia.Tests/Skopia.Tests.csproj --no-build --collect:"XPlat Code Coverage"

# Gerar o relatório HTML
reportgenerator `
  -reports:"Skopia.Tests\TestResults\**\coverage.cobertura.xml" `
  -targetdir:"coveragereport" `
  -reporttypes:Html `
  -assemblyfilters:"-Skopia.Infrastructure.Migrations" `
  -classfilters:"-*.Migrations.*"

# Abrir no navegador
start .\coveragereport\index.html
```

🎯 **Cobertura atual:** `93.9%`

ℹ️ Para apoiar na visualização dos relatórios, utilize os scripts SQL no diretório `scripts/` em seu gerenciador de banco de dados (ex: DBeaver):

- `mock-performance-report.sql`
- `mock-project-stats-report.sql`
- `mock-time-completion-report.sql`

⚠️ Ao inserir dados diretamente via banco, as validações da API não serão aplicadas.

---

## 🖥️ Como Executar o Projeto

### 🧩 Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

---

### 🔧 Rodando Localmente

```bash
# Na raiz da solução
dotnet build
dotnet ef database update -p Skopia.Infrastructure -s Skopia.Api
dotnet run --project Skopia.Api
```

Acesse: http://localhost:5062/swagger

---

### 🐳 Rodando com Docker

```bash
# Build da imagem
docker build -t demo-skopia .

# Executar o container
docker run -d -p 8080:80 --name demo-skopia -e ASPNETCORE_ENVIRONMENT=Development demo-skopia

# Acessar a aplicação
http://localhost:8080/swagger
```

❗ O `docker compose` não funcionou corretamente no ambiente de desenvolvimento. Recomenda-se o uso direto com `docker build` e `docker run`.

---

## ✅ Funcionalidades Implementadas

- ✅ Listagem de projetos
- ✅ Listagem de tarefas por projeto
- ✅ Criação de projetos
- ✅ Criação de tarefas
- ✅ Atualização de tarefas (status, descrição, etc.)
- ✅ Remoção de tarefas
- ✅ Histórico de alterações (status, comentários, datas)
- ✅ Comentários vinculados às tarefas
- ✅ Limite de até 20 tarefas por projeto
- ✅ Relatórios de desempenho (restrito a usuários com `Role = mgr`)

---

## 🧠 Refinamento – Perguntas ao Product Owner (PO)

1. **Usuários e Papéis**
   - A criação de projetos/tarefas será limitada por perfil de usuário?

2. **Limitação de Projetos**
   - Haverá um limite por usuário ou por período? Isso será configurável?

3. **Identificação do Gerente**
   - A role `mgr` será persistida na base ou recebida de um serviço externo?

4. **Formatos de Relatórios**
   - Será necessário exportar relatórios em formatos como CSV, JSON, XML?

5. **Comentários**
   - Poderão ser editados/removidos? Haverá restrição por status?

6. **Datas de Expiração**
   - Tarefas expiradas terão tratamento especial? Alguma automação?

7. **Limite de Tarefas por Projeto**
   - Esse limite será fixo ou poderá ser ajustado?

8. **Notificações**
   - Haverá notificações automáticas para tarefas expiradas?

9. **Disponibilidade**
   - A API terá períodos de indisponibilidade para manutenção?

10. **Auditoria**
    - Registros de exclusão serão auditados?

11. **Domínio Inteligente**
    - Algum tipo de automação será ativado por nomes de projetos/tarefas?

---

## 🔧 Sugestões Técnicas e Arquiteturais

### 🔐 Segurança

- Implementar autenticação via JWT e controle de acesso por role
- Proteção de endpoints críticos (criação/remoção)

### 🧱 Arquitetura

- Aplicar CQRS com MediatR
- Separar camadas de leitura e escrita
- Aplicar pipelines de CI/CD

### 🔍 Observabilidade

- Adicionar Serilog para logs estruturados
- Considerar Application Insights, Prometheus, ou ELK Stack

### 🧪 Testes

- Adicionar testes de integração e edge cases
- Simular perfis de usuário via headers
- Implementar versionamento de API (v1, v2)

### 🚀 Cloud Ready

- Rodar em App Services (Azure), ECS (AWS) ou Docker Swarm
- Banco de dados via Azure SQL, RDS (AWS), ou GCP SQL
- Utilizar Azure Storage ou S3 para arquivos persistentes

---

## 📄 Licença

Este projeto é fornecido apenas para fins de avaliação técnica.