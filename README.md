# AuthPerfisApi – API Minimal com Autenticação e Perfis de Acesso

Este projeto apresenta uma API Minimal desenvolvida em **ASP.NET Core**, implementando autenticação simples, perfis de acesso (Admin/User), login com token fake (GUID), 
hash de senha usando SHA256 e um middleware personalizado para proteção de rotas.  

## 🚀 Funcionalidades

- **Cadastro de usuários** (`POST /usuarios`)
  - Nome, Email, Senha (com hash) e Perfil (Admin/User)
  - Validações completas (campos obrigatórios, senha mínima, e-mail duplicado)

- **Login** (`POST /login`)
  - Validação de credenciais
  - Geração de token fake (GUID)
  - Armazenamento dos tokens em memória

- **Proteção de rotas**
  - Middleware que exige `X-Perfil: Admin` para acessar rotas administrativas
  - Exemplo: `GET /usuarios`

- **Armazenamento em memória**
  - Lista de usuários
  - Dicionário de tokens

---

## 🔐 Fluxo de Autenticação

1. O usuário se registra em `/usuarios`.
2. A senha é convertida em hash SHA256.
3. O usuário faz login em `/login`.
4. Se válido, recebe um token fake (GUID).
5. Rotas protegidas exigem envio do header:


---

## 🛠 Tecnologias Utilizadas

- ASP.NET Core Minimal API  
- C#  
- SHA256 para hashing  
- Middleware customizado  
- DTOs e boas práticas de API
- 
## Como Rodar o Projeto
1. Clone o repositório:
```bash
git clone https://github.com/CaioL-eandro/AuthPerfisApi.git

Acesse o diretório:

cd AuthPerfisApi

Execute:

dotnet run

Acesse o Swagger:

https://localhost:7058/swagger

