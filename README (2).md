# 🌱 EcoTrack.Net — Sprint 2

Projeto desenvolvido para o **Challenge FIAP - 2º Semestre / 2TDS**  
Disciplina: **Advanced Business Development with .NET**

API REST desenvolvida em **ASP.NET Core 8** com **Entity Framework Core** e arquitetura em camadas.  
O objetivo do sistema é registrar produtos e calcular seu impacto ambiental (CO₂ por unidade), promovendo consumo mais sustentável.

---

## 🎯 Objetivo Geral

O **EcoTrack.Net** permite o **cadastro, consulta, atualização e exclusão** de produtos, exibindo informações nutricionais e de impacto ambiental.  
Nesta **Sprint 2**, o foco foi a **camada Web (API)** — adicionando **paginação, ordenação, filtros, HATEOAS e rotas REST completas**.

---

## 🏗️ Arquitetura da Solução

O projeto segue uma **arquitetura em camadas**, organizada da seguinte forma:

```
EcoTrack.Domain/ -> Entidades e interfaces de repositório
EcoTrack.Application/ -> DTOs e regras de negócio (services)
EcoTrack.Infrastructure/ -> DbContext e implementação dos repositórios
EcoTrack.Net/ -> Camada Web (Controllers / API)
```

**Tecnologias utilizadas:**
- .NET 8
- Entity Framework Core
- Swagger (documentação da API)
- Oracle Database (compatível) / InMemoryDatabase (teste)
- REST + HATEOAS
- C#

---

## ⚙️ Configuração do Banco de Dados

A API está configurada por padrão para rodar com um banco **InMemory** (para facilitar testes locais e validação da Sprint 2).  
Entretanto, mantém compatibilidade com o **Oracle Database**, usado na Sprint 1.

**Para testar localmente (sem Oracle):**
```csharp
builder.Services.AddDbContext<EcoTrackDbContext>(opt => 
    opt.UseInMemoryDatabase("EcoTrackDb"));
```

**Para conectar ao Oracle (opcional):**
```csharp
builder.Services.AddDbContext<EcoTrackDbContext>(opt =>
    opt.UseOracle(builder.Configuration.GetConnectionString("Oracle")));
```

**Exemplo de connection string (em appsettings.json):**
```json
{
  "ConnectionStrings": {
    "Oracle": "User Id=rm560262;Password=SYSADM;Data Source=oracle.fiap.com.br:1521/ORCL"
  }
}
```

---

## 🚀 Como Executar o Projeto

### 1️⃣ Pré-requisitos
- .NET 8 SDK instalado  
- (Opcional) Oracle Database 21c XE  
- VS Code, Visual Studio ou Rider

### 2️⃣ Clonar o projeto
```bash
git clone https://github.com/gDantazz/Ecotrack-Net
cd .NET-EcoTrack/EcoTrack.Net
```

### 3️⃣ Restaurar e rodar
```bash
dotnet restore
dotnet run
```

Acesse a documentação Swagger em:  
👉 http://localhost:5000/swagger

---

## 🧩 Endpoints Principais

### 🔹 CRUD de Produtos
| Método | Endpoint | Descrição |
|--------|-----------|-----------|
| GET | /api/products | Busca produtos (com paginação, filtro, ordenação e HATEOAS) |
| GET | /api/products/{id} | Retorna produto por ID |
| POST | /api/products | Cria um novo produto |
| PUT | /api/products/{id} | Atualiza produto existente |
| DELETE | /api/products/{id} | Remove produto pelo ID |

---

### 🔎 Exemplo de Busca com Paginação e HATEOAS

**Requisição:**
```
GET /api/products?q=chocolate&category=Bebidas&page=1&pageSize=5&sortBy=kcal&sortDir=asc
```

**Resposta:**
```json
{
  "items": [
    {
      "id": "93d0fadc-9c0a-4f2a-9b23-cc93c9d62aa0",
      "name": "Achocolatado Eco",
      "category": "Bebidas",
      "caloriesPer100g": 89.5,
      "co2PerUnit": 0.42,
      "barcode": "7891000123456",
      "links": [
        { "rel": "self", "href": ".../api/products/93d0fadc...", "method": "GET" },
        { "rel": "update", "href": ".../api/products/93d0fadc...", "method": "PUT" },
        { "rel": "delete", "href": ".../api/products/93d0fadc...", "method": "DELETE" }
      ]
    }
  ],
  "total": 20,
  "page": 1,
  "pageSize": 5,
  "links": [
    { "rel": "self", "href": ".../api/products?page=1", "method": "GET" },
    { "rel": "next", "href": ".../api/products?page=2", "method": "GET" },
    { "rel": "last", "href": ".../api/products?page=4", "method": "GET" }
  ]
}
```

---

## 🧠 HATEOAS Implementado

Cada recurso retornado traz:
- Links de ação (self, update, delete)
- Links de paginação (first, prev, next, last)

Isso garante que o cliente possa navegar na API dinamicamente, sem precisar conhecer todas as rotas.

---

## ✅ Requisitos da Sprint 2 Atendidos

| Requisito | Status |
|------------|---------|
| CRUD completo | ✅ |
| Rota de busca com filtros e paginação | ✅ |
| Ordenação (asc/desc) por campos | ✅ |
| Implementação de HATEOAS | ✅ |
| Boas práticas REST | ✅ |
| README atualizado e explicativo | ✅ |
| Projeto compila e executa corretamente | ✅ |

---

## 👨‍💻 Integrantes

| Gustavo Dantas | RM560685
| Paulo Neto | RM560262
| Davi Vasconcelos Souza | RM559906

---

## 🏁 Conclusão

O projeto **EcoTrack.Net** agora possui uma API REST completa, com rotas bem estruturadas, paginação, filtros e suporte a HATEOAS.  
A arquitetura modular facilita a evolução do sistema e integração futura com o front-end e outros serviços da solução EcoTrack.
