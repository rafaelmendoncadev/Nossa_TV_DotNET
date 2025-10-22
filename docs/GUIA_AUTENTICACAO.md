# Guia de Autenticação - Nossa TV

## ?? Visão Geral

Sistema completo de autenticação implementado com ASP.NET Core Identity, incluindo cadastro de usuários, login, logout e controle de acesso baseado em roles.

## ?? Credenciais Padrão

### Usuário Administrador
- **Email:** admin@nossatv.com
- **Senha:** Admin@123
- **Role:** Admin

Este usuário é criado automaticamente na primeira execução da aplicação.

## ?? Funcionalidades Implementadas

### ? Sistema de Autenticação
- Cadastro de novos usuários
- Login com email e senha
- Logout seguro
- Recuperação de senha (configurável)
- Lembrar-me (persistência de login por 7 dias)

### ? Controle de Acesso
- Usuários autenticados têm acesso a:
  - Envio de mensagens
  - Histórico de mensagens pessoais
  - Área do cliente (em desenvolvimento)

- Administradores têm acesso adicional a:
  - Painel administrativo de mensagens
  - Gerenciamento de leads
  - Dashboard com estatísticas

### ? Interface de Navegação
- Menu dinâmico baseado em estado de autenticação
- Links específicos para usuários autenticados
- Acesso destacado ao painel admin para administradores
- Área do cliente destacada para usuários comuns
- Dropdown de perfil com opções de logout

## ??? Estrutura de Arquivos

```
Nossa_TV/
??? Models/
?   ??? ApplicationUser.cs              # Modelo de usuário customizado
??? Data/
?   ??? ApplicationDbContext.cs         # Contexto do Entity Framework
??? Controllers/
?   ??? AccountController.cs            # Autenticação (Login/Register/Logout)
?   ??? MessagesController.cs           # Requer autenticação
?   ??? Admin/
?       ??? AdminMessagesController.cs  # Requer role "Admin"
?       ??? AdminLeadsController.cs     # Requer role "Admin"
??? ViewModels/
?   ??? LoginViewModel.cs
?   ??? RegisterViewModel.cs
??? Views/
    ??? Account/
    ?   ??? Login.cshtml
    ?   ??? Register.cshtml
    ?   ??? AccessDenied.cshtml
    ??? Shared/
        ??? _Layout.cshtml              # Navegação com autenticação
```

## ?? Configuração

### 1. Banco de Dados
O sistema usa SQLite para armazenamento local. O banco é criado automaticamente em:
```
Nossa_TV/nossatv.db
```

### 2. Identity Settings (Program.cs)
```csharp
// Configurações de senha
options.Password.RequireDigit = true;
options.Password.RequireLowercase = true;
options.Password.RequireUppercase = false;
options.Password.RequireNonAlphanumeric = false;
options.Password.RequiredLength = 6;

// Configurações de lockout
options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
options.Lockout.MaxFailedAccessAttempts = 5;

// Cookie de autenticação
options.ExpireTimeSpan = TimeSpan.FromDays(7);
options.SlidingExpiration = true;
```

## ?? Fluxo de Autenticação

### Novo Usuário
1. Acessa `/Account/Register`
2. Preenche: Nome Completo, Email, Senha, Confirmação de Senha
3. Sistema cria usuário e faz login automaticamente
4. Redireciona para página inicial (autenticado)

### Usuário Existente
1. Acessa `/Account/Login`
2. Preenche: Email e Senha
3. Opcionalmente marca "Lembrar-me"
4. Sistema autentica e redireciona
5. Se houver `returnUrl`, volta para página solicitada

### Envio de Mensagem
1. Usuário deve estar autenticado
2. Se não estiver, é redirecionado para `/Account/Login`
3. Após login, retorna automaticamente para o formulário
4. Mensagem é salva com o email do usuário autenticado

## ?? Interface do Usuário

### Barra de Navegação - Não Autenticado
```
Logo | Recursos | Planos | FAQ | Entrar | Cadastre-se | Assinar Agora
```

### Barra de Navegação - Usuário Comum
```
Logo | Recursos | Planos | FAQ | Contato | Minhas Mensagens | Área do Cliente | [Dropdown: Perfil/Sair]
```

### Barra de Navegação - Administrador
```
Logo | Recursos | Planos | FAQ | Contato | Minhas Mensagens | [Painel Admin] | [Dropdown: Perfil/Sair]
```

### Componentes Visuais
- ?? Ícone de envelope para Contato
- ?? Ícone de inbox para Minhas Mensagens
- ?? Ícone de velocímetro para Painel Admin (dourado)
- ?? Ícone de TV para Área do Cliente (azul)
- ?? Ícone de pessoa para perfil do usuário

## ?? Segurança

### Proteção de Rotas
```csharp
// Requer autenticação
[Authorize]
public IActionResult Send()

// Requer role específica
[Authorize(Roles = "Admin")]
public class AdminMessagesController
```

### Anti-Forgery Tokens
Todos os formulários incluem proteção CSRF:
```csharp
@Html.AntiForgeryToken()

[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(LoginViewModel model)
```

### Validação de Dados
- Client-side: jQuery Validation
- Server-side: Data Annotations
- Validação customizada no controller

## ?? Roles e Permissões

### Role: User (Padrão)
- Acesso à área pública
- Envio de mensagens
- Visualização do próprio histórico
- Área do cliente

### Role: Admin
- Todas as permissões de User
- Acesso ao painel administrativo
- Gerenciamento de todas as mensagens
- Gerenciamento de leads
- Dashboard com estatísticas

## ?? Como Testar

### 1. Primeira Execução
```bash
dotnet run
```
O sistema criará:
- Banco de dados SQLite
- Roles (Admin, User)
- Usuário admin padrão

### 2. Testar Cadastro
1. Acessar `https://localhost:xxxx/Account/Register`
2. Criar novo usuário
3. Verificar login automático
4. Testar envio de mensagem

### 3. Testar Painel Admin
1. Fazer logout (se estiver logado)
2. Logar com admin@nossatv.com / Admin@123
3. Verificar acesso ao "Painel Admin" (dourado)
4. Testar gerenciamento de mensagens

## ?? Próximos Passos

- [ ] Implementar recuperação de senha via email
- [ ] Adicionar confirmação de email
- [ ] Autenticação de dois fatores (2FA)
- [ ] Login com redes sociais (Google, Facebook)
- [ ] Gestão de perfil do usuário
- [ ] Upload de foto de perfil
- [ ] Histórico de atividades do usuário
- [ ] Notificações em tempo real

## ?? Mensagens de Erro Comuns

### "Email ou senha inválidos"
- Verifique se o email está correto
- Confirme se a senha tem mínimo 6 caracteres
- Tente fazer logout e login novamente

### "Acesso Negado"
- Você não tem permissão para acessar essa área
- Verifique se está usando a conta correta
- Administradores devem usar a conta admin

### "Por favor, faça login"
- Sua sessão expirou
- Faça login novamente
- Marque "Lembrar-me" para sessão prolongada

## ?? Troubleshooting

### Banco de dados não foi criado
```bash
# Deletar banco existente
rm nossatv.db

# Executar aplicação novamente
dotnet run
```

### Usuário admin não existe
O usuário é criado automaticamente no `Program.cs`. Verifique os logs de inicialização.

### CSS não está sendo aplicado
1. Verifique se o Bootstrap está carregado no `_Layout.cshtml`
2. Limpe o cache do navegador (Ctrl+F5)
3. Verifique se `landing.css` está sendo carregado

---

Desenvolvido para o projeto Nossa TV
