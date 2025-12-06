# AuthPerfisApi – API Minimal com Autenticação e Perfis de Acesso

Este projeto é uma API Minimal desenvolvida em *ASP.NET Core*, criada para demonstrar conceitos essenciais de autenticação simples, perfis de acesso (Admin/User),
validação de login e proteção de rotas utilizando middleware personalizado.

---

# Funcionalidades
- **Cadastro de usuário** (`POST /usuarios`)
  - Nome, Email, SenhaHash e Perfil (Admin/User)
  - Validação de senha mínima (6 caracteres)
  - Verificação de e-mail duplicado
  - Geração automática de ID
  - 
- **Login com token fake** (`POST /login`)
  - Gera um GUID como token
  - Token armazenado em memória
  - Senha comparada via **hash SHA256**

- **Proteção de rotas via middleware**
  - `GET /usuarios` só pode ser acessado se enviar o header:
    ```
    X-Perfil: Admin
    
- **Armazenamento em memória**
  - Lista de usuários
  - Dicionário de tokens

# Fluxo de Autenticação

1. O usuário se registra em `/usuarios`
2. A senha é convertida para hash SHA256
3. O login é feito em `/login`
4. Se os dados estiverem corretos, um token fake (GUID) é gerado
5. Rotas protegidas exigem header indicando o perfil do usuário
6. 
# Tecnologias Utilizadas

- ASP.NET Core Minimal API
- C#
- SHA256 para hashing de senha
- Middleware customizado
- DTOs (Data Transfer Objects)
- Validações e boas práticas de backend

## 📌 Como Rodar o Projeto

1️ Clone o repositório
git clone https://github.com/CaioL-eandro/AuthPerfisApi.git

2️ Acesse a pasta do projeto
cd AuthPerfisApi

3️ Execute a aplicação
dotnet run

4️ Acesse o Swagger no navegador
https://localhost:7058/swagger

1. Clone o repositório:
   ```bash
   git clone https://github.com/CaioL-eandro/AuthPerfisApi.gi
