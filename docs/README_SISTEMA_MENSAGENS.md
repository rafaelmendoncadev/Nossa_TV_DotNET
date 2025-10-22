# Sistema de Mensagens com Painel Administrativo - Nossa TV

Sistema completo de gerenciamento de mensagens e leads integrado com Back4App, **agora com autentica��o completa usando ASP.NET Core Identity**.

## ?? **NOVIDADE: Sistema de Autentica��o**

### Credenciais de Acesso
**Usu�rio Administrador Padr�o:**
- Email: `admin@nossatv.com`
- Senha: `Admin@123`

### Funcionalidades de Autentica��o
- ? Cadastro de novos usu�rios
- ? Login seguro com email e senha
- ? Logout com prote��o CSRF
- ? Controle de acesso baseado em roles (Admin/User)
- ? Navega��o din�mica baseada em autentica��o
- ? Prote��o de rotas sens�veis
- ? Sess�o persistente com "Lembrar-me"

### **IMPORTANTE: Autentica��o Obrigat�ria**
?? **Agora � necess�rio fazer login antes de enviar mensagens!**

Os usu�rios devem:
1. Se cadastrar em `/Account/Register` OU
2. Fazer login em `/Account/Login`
3. Depois acessar o formul�rio de mensagens

## ?? Funcionalidades Implementadas

### ? Sistema de Mensagens do Usu�rio
- **Formul�rio de contato (requer autentica��o)**
- Valida��o de dados no cliente e servidor
- Feedback visual ap�s envio
- Hist�rico de mensagens para usu�rios autenticados
- CSS moderno e responsivo

### ? Painel Administrativo - Mensagens
- Listagem de todas as mensagens recebidas
- Filtros por status (Nova, Lida, Respondida, Arquivada) e data
- Marcar mensagens como lida
- Responder mensagens
- Arquivar ou excluir mensagens
- Dashboard com estat�sticas completas
- **Restrito a usu�rios com role "Admin"**

### ? Painel Administrativo - Leads
- Lista de todos os leads capturados automaticamente
- Informa��es detalhadas: nome, email, telefone, quantidade de mensagens
- Sistema de tags para classifica��o
- Notas personalizadas por lead
- Hist�rico completo de intera��es
- Exporta��o para CSV
- **Restrito a usu�rios com role "Admin"**

### ? Navega��o Integrada e Din�mica
A barra de navega��o se adapta ao estado de autentica��o:

#### Usu�rio N�O Autenticado:
- Links p�blicos (Recursos, Planos, FAQ)
- **Entrar** - Link para login
- **Cadastre-se** - Link para registro
- Assinar Agora

#### Usu�rio Comum Autenticado:
- ?? **Contato** - Formul�rio de mensagens
- ?? **Minhas Mensagens** - Hist�rico pessoal
- ?? **�rea do Cliente** - �rea exclusiva (destaque em azul)
- ?? Menu dropdown com nome do usu�rio
  - Meu Perfil
  - Sair

#### Administrador Autenticado:
- ?? **Contato** - Formul�rio de mensagens
- ?? **Minhas Mensagens** - Hist�rico pessoal
- ?? **Painel Admin** - Dashboard administrativo (destaque em dourado)
- ?? Menu dropdown com nome do usu�rio
  - Meu Perfil
  - Sair

## ??? Estrutura do Projeto

