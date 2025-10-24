# Resposta a Mensagens - Guia da Nova Funcionalidade

## ?? Visão Geral

A funcionalidade de **Resposta a Mensagens** permite que usuários façam novas perguntas/dúvidas após receber uma resposta do administrador, sem precisar enviar uma mensagem completamente nova.

Este guia explica como a funcionalidade funciona, como usá-la e como foi implementada.

---

## ? Benefícios

- ? **Continuidade de Conversa**: Usuários podem fazer perguntas de acompanhamento facilmente
- ? **Rastreabilidade**: Todas as perguntas e respostas relacionadas ficam agrupadas
- ? **Melhor UX**: Interface intuitiva e clara
- ? **Histórico Preservado**: O contexto original da conversa é mantido
- ? **Automação**: Novas mensagens são criadas automaticamente com "Re:" no assunto

---

## ?? Como Usar - Perspectiva do Usuário

### Fluxo Passo a Passo

1. **Enviar Pergunta Inicial**
   - Usuário autenticado acessa `Messages/Send`
   - Preenche o formulário com pergunta
   - Clica em "Enviar Mensagem"

2. **Administrador Responde**
   - Admin acessa `Admin/AdminMessages/Detail/{messageId}`
   - Lê a pergunta do usuário
   - Clica em "Responder" e digita a resposta
   - A mensagem muda para status "Respondida"

3. **Usuário Vê a Resposta**
   - Usuário acessa `Messages/MyMessages`
   - Clica em "Detalhes" da mensagem respondida
   - A resposta do admin é exibida em um card azul

4. **Usuário Faz Nova Pergunta** ? *NOVA FUNCIONALIDADE*
   - Abaixo da resposta, aparece um formulário "Tem mais alguma dúvida?"
   - Usuário digita a nova pergunta no textarea
   - Clica em "Enviar Pergunta"
   - O sistema cria uma nova mensagem com:
     - Assunto: `Re: [Assunto Original]`
     - Conteúdo: A pergunta do usuário
     - Status: "Nova" (aguardando resposta do admin)
   - Admin verá essa nova mensagem no painel

### Exemplo Prático

```
Pergunta Original:
- Assunto: "Como instalar a aplicação?"
- Conteúdo: "Qual é o passo a passo?"

Resposta do Admin:
- "Veja o guia em nosso site..."

Pergunta de Acompanhamento:
- Assunto: "Re: Como instalar a aplicação?"
- Conteúdo: "E depois que instalei, como faço login?"

Nova Resposta:
- Admin verá a nova pergunta e responde
```

---

## ??? Implementação Técnica

### Arquivos Modificados

#### 1. **Views/Messages/Detail.cshtml**
```razor
<!-- Novo formulário para responder após receber resposta -->
<div class="card mt-4 border-success">
    <div class="card-header bg-light">
        <h5 class="mb-0">
            <i class="bi bi-chat-left-text me-2"></i>Tem mais alguma dúvida?
        </h5>
    </div>
    <div class="card-body">
        <form asp-action="Reply" asp-route-id="@Model.Id" method="post">
            @Html.AntiForgeryToken()
            <textarea name="ReplyContent" 
                      class="form-control" 
                      rows="4" 
                      placeholder="Digite sua pergunta aqui..."
                      required></textarea>
            <button type="submit" class="btn btn-success mt-3">
                <i class="bi bi-send me-1"></i>Enviar Pergunta
            </button>
        </form>
    </div>
</div>
```

**Características:**
- Formulário aparece apenas se houver respostas do admin
- Campo de textarea com placeholder intuitivo
- Botão verde destacado com ícone
- Validação required no cliente
- Protegido por token CSRF

#### 2. **Controllers/MessagesController.cs**
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
[Authorize]
public async Task<IActionResult> Reply(string id, [FromForm] UserReplyViewModel model)
{
    // Validar modelo
    if (!ModelState.IsValid) return RedirectToAction("Detail", new { id });

    // Obter mensagem original
    var originalMessage = await _messageService.GetMessageDetailAsync(id);
    
    // Verificar se pertence ao usuário
    var userId = User.Identity!.Name;
    if (originalMessage.UserId != userId)
        return Forbid();

    // Verificar se foi respondida
    if (originalMessage.Replies.Count == 0)
        return RedirectToAction("Detail", new { id });

    // Criar nova mensagem com "Re:" no assunto
    var newMessage = new SendMessageViewModel
    {
        SenderName = originalMessage.SenderName,
        SenderEmail = originalMessage.SenderEmail,
        Subject = $"Re: {originalMessage.Subject}",
        MessageContent = model.ReplyContent
    };

    var result = await _messageService.SendMessageAsync(newMessage, userId);

    if (result)
    {
        TempData["SuccessMessage"] = "Pergunta enviada com sucesso!";
        return RedirectToAction("MyMessages");
    }

    TempData["ErrorMessage"] = "Erro ao enviar sua pergunta.";
    return RedirectToAction("Detail", new { id });
}
```

**Lógica:**
1. Valida o modelo recebido
2. Obtém a mensagem original para recuperar dados
3. Verifica se a mensagem pertence ao usuário (segurança)
4. Confirma que há respostas (não deve haver sem resposta)
5. Cria uma nova `SendMessageViewModel` com:
   - Mesmo nome e email do original
   - Assunto prefixado com "Re:"
   - Conteúdo da pergunta do usuário
6. Envia a nova mensagem usando o serviço existente
7. Redireciona com mensagem de sucesso

#### 3. **ViewModels/UserReplyViewModel.cs** (NOVO)
```csharp
public class UserReplyViewModel
{
    [Required(ErrorMessage = "A pergunta é obrigatória")]
    [StringLength(2000, ErrorMessage = "A pergunta deve ter no máximo 2000 caracteres")]
    public string ReplyContent { get; set; } = string.Empty;

