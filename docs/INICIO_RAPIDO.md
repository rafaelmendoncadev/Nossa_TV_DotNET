# ?? Início Rápido - Nossa TV

## ? Executar a Aplicação

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

### Novo Usuário
1. Clique em "Cadastre-se"
2. Preencha o formulário
3. Login automático

## ? Checklist de Funcionalidades

### Para Testar como Usuário Comum:
- [ ] Cadastrar nova conta
- [ ] Fazer login
- [ ] Enviar mensagem via "Contato"
- [ ] Visualizar "Minhas Mensagens"
- [ ] Ver "Área do Cliente" na navegação (azul)
- [ ] Fazer logout

### Para Testar como Administrador:
- [ ] Fazer login com admin@nossatv.com
- [ ] Ver "Painel Admin" na navegação (dourado)
- [ ] Acessar Dashboard
- [ ] Visualizar mensagens recebidas
- [ ] Responder uma mensagem
- [ ] Gerenciar leads
- [ ] Exportar leads para CSV

## ?? Fluxo Principal

```
1. Visitante ? Cadastre-se ? Preenche dados
                    ?
2. Sistema cria conta ? Login automático
                    ?
3. Usuário autenticado ? Vê "Área do Cliente"
                    ?
4. Clica em "Contato" ? Envia mensagem
                    ?
5. Acessa "Minhas Mensagens" ? Vê histórico
                    ?
6. Admin recebe ? Acessa "Painel Admin" ? Responde
```

## ?? Estrutura de Navegação

### ?? Não Autenticado
```
[Logo] Recursos | Planos | FAQ | ?? Entrar | ?? Cadastre-se | Assinar
```

### ?? Usuário Comum
```
[Logo] Recursos | Planos | FAQ | ?? Contato | ?? Minhas Mensagens | ?? Área Cliente | [?? Nome ?]
```

### ?? Administrador
```
[Logo] Recursos | Planos | FAQ | ?? Contato | ?? Minhas Mensagens | ?? Painel Admin | [?? admin ?]
```

## ?? Banco de Dados

### SQLite Local (Autenticação)
```
Nossa_TV/nossatv.db
```
Criado automaticamente com:
- Tabelas do Identity
- Roles (Admin, User)
- Usuário admin padrão

### Back4App (Mensagens e Leads)
```
Classes:
- Message (mensagens dos usuários)
- MessageReply (respostas do admin)
- Lead (leads capturados)
```

## ?? CSS e Design

### Melhorias Implementadas
? Formulário de mensagens moderno  
? Cards com sombras e bordas arredondadas  
? Botões com gradientes  
? Alertas estilizados  
? Navegação responsiva  
? Dropdown de usuário  

### Framework
- Bootstrap 5 (via CDN)
- Bootstrap Icons
- CSS customizado (landing.css)

## ?? Segurança

### Requisitos de Senha
- Mínimo 6 caracteres
- Pelo menos 1 dígito
- Pelo menos 1 letra minúscula

### Proteção de Rotas
```csharp
[Authorize]                    // Qualquer usuário autenticado
[Authorize(Roles = "Admin")]   // Apenas administradores
```

## ?? Troubleshooting

### Erro: "Banco de dados não existe"
```bash
# O banco é criado automaticamente na primeira execução
dotnet run
```

### CSS não carrega
1. Limpar cache (Ctrl + F5)
2. Verificar console do navegador
3. Confirmar que landing.css existe em wwwroot/css/

### Não consigo logar como admin
1. Verificar se o banco foi criado
2. Usar exatamente: admin@nossatv.com / Admin@123
3. Verificar logs de inicialização

### Mensagem: "Por favor, faça login"
? **Esperado!** Agora é obrigatório estar autenticado para enviar mensagens.

## ?? Documentação Completa

- [README_SISTEMA_MENSAGENS.md](README_SISTEMA_MENSAGENS.md) - Visão geral completa
- [GUIA_AUTENTICACAO.md](GUIA_AUTENTICACAO.md) - Detalhes de autenticação
- [GUIA_NAVEGACAO.md](GUIA_NAVEGACAO.md) - Guia de navegação

## ?? Próximos Passos

Depois de testar o básico:
1. Personalizar mensagens de erro
2. Adicionar recuperação de senha
3. Implementar confirmação de email
4. Adicionar foto de perfil
5. Criar área do cliente completa

---

**Pronto para começar? Execute `dotnet run` e teste! ??**