```
Nossa_TV/
??? Controllers/
?   ??? AccountController.cs               # Login, Registro, Logout
?   ??? MessagesController.cs              # �rea p�blica de mensagens (autentica��o obrigat�ria)
?   ??? Admin/
?       ??? AdminMessagesController.cs     # Gerenciamento admin de mensagens (role Admin)
?       ??? AdminLeadsController.cs        # Gerenciamento de leads (role Admin)
??? Models/
?   ??? ApplicationUser.cs                 # Modelo de usu�rio (Identity)
?   ??? Message.cs                         # Modelo de mensagem
?   ??? MessageReply.cs                    # Modelo de resposta
?   ??? Lead.cs                            # Modelo de lead
?   ??? MessageStatus.cs                   # Enum de status
??? Data/
?   ??? ApplicationDbContext.cs            # Contexto do Entity Framework
??? ViewModels/
?   ??? LoginViewModel.cs                  # ViewModel de login
?   ??? RegisterViewModel.cs               # ViewModel de registro
?   ??? SendMessageViewModel.cs
?   ??? AdminMessageListViewModel.cs
?   ??? AdminMessageDetailViewModel.cs
?   ??? UserMessageHistoryViewModel.cs
?   ??? LeadListViewModel.cs
?   ??? LeadDetailViewModel.cs
??? Services/
?   ??? IMessageService.cs
?   ??? MessageService.cs                  # L�gica de neg�cio de mensagens
?   ??? ILeadService.cs
?   ??? LeadService.cs                     # L�gica de neg�cio de leads
??? Views/
    ??? Account/
    ?   ??? Login.cshtml                   # P�gina de login
    ?   ??? Register.cshtml                # P�gina de cadastro
    ?   ??? AccessDenied.cshtml            # Acesso negado
    ??? Messages/
    ?   ??? Send.cshtml                    # Formul�rio de envio (CSS melhorado)
    ?   ??? MyMessages.cshtml              # Hist�rico do usu�rio
    ?   ??? Detail.cshtml                  # Detalhes da mensagem
    ??? Admin/
        ??? AdminMessages/
        ?   ??? Index.cshtml               # Lista de mensagens
        ?   ??? Detail.cshtml              # Detalhes e resposta
        ?   ??? Dashboard.cshtml           # Estat�sticas
        ??? AdminLeads/
            ??? Index.cshtml               # Lista de leads
            ??? Detail.cshtml              # Detalhes do lead
```

## ?? Configura��o

### Banco de Dados - SQLite
O sistema usa SQLite para autentica��o local:
```
Data Source=nossatv.db
```

O banco de dados � criado automaticamente na primeira execu��o, incluindo:
- Tabelas do Identity (Users, Roles, UserRoles, etc.)
- Roles padr�o (Admin, User)
- Usu�rio administrador padr�o

### Back4App (Mensagens e Leads)
O sistema continua usando Back4App para mensagens e leads:

```csharp
builder.Services.AddHttpClient("Back4App", client =>
{
    client.BaseAddress = new Uri("https://parseapi.back4app.com/");
    client.DefaultRequestHeaders.Add("X-Parse-Application-Id", "z4XT6b7pn6D6TwfLjAkVImWCI6txKKF5fBJ9m2O3");
    client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", "oxumxDTkBG21lfrZC1xuXpwc2F1975cSq54OGhVp");
});
```

### Classes do Back4App
1. **Message** - Armazena todas as mensagens
2. **MessageReply** - Armazena respostas do admin
3. **Lead** - Armazena leads capturados

## ?? Navega��o e Rotas

### �rea P�blica
- `GET /` - P�gina inicial
- `GET /Account/Register` - Cadastro de novo usu�rio
- `GET /Account/Login` - Login de usu�rio
- `POST /Account/Logout` - Logout (requer autentica��o)

### �rea de Mensagens (Requer Autentica��o)
- `GET /Messages/Send` - Formul�rio de envio de mensagem
- `POST /Messages/Send` - Processar envio de mensagem
- `GET /Messages/MyMessages` - Hist�rico de mensagens
- `GET /Messages/Detail/{id}` - Detalhes de uma mensagem espec�fica

### �rea Admin - Mensagens (Requer Role "Admin")
- `GET /Admin/AdminMessages` - Lista de mensagens
- `GET /Admin/AdminMessages/Dashboard` - Dashboard com estat�sticas
- `GET /Admin/AdminMessages/Detail/{id}` - Detalhes da mensagem
- `POST /Admin/AdminMessages/Reply/{id}` - Responder mensagem
- `POST /Admin/AdminMessages/MarkAsRead/{id}` - Marcar como lida
- `POST /Admin/AdminMessages/Archive/{id}` - Arquivar mensagem
- `POST /Admin/AdminMessages/Delete/{id}` - Excluir mensagem

### �rea Admin - Leads (Requer Role "Admin")
- `GET /Admin/AdminLeads` - Lista de leads
- `GET /Admin/AdminLeads/Detail/{id}` - Detalhes do lead
- `POST /Admin/AdminLeads/AddTag` - Adicionar tag
- `POST /Admin/AdminLeads/RemoveTag` - Remover tag
- `POST /Admin/AdminLeads/UpdateNotes` - Atualizar notas
- `GET /Admin/AdminLeads/Export` - Exportar leads para CSV

## ?? CSS e Interface

