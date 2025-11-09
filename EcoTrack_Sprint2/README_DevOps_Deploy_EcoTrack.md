# ☁️ EcoTrack.NET — Deploy em Cloud (Sprint 2 - DevOps & Cloud Computing)

Projeto desenvolvido para o **Challenge FIAP - 2º Semestre / 2TDS**  
Disciplina: **DevOps Tools & Cloud Computing**

O **EcoTrack.NET** é uma API REST desenvolvida em **.NET 8**, com **Entity Framework Core** e arquitetura em camadas, agora totalmente **containerizada e hospedada na nuvem (Azure)** utilizando **Docker**.

---

## 🚀 Objetivo

Demonstrar o deploy completo da aplicação na nuvem com:

- Provisionamento de uma **VM Linux (Ubuntu)**  
- Instalação e configuração do **Docker e Docker Compose**  
- Build e execução da aplicação em container  
- Exposição da API para acesso externo (`http://IP-DA-VM:5000/swagger`)  
- Organização dos arquivos de automação (`Dockerfile` e `docker-compose.yml`)

---

## ⚙️ Tecnologias Utilizadas

- **.NET 8**
- **Entity Framework Core**
- **Docker**
- **Docker Compose**
- **Azure Virtual Machine (Ubuntu 24.04 LTS)**
- **InMemory Database** (para demonstração)
- **Swagger UI**

---

## 🧱 Estrutura do Projeto

```
EcoTrack_Sprint2/
│
├── EcoTrack.Domain/
├── EcoTrack.Application/
├── EcoTrack.Infrastructure/
├── EcoTrack.Net/              # API Web (camada principal)
│   ├── Controllers/
│   ├── Program.cs
│   ├── appsettings.json
│   └── EcoTrack.Net.csproj
│
├── Dockerfile
├── docker-compose.yml
└── README.md
```

---

## 🐋 Arquivos de Deploy

### Dockerfile
```dockerfile
# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia tudo
COPY . .

# Restaura dependências do projeto principal
WORKDIR "/src/EcoTrack.Net"
RUN dotnet restore "EcoTrack.Net.csproj"

# Compila e publica
RUN dotnet publish "EcoTrack.Net.csproj" -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "EcoTrack.Net.dll"]
```

### docker-compose.yml
```yaml
version: '3.8'

services:
  ecotrack:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "5000:5000"
    restart: always
```

---

## ☁️ Deploy na Azure VM (Ubuntu)

### 1️⃣ Conectar na VM
```bash
ssh ecotrack@SEU_IP_PUBLICO
```

### 2️⃣ Atualizar pacotes e instalar Docker
```bash
sudo apt update -y
sudo apt install -y ca-certificates curl gnupg lsb-release
sudo mkdir -p /etc/apt/keyrings
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o /etc/apt/keyrings/docker.gpg
echo   "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg]   https://download.docker.com/linux/ubuntu   $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt update -y
sudo apt install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin
sudo usermod -aG docker $USER
```

> **Saia e entre novamente no SSH** para aplicar o grupo Docker:
```bash
exit
ssh ecotrack@SEU_IP_PUBLICO
```

---

### 3️⃣ Clonar o repositório
```bash
git clone https://github.com/gDantazz/Ecotrack-Net.git
cd Ecotrack-Net/EcoTrack_Sprint2
```

---

### 4️⃣ Executar com Docker Compose
```bash
sudo docker-compose down
sudo docker-compose up -d --build
```

---

### 5️⃣ Verificar o container
```bash
docker ps
```

Você deverá ver algo como:
```
CONTAINER ID   IMAGE                       STATUS          PORTS
abcd1234ef56   ecotrack_sprint2_ecotrack   Up 20 seconds   0.0.0.0:5000->5000/tcp
```

---

### 6️⃣ Testar no navegador
Acesse:
```
http://SEU_IP_PUBLICO:5000/swagger
```

Se tudo estiver certo, o **Swagger** da API abrirá exibindo todos os endpoints.

---

## ✅ Checklist — Requisitos Atendidos

| Requisito | Status |
|------------|:------:|
| Provisionar VM na nuvem (Azure) | ✅ |
| Instalar e configurar Docker | ✅ |
| Subir API .NET em container | ✅ |
| Expor aplicação via porta 5000 | ✅ |
| Usar Dockerfile e docker-compose.yml | ✅ |
| Demonstrar domínio em Cloud + Docker | ✅ |
| Executar em background | ✅ |
| Organização e documentação | ✅ |

---

## 🎥 Vídeo Demonstrativo
https://youtu.be/NadbXaEP0Fs

---

## 👨‍💻 Integrantes
| Gustavo Dantas | RM560685
| Paulo Neto | RM560262
| Davi Vasconcelos Souza | RM559906

---

## 🏁 Conclusão

A aplicação **EcoTrack.NET** foi completamente **containerizada** e **implantada na nuvem**, demonstrando domínio prático sobre **Cloud Computing, Docker e DevOps**.  
A solução é modular, escalável e pronta para integrações futuras.

---
