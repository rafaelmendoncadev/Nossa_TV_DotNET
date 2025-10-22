# Guia de Autentica��o - Nossa TV

## ?? Vis�o Geral

Sistema completo de autentica��o implementado com ASP.NET Core Identity, incluindo cadastro de usu�rios, login, logout e controle de acesso baseado em roles.

## ?? Credenciais Padr�o

### Usu�rio Administrador
- **Email:** admin@nossatv.com
- **Senha:** Admin@123
- **Role:** Admin

Este usu�rio � criado automaticamente na primeira execu��o da aplica��o.

## ?? Funcionalidades Implementadas

### ? Sistema de Autentica��o
- Cadastro de novos usu�rios
- Login com email e senha
- Logout seguro
- Recupera��o de senha (configur�vel)
- Lembrar-me (persist�ncia de login por 7 dias)

### ? Controle de Acesso
- Usu�rios autenticados t�m acesso a:
  - Envio de mensagens
  - Hist�rico de mensagens pessoais
  - �rea do cliente (em desenvolvimento)

- Administradores t�m acesso adicional a:
  - Painel administrativo de mensagens
  - Gerenciamento de leads
  - Dashboard com estat�sticas

### ? Interface de Navega��o
- Menu din�mico baseado em estado de autentica��o
- Links espec�ficos para usu�rios autenticados
- Acesso destacado ao painel admin para administradores
- �rea do cliente destacada para usu�rios comuns
- Dropdown de perfil com op��es de logout

## ??? Estrutura de Arquivos

```
Nossa_TV/
??? Models/
?   ??? ApplicationUser.cs              # Modelo de usu�rio customizado
??? Data/
?   ??? ApplicationDbContext.cs         # Contexto do Entity Framework
??? Controllers/
?   ??? AccountController.cs            # Autentica��o (Login/Register/Logout)
?   ??? MessagesController.cs           # Requer autentica��o
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
        ??? _Layout.cshtml              # Navega��o com autentica��o
```

## ?? Configura��o

### 1. Banco de Dados
O sistema usa SQLite para armazenamento local. O banco � criado automaticamente em:
```
Nossa_TV/nossatv.db
```

### 2. Identity Settings (Program.cs)
```csharp
// Configura��es de senha
options.Password.RequireDigit = true;
options.Password.RequireLowercase = true;
options.Password.RequireUppercase = false;
options.Password.RequireNonAlphanumeric = false;
options.Password.RequiredLength = 6;

// Configura��es de lockout
options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
options.Lockout.MaxFailedAccessAttempts = 5;

// Cookie de autentica��o
options.ExpireTimeSpan = TimeSpan.FromDays(7);
options.SlidingExpiration = true;
```

## ?? Fluxo de Autentica��o

### Novo Usu�rio
1. Acessa `/Account/Register`
2. Preenche: Nome Completo, Email, Senha, Confirma��o de Senha
3. Sistema cria usu�rio e faz login automaticamente
4. Redireciona para p�gina inicial (autenticado)

### Usu�rio Existente
1. Acessa `/Account/Login`
2. Preenche: Email e Senha
3. Opcionalmente marca "Lembrar-me"
4. Sistema autentica e redireciona
5. Se houver `returnUrl`, volta para p�gina solicitada

### Envio de Mensagem
1. Usu�rio deve estar autenticado
2. Se n�o estiver, � redirecionado para `/Account/Login`
3. Ap�s login, retorna automaticamente para o formul�rio
4. Mensagem � salva com o email do usu�rio autenticado

## ?? Interface do Usu�rio

### Barra de Navega��o - N�o Autenticado
```
Logo | Recursos | Planos | FAQ | Entrar | Cadastre-se | Assinar Agora
```

### Barra de Navega��o - Usu�rio Comum
```
Logo | Recursos | Planos | FAQ | Contato | Minhas Mensagens | �rea do Cliente | [Dropdown: Perfil/Sair]
```

### Barra de Navega��o - Administrador
```
Logo | Recursos | Planos | FAQ | Contato | Minhas Mensagens | [Painel Admin] | [Dropdown: Perfil/Sair]
```

### Componentes Visuais
- ?? �cone de envelope para Contato
- ?? �cone de inbox para Minhas Mensagens
- ?? �cone de veloc�metro para Painel Admin (dourado)
- ?? �cone de TV para �rea do Cliente (azul)
- ?? �cone de pessoa para perfil do usu�rio

## ?? Seguran�a

### Prote��o de Rotas
```csharp
// Requer autentica��o
[Authorize]
public IActionResult Send()

// Requer role espec�fica
[Authorize(Roles = "Admin")]
public class AdminMessagesController
```

### Anti-Forgery Tokens
Todos os formul�rios incluem prote��o CSRF:
```csharp
@Html.AntiForgeryToken()

[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(LoginViewModel model)
```

### Valida��o de Dados
- Client-side: jQuery Validation
- Server-side: Data Annotations
- Valida��o customizada no controller

## ?? Roles e Permiss�es

### Role: User (Padr�o)
- Acesso � �rea p�blica
- Envio de mensagens
- Visualiza��o do pr�prio hist�rico
- �rea do cliente

### Role: Admin
- Todas as permiss�es de User
- Acesso ao painel administrativo
- Gerenciamento de todas as mensagens
- Gerenciamento de leads
- Dashboard com estat�sticas

## ?? Como Testar

### 1. Primeira Execu��o
```bash
dotnet run
```
O sistema criar�:
- Banco de dados SQLite
- Roles (Admin, User)
- Usu�rio admin padr�o

### 2. Testar Cadastro
1. Acessar `https://localhost:xxxx/Account/Register`
2. Criar novo usu�rio
3. Verificar login autom�tico
4. Testar envio de mensagem

### 3. Testar Painel Admin
1. Fazer logout (se estiver logado)
2. Logar com admin@nossatv.com / Admin@123
3. Verificar acesso ao "Painel Admin" (dourado)
4. Testar gerenciamento de mensagens

## ?? Pr�ximos Passos

- [ ] Implementar recupera��o de senha via email
- [ ] Adicionar confirma��o de email
- [ ] Autentica��o de dois fatores (2FA)
- [ ] Login com redes sociais (Google, Facebook)
- [ ] Gest�o de perfil do usu�rio
- [ ] Upload de foto de perfil
- [ ] Hist�rico de atividades do usu�rio
- [ ] Notifica��es em tempo real

## ?? Mensagens de Erro Comuns

### "Email ou senha inv�lidos"
- Verifique se o email est� correto
- Confirme se a senha tem m�nimo 6 caracteres
- Tente fazer logout e login novamente

### "Acesso Negado"
- Voc� n�o tem permiss�o para acessar essa �rea
- Verifique se est� usando a conta correta
- Administradores devem usar a conta admin

### "Por favor, fa�a login"
- Sua sess�o expirou
- Fa�a login novamente
- Marque "Lembrar-me" para sess�o prolongada

## ?? Troubleshooting

### Banco de dados n�o foi criado
```bash
# Deletar banco existente
rm nossatv.db

# Executar aplica��o novamente
dotnet run
```

### Usu�rio admin n�o existe
O usu�rio � criado automaticamente no `Program.cs`. Verifique os logs de inicializa��o.

### CSS n�o est� sendo aplicado
1. Verifique se o Bootstrap est� carregado no `_Layout.cshtml`
2. Limpe o cache do navegador (Ctrl+F5)
3. Verifique se `landing.css` est� sendo carregado

---

Desenvolvido para o projeto Nossa TV
