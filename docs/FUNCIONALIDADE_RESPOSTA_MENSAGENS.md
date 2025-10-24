# Resposta a Mensagens - Guia da Nova Funcionalidade

## ?? Vis�o Geral

A funcionalidade de **Resposta a Mensagens** permite que usu�rios fa�am novas perguntas/d�vidas ap�s receber uma resposta do administrador, sem precisar enviar uma mensagem completamente nova.

Este guia explica como a funcionalidade funciona, como us�-la e como foi implementada.

---

## ? Benef�cios

- ? **Continuidade de Conversa**: Usu�rios podem fazer perguntas de acompanhamento facilmente
- ? **Rastreabilidade**: Todas as perguntas e respostas relacionadas ficam agrupadas
- ? **Melhor UX**: Interface intuitiva e clara
- ? **Hist�rico Preservado**: O contexto original da conversa � mantido
- ? **Automa��o**: Novas mensagens s�o criadas automaticamente com "Re:" no assunto

---

## ?? Como Usar - Perspectiva do Usu�rio

### Fluxo Passo a Passo

1. **Enviar Pergunta Inicial**
   - Usu�rio autenticado acessa `Messages/Send`
   - Preenche o formul�rio com pergunta
   - Clica em "Enviar Mensagem"

2. **Administrador Responde**
   - Admin acessa `Admin/AdminMessages/Detail/{messageId}`
   - L� a pergunta do usu�rio
   - Clica em "Responder" e digita a resposta
   - A mensagem muda para status "Respondida"

3. **Usu�rio V� a Resposta**
   - Usu�rio acessa `Messages/MyMessages`
   - Clica em "Detalhes" da mensagem respondida
   - A resposta do admin � exibida em um card azul

4. **Usu�rio Faz Nova Pergunta** ? *NOVA FUNCIONALIDADE*
   - Abaixo da resposta, aparece um formul�rio "Tem mais alguma d�vida?"
   - Usu�rio digita a nova pergunta no textarea
   - Clica em "Enviar Pergunta"
   - O sistema cria uma nova mensagem com:
     - Assunto: `Re: [Assunto Original]`
     - Conte�do: A pergunta do usu�rio
     - Status: "Nova" (aguardando resposta do admin)
   - Admin ver� essa nova mensagem no painel

### Exemplo Pr�tico

```
Pergunta Original:
- Assunto: "Como instalar a aplica��o?"
- Conte�do: "Qual � o passo a passo?"

Resposta do Admin:
- "Veja o guia em nosso site..."

Pergunta de Acompanhamento:
- Assunto: "Re: Como instalar a aplica��o?"
- Conte�do: "E depois que instalei, como fa�o login?"

Nova Resposta:
- Admin ver� a nova pergunta e responde
```

---

## ??? Implementa��o T�cnica

### Arquivos Modificados

#### 1. **Views/Messages/Detail.cshtml**
```razor
<!-- Novo formul�rio para responder ap�s receber resposta -->
<div class="card mt-4 border-success">
    <div class="card-header bg-light">
        <h5 class="mb-0">
            <i class="bi bi-chat-left-text me-2"></i>Tem mais alguma d�vida?
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

**Caracter�sticas:**
- Formul�rio aparece apenas se houver respostas do admin
- Campo de textarea com placeholder intuitivo
- Bot�o verde destacado com �cone
- Valida��o required no cliente
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
    
    // Verificar se pertence ao usu�rio
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

**L�gica:**
1. Valida o modelo recebido
2. Obt�m a mensagem original para recuperar dados
3. Verifica se a mensagem pertence ao usu�rio (seguran�a)
4. Confirma que h� respostas (n�o deve haver sem resposta)
5. Cria uma nova `SendMessageViewModel` com:
   - Mesmo nome e email do original
   - Assunto prefixado com "Re:"
   - Conte�do da pergunta do usu�rio
6. Envia a nova mensagem usando o servi�o existente
7. Redireciona com mensagem de sucesso

#### 3. **ViewModels/UserReplyViewModel.cs** (NOVO)
```csharp
public class UserReplyViewModel
{
    [Required(ErrorMessage = "A pergunta � obrigat�ria")]
    [StringLength(2000, ErrorMessage = "A pergunta deve ter no m�ximo 2000 caracteres")]
    public string ReplyContent { get; set; } = string.Empty;

