# ?? In�cio R�pido - Nossa TV

## ? Executar a Aplica��o

```bash
cd Nossa_TV
dotnet run
```

Acesse: `https://localhost:5001`

## ?? Credenciais de Teste

### Administrador
- **Email:** admin@nossatv.com
- **Senha:** Admin@123
- **Acesso:** Painel Admin completo

### Novo Usu�rio
1. Clique em "Cadastre-se"
2. Preencha o formul�rio
3. Login autom�tico

## ? Checklist de Funcionalidades

### Para Testar como Usu�rio Comum:
- [ ] Cadastrar nova conta
- [ ] Fazer login
- [ ] Enviar mensagem via "Contato"
- [ ] Visualizar "Minhas Mensagens"
- [ ] Ver "�rea do Cliente" na navega��o (azul)
- [ ] Fazer logout

### Para Testar como Administrador:
- [ ] Fazer login com admin@nossatv.com
- [ ] Ver "Painel Admin" na navega��o (dourado)
- [ ] Acessar Dashboard
- [ ] Visualizar mensagens recebidas
- [ ] Responder uma mensagem
- [ ] Gerenciar leads
- [ ] Exportar leads para CSV

## ?? Fluxo Principal

```
1. Visitante ? Cadastre-se ? Preenche dados
                    ?
2. Sistema cria conta ? Login autom�tico
                    ?
3. Usu�rio autenticado ? V� "�rea do Cliente"
                    ?
4. Clica em "Contato" ? Envia mensagem
                    ?
5. Acessa "Minhas Mensagens" ? V� hist�rico
                    ?
6. Admin recebe ? Acessa "Painel Admin" ? Responde
```

## ?? Estrutura de Navega��o

### ?? N�o Autenticado
```
[Logo] Recursos | Planos | FAQ | ?? Entrar | ?? Cadastre-se | Assinar
```

### ?? Usu�rio Comum
```
[Logo] Recursos | Planos | FAQ | ?? Contato | ?? Minhas Mensagens | ?? �rea Cliente | [?? Nome ?]
```

### ?? Administrador
```
[Logo] Recursos | Planos | FAQ | ?? Contato | ?? Minhas Mensagens | ?? Painel Admin | [?? admin ?]
```

## ?? Banco de Dados

### SQLite Local (Autentica��o)
```
Nossa_TV/nossatv.db
```
Criado automaticamente com:
- Tabelas do Identity
- Roles (Admin, User)
- Usu�rio admin padr�o

### Back4App (Mensagens e Leads)
```
Classes:
- Message (mensagens dos usu�rios)
- MessageReply (respostas do admin)
- Lead (leads capturados)
```

## ?? CSS e Design

### Melhorias Implementadas
? Formul�rio de mensagens moderno  
? Cards com sombras e bordas arredondadas  
? Bot�es com gradientes  
? Alertas estilizados  
? Navega��o responsiva  
? Dropdown de usu�rio  

### Framework
- Bootstrap 5 (via CDN)
- Bootstrap Icons
- CSS customizado (landing.css)

## ?? Seguran�a

### Requisitos de Senha
- M�nimo 6 caracteres
- Pelo menos 1 d�gito
- Pelo menos 1 letra min�scula

### Prote��o de Rotas
```csharp
[Authorize]                    // Qualquer usu�rio autenticado
[Authorize(Roles = "Admin")]   // Apenas administradores
```

## ?? Troubleshooting

### Erro: "Banco de dados n�o existe"
```bash
# O banco � criado automaticamente na primeira execu��o
dotnet run
```

### CSS n�o carrega
1. Limpar cache (Ctrl + F5)
2. Verificar console do navegador
3. Confirmar que landing.css existe em wwwroot/css/

### N�o consigo logar como admin
1. Verificar se o banco foi criado
2. Usar exatamente: admin@nossatv.com / Admin@123
3. Verificar logs de inicializa��o

### Mensagem: "Por favor, fa�a login"
? **Esperado!** Agora � obrigat�rio estar autenticado para enviar mensagens.

## ?? Documenta��o Completa

- [README_SISTEMA_MENSAGENS.md](README_SISTEMA_MENSAGENS.md) - Vis�o geral completa
- [GUIA_AUTENTICACAO.md](GUIA_AUTENTICACAO.md) - Detalhes de autentica��o
- [GUIA_NAVEGACAO.md](GUIA_NAVEGACAO.md) - Guia de navega��o

## ?? Pr�ximos Passos

Depois de testar o b�sico:
1. Personalizar mensagens de erro
2. Adicionar recupera��o de senha
3. Implementar confirma��o de email
4. Adicionar foto de perfil
5. Criar �rea do cliente completa

---

**Pronto para come�ar? Execute `dotnet run` e teste! ??**
