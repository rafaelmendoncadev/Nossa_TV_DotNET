# Guia de Configura��o do Back4App

## ?? Pr�-requisitos

1. Conta no Back4App (https://www.back4app.com/)
2. Aplica��o criada no Back4App

## ?? Credenciais Configuradas

```
Application ID: z4XT6b7pn6D6TwfLjAkVImWCI6txKKF5fBJ9m2O3
Client Key: oxumxDTkBG21lfrZC1xuXpwc2F1975cSq54OGhVp
.NET Key: 5RlmAyaB4FTiVdoypuq1thY7QASn29E8BJRulLcd
Master Key: HvBfk8RaA4BPqD6zVUGZWM1ABK3wVDkhdbhgGpFr
```

## ??? Estrutura do Banco de Dados

### Tabela: Message

| Campo | Tipo | Descri��o |
|-------|------|-----------|
| objectId | String | ID �nico (auto-gerado) |
| userId | String | ID do usu�rio (nullable) |
| senderName | String | Nome do remetente |
| senderEmail | String | Email do remetente |
| subject | String | Assunto da mensagem |
| messageContent | String | Conte�do da mensagem |
| status | String | Status (New, Read, Replied, Archived) |
| isRead | Boolean | Se foi lida |
| readAt | Date | Data/hora da leitura |
| repliedAt | Date | Data/hora da resposta |
| createdAt | Date | Data/hora de cria��o (auto) |
| updatedAt | Date | Data/hora de atualiza��o (auto) |

### Tabela: MessageReply

| Campo | Tipo | Descri��o |
|-------|------|-----------|
| objectId | String | ID �nico (auto-gerado) |
| messageId | String | ID da mensagem original |
| adminUserId | String | ID do admin que respondeu |
| replyContent | String | Conte�do da resposta |
| createdAt | Date | Data/hora de cria��o (auto) |
| updatedAt | Date | Data/hora de atualiza��o (auto) |

### Tabela: Lead

| Campo | Tipo | Descri��o |
|-------|------|-----------|
| objectId | String | ID �nico (auto-gerado) |
| name | String | Nome do lead |
| email | String | Email do lead (deve ser �nico) |
| phone | String | Telefone (opcional) |
| firstContactDate | Date | Data do primeiro contato |
| lastContactDate | Date | Data do �ltimo contato |
| messageCount | Number | Total de mensagens enviadas |
| tags | Array | Tags para classifica��o |
| notes | String | Notas sobre o lead |
| createdAt | Date | Data/hora de cria��o (auto) |
| updatedAt | Date | Data/hora de atualiza��o (auto) |

## ?? Configura��o no Back4App Dashboard

### 1. Acessar o Dashboard
1. Fa�a login em https://www.back4app.com/
2. Selecione sua aplica��o
3. V� para "Database" > "Browser"

### 2. Criar as Classes (Tabelas)

#### Classe Message
1. Clique em "Create a class"
2. Nome: `Message`
3. Adicione os campos conforme tabela acima
4. Configure os tipos corretos para cada campo

#### Classe MessageReply
1. Clique em "Create a class"
2. Nome: `MessageReply`
3. Adicione os campos conforme tabela acima

#### Classe Lead
1. Clique em "Create a class"
2. Nome: `Lead`
3. Adicione os campos conforme tabela acima
4. **Importante**: Configure o campo `email` como �nico:
   - V� em "More" > "Index"
   - Crie um �ndice �nico para o campo `email`

### 3. Configurar Permiss�es (Class Level Permissions)

Para cada classe, configure as permiss�es:

**Development (Recomendado para teste):**
- Find: Public Read
- Get: Public Read  
- Create: Public Write
- Update: Authenticated Users
- Delete: Only Master Key

**Production (Recomendado):**
- Find: Authenticated Users / Master Key
- Get: Authenticated Users / Master Key
- Create: Authenticated Users / Master Key
- Update: Master Key
- Delete: Master Key

### 4. Criar �ndices para Performance

#### Classe Message
```
�ndices sugeridos:
- senderEmail (ascending)
- status (ascending)
- createdAt (descending)
- isRead (ascending)
```

#### Classe Lead
```
�ndices sugeridos:
- email (ascending, unique)
- lastContactDate (descending)
```

### 5. Cloud Code (Opcional)

Voc� pode adicionar valida��es server-side no Back4App:

```javascript
// beforeSave para Message
Parse.Cloud.beforeSave("Message", (request) => {
  const message = request.object;
  
  // Valida��o de email
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!emailRegex.test(message.get("senderEmail"))) {
    throw "Email inv�lido";
  }
  
  // Valida��o de conte�do
  if (!message.get("messageContent") || message.get("messageContent").length < 10) {
    throw "Mensagem muito curta";
  }
});

// afterSave para Message - atualizar lead
Parse.Cloud.afterSave("Message", async (request) => {
  const message = request.object;
  
  if (message.existed()) {
    return; // N�o processar updates
  }
  
  const email = message.get("senderEmail");
  const name = message.get("senderName");
  
  // Buscar ou criar lead
  const Lead = Parse.Object.extend("Lead");
  const query = new Parse.Query(Lead);
  query.equalTo("email", email);
  
  let lead = await query.first({ useMasterKey: true });
  
  if (lead) {
    // Atualizar lead existente
    lead.increment("messageCount");
    lead.set("lastContactDate", new Date());
  } else {
    // Criar novo lead
    lead = new Lead();
    lead.set("email", email);
    lead.set("name", name);
    lead.set("firstContactDate", new Date());
    lead.set("lastContactDate", new Date());
    lead.set("messageCount", 1);
    lead.set("tags", []);
  }
  
  await lead.save(null, { useMasterKey: true });
});
```

## ?? Seguran�a

### Vari�veis de Ambiente (Recomendado para Produ��o)

Em vez de hardcode no c�digo, use vari�veis de ambiente:

```json
// appsettings.json
{
  "Back4App": {
    "ApplicationId": "your-app-id",
    "ClientKey": "your-client-key",
    "MasterKey": "your-master-key"
  }
}
```

```csharp
// Program.cs
var back4AppConfig = builder.Configuration.GetSection("Back4App");
builder.Services.AddHttpClient("Back4App", client =>
{
    client.BaseAddress = new Uri("https://parseapi.back4app.com/");
    client.DefaultRequestHeaders.Add("X-Parse-Application-Id", back4AppConfig["ApplicationId"]);
    client.DefaultRequestHeaders.Add("X-Parse-REST-API-Key", back4AppConfig["ClientKey"]);
});
```

### Prote��o das Chaves

1. **Nunca** commite as chaves no reposit�rio p�blico
2. Use User Secrets para desenvolvimento:
   ```bash
   dotnet user-secrets set "Back4App:ApplicationId" "seu-app-id"
   dotnet user-secrets set "Back4App:ClientKey" "seu-client-key"
   dotnet user-secrets set "Back4App:MasterKey" "seu-master-key"
   ```
3. Configure as vari�veis de ambiente no servidor de produ��o

## ?? Monitoramento

### Analytics do Back4App

Acesse "Analytics" no dashboard para ver:
- N�mero de requisi��es
- Uso de armazenamento
- Erros de API
- Performance

### Logs

Acesse "Logs" para ver:
- Requisi��es em tempo real
- Erros e warnings
- Cloud Code logs

## ?? Testando a Conex�o

Execute o projeto e acesse:
```
http://localhost:5000/Messages/Send
```

Envie uma mensagem de teste. Verifique no Back4App Dashboard:
1. V� para "Database" > "Browser"
2. Selecione a classe "Message"
3. Voc� deve ver a mensagem criada

## ?? Troubleshooting

### Erro 403 - Forbidden
- Verifique se as credenciais est�o corretas
- Confira as permiss�es da classe

### Erro 404 - Not Found
- Verifique se a URL base est� correta
- Confirme se as classes foram criadas

### Erro 141 - Invalid Class Name
- O nome da classe n�o existe no Back4App
- Crie a classe no dashboard

### Dados n�o aparecem
- Verifique os logs no Back4App
- Confirme que a requisi��o foi bem-sucedida
- Verifique se os campos est�o corretos

## ?? Recursos Adicionais

- Documenta��o Back4App: https://www.back4app.com/docs/
- Parse REST API: https://docs.parseplatform.org/rest/guide/
- Parse Query: https://docs.parseplatform.org/rest/guide/#queries

---

? Sistema configurado e pronto para uso!