    public string OriginalMessageId { get; set; } = string.Empty;
}
```

**Propósito:**
- Validação server-side do conteúdo da resposta
- Máximo 2000 caracteres
- Mensagem obrigatória

#### 4. **Views/_ViewImports.cshtml**
```csharp
@using Nossa_TV.ViewModels  // Adicionado para acesso aos ViewModels
```

---

## ?? Segurança Implementada

### 1. **Autorização**
```csharp
[Authorize]  // Apenas usuários autenticados
```

### 2. **Verificação de Propriedade**
```csharp
if (originalMessage.UserId != userId)
    return Forbid();  // Retorna 403 Forbidden
```

### 3. **Token Anti-CSRF**
```html
@Html.AntiForgeryToken()
[ValidateAntiForgeryToken]
```

### 4. **Validação do Modelo**
- Required
- StringLength (máximo 2000 caracteres)
- Mensagens de erro customizadas

### 5. **Verificação de Estado**
```csharp
if (originalMessage.Replies.Count == 0)
    return RedirectToAction("Detail", new { id });
    // Não permite resposta a uma mensagem sem respostas prévias
```

---

## ?? Fluxo de Dados

```
???????????????
?   Usuário   ?
???????????????
       ?
       ??? Acessa Messages/Detail/{messageId}
       ?   ??? View carrega AdminMessageDetailViewModel
       ?       ??? Se Replies.Count > 0, mostra formulário
       ?
       ??? Preenche formulário "Tem mais alguma dúvida?"
       ?   ??? Clica "Enviar Pergunta"
       ?
       ??? POST para Messages/Reply/{messageId}
       ?   ??? UserReplyViewModel é validado
       ?
       ??? MessagesController.Reply() processa:
       ?   ??? Obtém mensagem original
       ?   ??? Valida propriedade
       ?   ??? Cria SendMessageViewModel
       ?   ??? Chama _messageService.SendMessageAsync()
       ?
       ??? MessageService cria nova Message:
       ?   ??? Subject = "Re: [Original]"
       ?   ??? MessageContent = pergunta do usuário
       ?   ??? UserId = userId do usuário
       ?   ??? Status = "New"
       ?   ??? Salva no banco
       ?
       ??? Redireciona para MyMessages com sucesso
```

---

## ?? Banco de Dados

### Tabela `messages` (SQLite)

Nenhuma alteração na estrutura - reutiliza campos existentes:

```sql
CREATE TABLE messages (
    id TEXT PRIMARY KEY,
    user_id TEXT,
    sender_name TEXT,
    sender_email TEXT,
    subject TEXT,      -- "Re: Assunto Original"
    message_content TEXT,
    status TEXT,        -- "New"
    is_read BOOLEAN,
    read_at DATETIME,
    replied_at DATETIME,
    created_at DATETIME,
    updated_at DATETIME
);
```

**Como a resposta fica registrada:**
- Cada pergunta e resposta é uma linha separada na tabela
- O campo `subject` com "Re:" permite agrupar relacionadas
- O campo `user_id` vincula ao usuário
- O campo `created_at` mantém a ordem cronológica

---

## ?? Integração com Sistema Existente

### Reutiliza Componentes

1. **IMessageService.SendMessageAsync()**
   - Método já existente
   - Faz todo o trabalho pesado
   - Captura/atualiza leads automaticamente

2. **SendMessageViewModel**
   - Classe já existente
   - Validação já implementada
   - Comportamento conhecido

3. **Banco de Dados**
   - Mesma tabela `messages`
   - Mesma estrutura
   - Compatível 100%

### Não Quebra Nada

- ? Views antigas funcionam normalmente
- ? AdminMessagesController não foi alterado
- ? Rotas existentes não mudaram
- ? Dados históricos não foram afetados

---

## ?? Interface do Usuário

### Quando o Formulário Aparece

```
? Aparece quando: A mensagem tem PELO MENOS 1 resposta do admin
? Não aparece quando:
   - A mensagem ainda não foi respondida
   - O usuário está vendo sua própria pergunta original