    public string OriginalMessageId { get; set; } = string.Empty;
}
```

**Prop�sito:**
- Valida��o server-side do conte�do da resposta
- M�ximo 2000 caracteres
- Mensagem obrigat�ria

#### 4. **Views/_ViewImports.cshtml**
```csharp
@using Nossa_TV.ViewModels  // Adicionado para acesso aos ViewModels
```

---

## ?? Seguran�a Implementada

### 1. **Autoriza��o**
```csharp
[Authorize]  // Apenas usu�rios autenticados
```

### 2. **Verifica��o de Propriedade**
```csharp
if (originalMessage.UserId != userId)
    return Forbid();  // Retorna 403 Forbidden
```

### 3. **Token Anti-CSRF**
```html
@Html.AntiForgeryToken()
[ValidateAntiForgeryToken]
```

### 4. **Valida��o do Modelo**
- Required
- StringLength (m�ximo 2000 caracteres)
- Mensagens de erro customizadas

### 5. **Verifica��o de Estado**
```csharp
if (originalMessage.Replies.Count == 0)
    return RedirectToAction("Detail", new { id });
    // N�o permite resposta a uma mensagem sem respostas pr�vias
```

---

## ?? Fluxo de Dados

```
???????????????
?   Usu�rio   ?
???????????????
       ?
       ??? Acessa Messages/Detail/{messageId}
       ?   ??? View carrega AdminMessageDetailViewModel
       ?       ??? Se Replies.Count > 0, mostra formul�rio
       ?
       ??? Preenche formul�rio "Tem mais alguma d�vida?"
       ?   ??? Clica "Enviar Pergunta"
       ?
       ??? POST para Messages/Reply/{messageId}
       ?   ??? UserReplyViewModel � validado
       ?
       ??? MessagesController.Reply() processa:
       ?   ??? Obt�m mensagem original
       ?   ??? Valida propriedade
       ?   ??? Cria SendMessageViewModel
       ?   ??? Chama _messageService.SendMessageAsync()
       ?
       ??? MessageService cria nova Message:
       ?   ??? Subject = "Re: [Original]"
       ?   ??? MessageContent = pergunta do usu�rio
       ?   ??? UserId = userId do usu�rio
       ?   ??? Status = "New"
       ?   ??? Salva no banco
       ?
       ??? Redireciona para MyMessages com sucesso
```

---

## ?? Banco de Dados

### Tabela `messages` (SQLite)

Nenhuma altera��o na estrutura - reutiliza campos existentes:

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
- Cada pergunta e resposta � uma linha separada na tabela
- O campo `subject` com "Re:" permite agrupar relacionadas
- O campo `user_id` vincula ao usu�rio
- O campo `created_at` mant�m a ordem cronol�gica

---

## ?? Integra��o com Sistema Existente

### Reutiliza Componentes

1. **IMessageService.SendMessageAsync()**
   - M�todo j� existente
   - Faz todo o trabalho pesado
   - Captura/atualiza leads automaticamente

2. **SendMessageViewModel**
   - Classe j� existente
   - Valida��o j� implementada
   - Comportamento conhecido

3. **Banco de Dados**
   - Mesma tabela `messages`
   - Mesma estrutura
   - Compat�vel 100%

### N�o Quebra Nada

- ? Views antigas funcionam normalmente
- ? AdminMessagesController n�o foi alterado
- ? Rotas existentes n�o mudaram
- ? Dados hist�ricos n�o foram afetados

---

## ?? Interface do Usu�rio

### Quando o Formul�rio Aparece

```
? Aparece quando: A mensagem tem PELO MENOS 1 resposta do admin
? N�o aparece quando:
   - A mensagem ainda n�o foi respondida
   - O usu�rio est� vendo sua pr�pria pergunta original
