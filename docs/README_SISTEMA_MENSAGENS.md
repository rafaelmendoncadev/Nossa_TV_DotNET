# Sistema de Mensagens com Painel Administrativo - Nossa TV

Sistema completo de gerenciamento de mensagens e leads integrado com Back4App, **agora com autenticação completa usando ASP.NET Core Identity**.

## ?? **NOVIDADE: Sistema de Autenticação**

### Credenciais de Acesso
**Usuário Administrador Padrão:**
- Email: `admin@nossatv.com`
- Senha: `Admin@123`

### Funcionalidades de Autenticação
- ? Cadastro de novos usuários
- ? Login seguro com email e senha
- ? Logout com proteção CSRF
- ? Controle de acesso baseado em roles (Admin/User)
- ? Navegação dinâmica baseada em autenticação
- ? Proteção de rotas sensíveis
- ? Sessão persistente com "Lembrar-me"

### **IMPORTANTE: Autenticação Obrigatória**
?? **Agora é necessário fazer login antes de enviar mensagens!**

Os usuários devem:
1. Se cadastrar em `/Account/Register` OU
2. Fazer login em `/Account/Login`
3. Depois acessar o formulário de mensagens

## ?? Funcionalidades Implementadas

### ? Sistema de Mensagens do Usuário
- **Formulário de contato (requer autenticação)**
- Validação de dados no cliente e servidor
- Feedback visual após envio
- Histórico de mensagens para usuários autenticados
- CSS moderno e responsivo

### ? Painel Administrativo - Mensagens
- Listagem de todas as mensagens recebidas
- Filtros por status (Nova, Lida, Respondida, Arquivada) e data
- Marcar mensagens como lida
- Responder mensagens
- Arquivar ou excluir mensagens
- Dashboard com estatísticas completas
- **Restrito a usuários com role "Admin"**

### ? Painel Administrativo - Leads
- Lista de todos os leads capturados automaticamente
- Informações detalhadas: nome, email, telefone, quantidade de mensagens
- Sistema de tags para classificação
- Notas personalizadas por lead
- Histórico completo de interações
- Exportação para CSV
- **Restrito a usuários com role "Admin"**

### ? Navegação Integrada e Dinâmica
A barra de navegação se adapta ao estado de autenticação:

#### Usuário NÃO Autenticado:
- Links públicos (Recursos, Planos, FAQ)
- **Entrar** - Link para login
- **Cadastre-se** - Link para registro
- Assinar Agora

#### Usuário Comum Autenticado:
- ?? **Contato** - Formulário de mensagens
- ?? **Minhas Mensagens** - Histórico pessoal
- ?? **Área do Cliente** - Área exclusiva (destaque em azul)
- ?? Menu dropdown com nome do usuário
  - Meu Perfil
  - Sair

#### Administrador Autenticado:
- ?? **Contato** - Formulário de mensagens
- ?? **Minhas Mensagens** - Histórico pessoal
- ?? **Painel Admin** - Dashboard administrativo (destaque em dourado)
- ?? Menu dropdown com nome do usuário
  - Meu Perfil
  - Sair

## ??? Estrutura do Projeto

```
Nossa_TV/
??? Controllers/
?   ??? AccountController.cs               # Login, Registro, Logout
?   ??? MessagesController.cs              # Área pública de mensagens (autenticação obrigatória)
?   ??? Admin/
?       ??? AdminMessagesController.cs     # Gerenciamento admin de mensagens (role Admin)
?       ??? AdminLeadsController.cs        # Gerenciamento de leads (role Admin)
??? Models/
?   ??? ApplicationUser.cs                 # Modelo de usuário (Identity)
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
?   ??? MessageService.cs                  # Lógica de negócio de mensagens
?   ??? ILeadService.cs
?   ??? LeadService.cs                     # Lógica de negócio de leads
??? Views/
    ??? Account/
    ?   ??? Login.cshtml                   # Página de login
    ?   ??? Register.cshtml                # Página de cadastro
    ?   ??? AccessDenied.cshtml            # Acesso negado
    ??? Messages/
    ?   ??? Send.cshtml                    # Formulário de envio (CSS melhorado)
    ?   ??? MyMessages.cshtml              # Histórico do usuário
    ?   ??? Detail.cshtml                  # Detalhes da mensagem
    ??? Admin/
        ??? AdminMessages/
        ?   ??? Index.cshtml               # Lista de mensagens
        ?   ??? Detail.cshtml              # Detalhes e resposta
        ?   ??? Dashboard.cshtml           # Estatísticas
        ??? AdminLeads/
            ??? Index.cshtml               # Lista de leads
            ??? Detail.cshtml              # Detalhes do lead
```

## ?? Configuração

### Banco de Dados - SQLite
O sistema usa SQLite para autenticação local:
```
Data Source=nossatv.db
```

O banco de dados é criado automaticamente na primeira execução, incluindo:
- Tabelas do Identity (Users, Roles, UserRoles, etc.)
- Roles padrão (Admin, User)
- Usuário administrador padrão

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

## ?? Navegação e Rotas

### Área Pública
- `GET /` - Página inicial
- `GET /Account/Register` - Cadastro de novo usuário
- `GET /Account/Login` - Login de usuário
- `POST /Account/Logout` - Logout (requer autenticação)

### Área de Mensagens (Requer Autenticação)
- `GET /Messages/Send` - Formulário de envio de mensagem
- `POST /Messages/Send` - Processar envio de mensagem
- `GET /Messages/MyMessages` - Histórico de mensagens
- `GET /Messages/Detail/{id}` - Detalhes de uma mensagem específica