```

### Estilo Visual

- **Cor de Borda**: Verde (`border-success`)
- **Cor do Header**: Cinza claro (`bg-light`)
- **Ícone**: `bi-chat-left-text` (ícone de chat)
- **Botão**: Verde com ícone de envio (`btn-success`)

---

## ?? Cenários de Teste

### ? Teste 1: Fluxo Completo
1. Criar usuário novo
2. Enviar mensagem
3. Fazer login com admin
4. Responder mensagem
5. Fazer logout admin
6. Logar como usuário
7. Acessar `Messages/Detail/{messageId}`
8. Verificar que formulário aparece
9. Enviar pergunta de acompanhamento
10. Verificar que nova mensagem foi criada

### ? Teste 2: Validação
1. Enviar pergunta vazia ? Erro "Campo obrigatório"
2. Enviar pergunta com 2001+ caracteres ? Erro de tamanho
3. Acessar resposta de outro usuário ? Retorna Forbid

### ? Teste 3: Histórico
1. Enviar pergunta inicial
2. Admin responde
3. Usuário faz segunda pergunta
4. Admin responde segunda
5. Em `MyMessages`, verificar que há 2 mensagens
6. Clicar na primeira, ver resposta e formulário
7. Clicar na segunda, ver resposta e formulário

---

## ?? Como Estender

### Adicionar Campo Customizado

Se quiser adicionar um campo à resposta do usuário:

1. **Atualizar UserReplyViewModel**
```csharp
public class UserReplyViewModel
{
    public string ReplyContent { get; set; }
    public string? Priority { get; set; }  // Novo campo
}
```

2. **Atualizar a View**
```html
<select name="Priority" class="form-control">
    <option>Normal</option>
    <option>Urgente</option>
</select>
```

3. **Usar no Controller**
```csharp
var newMessage = new SendMessageViewModel
{
    // ... dados existentes ...
    Subject = $"Re: [P: {model.Priority}] {originalMessage.Subject}"
};
```

### Notificar Admin

Para notificar o admin quando há resposta:

```csharp
// Após SendMessageAsync retornar true
if (result)
{
    await _emailService.SendAdminNotificationAsync(
        "Nova pergunta de acompanhamento",
        $"Usuário {originalMessage.SenderName} respondeu a: {originalMessage.Subject}"
    );
}
```

---

## ?? Estatísticas

### Queries Úteis para Analytics

```sql
-- Quantas mensagens têm respostas
SELECT COUNT(*) FROM messages WHERE status = 'Replied';

-- Quantas perguntas de acompanhamento foram feitas
SELECT COUNT(*) FROM messages WHERE subject LIKE 'Re:%';

-- Tempo médio para primeira resposta
SELECT AVG(julianday(replied_at) - julianday(created_at)) 
FROM messages 
WHERE replied_at IS NOT NULL;

-- Taxa de acompanhamento (perguntas/respostas)
SELECT COUNT(*) FILTER (WHERE subject LIKE 'Re:%') as follow_ups,
       COUNT(*) FILTER (WHERE status = 'Replied') as replied
FROM messages;
```

---

## ?? Troubleshooting

### Problema: Formulário não aparece
**Solução**: Verifique se:
- A mensagem tem `Replies.Count > 0`
- Você está autenticado como o usuário que enviou
- A view está carregando `AdminMessageDetailViewModel`

### Problema: Erro ao enviar pergunta
**Solução**: Verifique:
- Token CSRF válido
- Conteúdo não vazio
- Conteúdo < 2000 caracteres
- Banco de dados conectado

### Problema: Perdeu o contexto da conversa
**Solução**: Uso de "Re:" no assunto permite rastreamento visual

---

## ?? Referências

- [ASP.NET Core Razor Pages](https://docs.microsoft.com/aspnet/core/razor-pages)
- [Authorization in ASP.NET Core](https://docs.microsoft.com/aspnet/core/security/authorization)
- [ASP.NET Core Identity](https://docs.microsoft.com/aspnet/core/security/authentication/identity)
- [Form Validation](https://docs.microsoft.com/aspnet/core/mvc/models/validation)

---

## ? Checklist de Funcionalidade

- [x] Implementar método Reply no controller
- [x] Criar UserReplyViewModel com validação
- [x] Adicionar formulário na view Detail.cshtml
- [x] Testar segurança (autorização)
- [x] Testar validação (modelo inválido)
- [x] Tester fluxo completo
- [x] Adicionar documentação
- [x] Compilação sem erros
- [x] Interface amigável

---

**Data de Implementação**: 2024
**Status**: ? Em Produção
**Versão**: 1.0