```

### Estilo Visual

- **Cor de Borda**: Verde (`border-success`)
- **Cor do Header**: Cinza claro (`bg-light`)
- **�cone**: `bi-chat-left-text` (�cone de chat)
- **Bot�o**: Verde com �cone de envio (`btn-success`)

---

## ?? Cen�rios de Teste

### ? Teste 1: Fluxo Completo
1. Criar usu�rio novo
2. Enviar mensagem
3. Fazer login com admin
4. Responder mensagem
5. Fazer logout admin
6. Logar como usu�rio
7. Acessar `Messages/Detail/{messageId}`
8. Verificar que formul�rio aparece
9. Enviar pergunta de acompanhamento
10. Verificar que nova mensagem foi criada

### ? Teste 2: Valida��o
1. Enviar pergunta vazia ? Erro "Campo obrigat�rio"
2. Enviar pergunta com 2001+ caracteres ? Erro de tamanho
3. Acessar resposta de outro usu�rio ? Retorna Forbid

### ? Teste 3: Hist�rico
1. Enviar pergunta inicial
2. Admin responde
3. Usu�rio faz segunda pergunta
4. Admin responde segunda
5. Em `MyMessages`, verificar que h� 2 mensagens
6. Clicar na primeira, ver resposta e formul�rio
7. Clicar na segunda, ver resposta e formul�rio

---

## ?? Como Estender

### Adicionar Campo Customizado

Se quiser adicionar um campo � resposta do usu�rio:

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

Para notificar o admin quando h� resposta:

```csharp
// Ap�s SendMessageAsync retornar true
if (result)
{
    await _emailService.SendAdminNotificationAsync(
        "Nova pergunta de acompanhamento",
        $"Usu�rio {originalMessage.SenderName} respondeu a: {originalMessage.Subject}"
    );
}
```

---

## ?? Estat�sticas

### Queries �teis para Analytics

```sql
-- Quantas mensagens t�m respostas
SELECT COUNT(*) FROM messages WHERE status = 'Replied';

-- Quantas perguntas de acompanhamento foram feitas
SELECT COUNT(*) FROM messages WHERE subject LIKE 'Re:%';

-- Tempo m�dio para primeira resposta
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

### Problema: Formul�rio n�o aparece
**Solu��o**: Verifique se:
- A mensagem tem `Replies.Count > 0`
- Voc� est� autenticado como o usu�rio que enviou
- A view est� carregando `AdminMessageDetailViewModel`

### Problema: Erro ao enviar pergunta
**Solu��o**: Verifique:
- Token CSRF v�lido
- Conte�do n�o vazio
- Conte�do < 2000 caracteres
- Banco de dados conectado

### Problema: Perdeu o contexto da conversa
**Solu��o**: Uso de "Re:" no assunto permite rastreamento visual

---

## ?? Refer�ncias

- [ASP.NET Core Razor Pages](https://docs.microsoft.com/aspnet/core/razor-pages)
- [Authorization in ASP.NET Core](https://docs.microsoft.com/aspnet/core/security/authorization)
- [ASP.NET Core Identity](https://docs.microsoft.com/aspnet/core/security/authentication/identity)
- [Form Validation](https://docs.microsoft.com/aspnet/core/mvc/models/validation)

---

## ? Checklist de Funcionalidade

- [x] Implementar m�todo Reply no controller
- [x] Criar UserReplyViewModel com valida��o
- [x] Adicionar formul�rio na view Detail.cshtml
- [x] Testar seguran�a (autoriza��o)
- [x] Testar valida��o (modelo inv�lido)
- [x] Tester fluxo completo
- [x] Adicionar documenta��o
- [x] Compila��o sem erros
- [x] Interface amig�vel

---

**Data de Implementa��o**: 2024
**Status**: ? Em Produ��o
**Vers�o**: 1.0