### Área Admin - Mensagens (Requer Role "Admin")
- `GET /Admin/AdminMessages` - Lista de mensagens
- `GET /Admin/AdminMessages/Dashboard` - Dashboard com estatísticas
- `GET /Admin/AdminMessages/Detail/{id}` - Detalhes da mensagem
- `POST /Admin/AdminMessages/Reply/{id}` - Responder mensagem
- `POST /Admin/AdminMessages/MarkAsRead/{id}` - Marcar como lida
- `POST /Admin/AdminMessages/Archive/{id}` - Arquivar mensagem
- `POST /Admin/AdminMessages/Delete/{id}` - Excluir mensagem

### Área Admin - Leads (Requer Role "Admin")
- `GET /Admin/AdminLeads` - Lista de leads
- `GET /Admin/AdminLeads/Detail/{id}` - Detalhes do lead
- `POST /Admin/AdminLeads/AddTag` - Adicionar tag
- `POST /Admin/AdminLeads/RemoveTag` - Remover tag
- `POST /Admin/AdminLeads/UpdateNotes` - Atualizar notas
- `GET /Admin/AdminLeads/Export` - Exportar leads para CSV

## ?? CSS e Interface

### Melhorias no CSS
- ? Formulário de mensagens com design moderno
- ? Cards com sombras e bordas arredondadas
- ? Botões com gradientes e animações
- ? Alertas com bordas coloridas e ícones
- ? Formulários de autenticação estilizados
- ? Dropdown de usuário com transições suaves
- ? Navegação responsiva para mobile

### Bootstrap 5
Carregado via CDN no `_Layout.cshtml`:
```html
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
```

## ?? Segurança

### Configurações de Senha
- Mínimo 6 caracteres
- Requer dígito
- Requer letra minúscula
- Não requer letra maiúscula
- Não requer caractere especial

### Proteção de Rotas
```csharp
[Authorize]                    // Requer autenticação
[Authorize(Roles = "Admin")]   // Requer role específica
```

### Anti-Forgery
Todos os formulários incluem tokens CSRF:
```csharp
@Html.AntiForgeryToken()
[ValidateAntiForgeryToken]
```

### Lockout
- Máximo 5 tentativas falhas
- Bloqueio de 5 minutos após exceder

## ?? Fluxo Completo de Uso

### 1. Primeiro Acesso (Novo Usuário)
1. Usuário acessa o site
2. Clica em "Cadastre-se"
3. Preenche: Nome, Email, Senha
4. Sistema cria conta e faz login automaticamente
5. Usuário é redirecionado para home (autenticado)
6. Vê "Área do Cliente" na navegação

### 2. Enviar Mensagem
1. Usuário autenticado clica em "Contato"
2. Preenche formulário com design moderno
3. Mensagem é enviada para Back4App
4. Lead é capturado/atualizado automaticamente
5. Feedback de sucesso é exibido
6. Pode acessar "Minhas Mensagens" para histórico

### 3. Administrador
1. Faz login com admin@nossatv.com
2. Vê "Painel Admin" (dourado) na navegação
3. Acessa dashboard com estatísticas
4. Gerencia mensagens e leads
5. Responde mensagens dos usuários

## ?? Como Executar

### 1. Primeira Execução
```bash
dotnet run
```

O sistema irá:
- Criar banco de dados SQLite
- Criar roles (Admin, User)
- Criar usuário admin padrão
- Iniciar aplicação

### 2. Acessar a Aplicação
```
https://localhost:5001
```

### 3. Testar Autenticação
1. Cadastrar novo usuário
2. Fazer login
3. Enviar mensagem
4. Fazer logout
5. Logar como admin
6. Acessar painel admin

## ?? Próximos Passos Sugeridos

- [x] Implementar autenticação com ASP.NET Core Identity
- [x] Adicionar autorização baseada em roles (Admin)
- [x] Adicionar links de navegação integrados
- [x] Melhorar CSS do formulário de mensagens
- [ ] Implementar recuperação de senha via email
- [ ] Adicionar confirmação de email
- [ ] Implementar notificações em tempo real (SignalR)
- [ ] Sistema de email para notificar respostas
- [ ] Analytics avançado de conversão
- [ ] Integração com CRM
- [ ] Rate limiting para prevenir spam
- [ ] Cache para melhorar performance
- [ ] Badge de notificação para novas mensagens
- [ ] Upload de avatar de usuário
- [ ] Autenticação de dois fatores (2FA)

## ?? Documentação Adicional

- [GUIA_AUTENTICACAO.md](docs/GUIA_AUTENTICACAO.md) - Guia completo de autenticação
- [GUIA_NAVEGACAO.md](docs/GUIA_NAVEGACAO.md) - Guia de navegação
- [GUIA_CONFIGURACAO_BACK4APP.md](docs/GUIA_CONFIGURACAO_BACK4APP.md) - Configuração do Back4App

## ?? Contribuindo

Para adicionar novas funcionalidades:
1. Adicione o método na interface do serviço
2. Implemente a lógica no serviço
3. Crie a action no controller (com atributos de autorização)
4. Desenvolva a view correspondente
5. Adicione links de navegação se necessário
6. Teste com diferentes roles (User/Admin)

## ?? Suporte

Para dúvidas ou problemas:
- Documentação do Identity: https://docs.microsoft.com/aspnet/core/security/authentication/identity
- Documentação do Back4App: https://www.back4app.com/docs/

---

**Desenvolvido para o projeto Nossa TV**

? Agora com autenticação completa e segura!