### Melhorias no CSS
- ? Formul�rio de mensagens com design moderno
- ? Cards com sombras e bordas arredondadas
- ? Bot�es com gradientes e anima��es
- ? Alertas com bordas coloridas e �cones
- ? Formul�rios de autentica��o estilizados
- ? Dropdown de usu�rio com transi��es suaves
- ? Navega��o responsiva para mobile

### Bootstrap 5
Carregado via CDN no `_Layout.cshtml`:
```html
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
```

## ?? Seguran�a

### Configura��es de Senha
- M�nimo 6 caracteres
- Requer d�gito
- Requer letra min�scula
- N�o requer letra mai�scula
- N�o requer caractere especial

### Prote��o de Rotas
```csharp
[Authorize]                    // Requer autentica��o
[Authorize(Roles = "Admin")]   // Requer role espec�fica
```

### Anti-Forgery
Todos os formul�rios incluem tokens CSRF:
```csharp
@Html.AntiForgeryToken()
[ValidateAntiForgeryToken]
```

### Lockout
- M�ximo 5 tentativas falhas
- Bloqueio de 5 minutos ap�s exceder

## ?? Fluxo Completo de Uso

### 1. Primeiro Acesso (Novo Usu�rio)
1. Usu�rio acessa o site
2. Clica em "Cadastre-se"
3. Preenche: Nome, Email, Senha
4. Sistema cria conta e faz login automaticamente
5. Usu�rio � redirecionado para home (autenticado)
6. V� "�rea do Cliente" na navega��o

### 2. Enviar Mensagem
1. Usu�rio autenticado clica em "Contato"
2. Preenche formul�rio com design moderno
3. Mensagem � enviada para Back4App
4. Lead � capturado/atualizado automaticamente
5. Feedback de sucesso � exibido
6. Pode acessar "Minhas Mensagens" para hist�rico

### 3. Administrador
1. Faz login com admin@nossatv.com
2. V� "Painel Admin" (dourado) na navega��o
3. Acessa dashboard com estat�sticas
4. Gerencia mensagens e leads
5. Responde mensagens dos usu�rios

## ?? Como Executar

### 1. Primeira Execu��o
```bash
dotnet run
```

O sistema ir�:
- Criar banco de dados SQLite
- Criar roles (Admin, User)
- Criar usu�rio admin padr�o
- Iniciar aplica��o

### 2. Acessar a Aplica��o
```
https://localhost:5001
```

### 3. Testar Autentica��o
1. Cadastrar novo usu�rio
2. Fazer login
3. Enviar mensagem
4. Fazer logout
5. Logar como admin
6. Acessar painel admin

## ?? Pr�ximos Passos Sugeridos

- [x] Implementar autentica��o com ASP.NET Core Identity
- [x] Adicionar autoriza��o baseada em roles (Admin)
- [x] Adicionar links de navega��o integrados
- [x] Melhorar CSS do formul�rio de mensagens
- [ ] Implementar recupera��o de senha via email
- [ ] Adicionar confirma��o de email
- [ ] Implementar notifica��es em tempo real (SignalR)
- [ ] Sistema de email para notificar respostas
- [ ] Analytics avan�ado de convers�o
- [ ] Integra��o com CRM
- [ ] Rate limiting para prevenir spam
- [ ] Cache para melhorar performance
- [ ] Badge de notifica��o para novas mensagens
- [ ] Upload de avatar de usu�rio
- [ ] Autentica��o de dois fatores (2FA)

## ?? Documenta��o Adicional

- [GUIA_AUTENTICACAO.md](docs/GUIA_AUTENTICACAO.md) - Guia completo de autentica��o
- [GUIA_NAVEGACAO.md](docs/GUIA_NAVEGACAO.md) - Guia de navega��o
- [GUIA_CONFIGURACAO_BACK4APP.md](docs/GUIA_CONFIGURACAO_BACK4APP.md) - Configura��o do Back4App

## ?? Contribuindo

Para adicionar novas funcionalidades:
1. Adicione o m�todo na interface do servi�o
2. Implemente a l�gica no servi�o
3. Crie a action no controller (com atributos de autoriza��o)
4. Desenvolva a view correspondente
5. Adicione links de navega��o se necess�rio
6. Teste com diferentes roles (User/Admin)

## ?? Suporte

Para d�vidas ou problemas:
- Documenta��o do Identity: https://docs.microsoft.com/aspnet/core/security/authentication/identity
- Documenta��o do Back4App: https://www.back4app.com/docs/

---

**Desenvolvido para o projeto Nossa TV**

? Agora com autentica��o completa e segura!
