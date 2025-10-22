# ?? Nossa TV - Sistema Completo de Streaming

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=flat-square)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=flat-square&logo=bootstrap)
![SQLite](https://img.shields.io/badge/SQLite-3-003B57?style=flat-square&logo=sqlite)

Plataforma moderna de streaming com +400 canais ao vivo, sistema completo de autenticação e painel administrativo.

## ? Início Rápido

```bash
# Clone o repositório
cd Nossa_TV

# Execute a aplicação
dotnet run

# Acesse no navegador
https://localhost:5001
```

### ?? Credenciais de Teste
**Administrador:**
- Email: `admin@nossatv.com`
- Senha: `Admin@123`

## ? Funcionalidades Principais

### ?? Streaming
- +400 canais ao vivo
- Duas telas simultaneamente
- Compatível com TV, Mobile, Tablet e Desktop
- Interface moderna e responsiva

### ?? Autenticação
- Sistema completo com ASP.NET Core Identity
- Cadastro e login seguro
- Controle de acesso baseado em roles
- Sessão persistente com "Lembrar-me"

### ?? Sistema de Mensagens
- Formulário de contato (requer autenticação)
- Histórico de mensagens pessoais
- Validação client e server-side
- Integração com Back4App

### ?? Painel Administrativo
- Dashboard com estatísticas
- Gerenciamento de mensagens
- Sistema de leads
- Exportação para CSV
- Restrito a administradores

### ?? Interface Moderna
- Design responsivo com Bootstrap 5
- CSS customizado com gradientes
- Ícones do Bootstrap Icons
- Navegação dinâmica baseada em autenticação

## ?? Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Navegador moderno (Chrome, Firefox, Edge, Safari)

## ?? Instalação

### 1. Clone o Repositório
```bash
git clone https://github.com/seu-usuario/Nossa_TV.git
cd Nossa_TV
```

### 2. Restaurar Dependências
```bash
dotnet restore
```

### 3. Executar a Aplicação
```bash
dotnet run
```

### 4. Acessar a Aplicação
```
HTTP:  http://localhost:5000
HTTPS: https://localhost:5001
```

## ?? Estrutura do Projeto

```
Nossa_TV/
??? Controllers/           # Controllers MVC
?   ??? AccountController.cs
?   ??? MessagesController.cs
?   ??? HomeController.cs
?   ??? Admin/
??? Models/               # Modelos de dados
?   ??? ApplicationUser.cs
?   ??? Message.cs
?   ??? Lead.cs
??? ViewModels/          # ViewModels
??? Views/               # Views Razor
?   ??? Account/
?   ??? Messages/
?   ??? Admin/
??? Services/            # Lógica de negócio
?   ??? MessageService.cs
?   ??? LeadService.cs
??? Data/                # Contexto EF
?   ??? ApplicationDbContext.cs
??? wwwroot/             # Arquivos estáticos
?   ??? css/
?   ??? js/
?   ??? lib/
??? docs/                # Documentação
```

## ?? Funcionalidades por Tipo de Usuário

### ?? Visitante (Não Autenticado)
- ? Visualizar página inicial
- ? Ver recursos e planos
- ? Ler FAQ
- ? Cadastrar-se
- ? Fazer login

### ?? Usuário Comum (Autenticado)
- ? Todas as funcionalidades de visitante
- ? Enviar mensagens
- ? Ver histórico de mensagens
- ? Acessar área do cliente
- ? Gerenciar perfil

### ?? Administrador
- ? Todas as funcionalidades de usuário
- ? Acessar painel administrativo
- ? Visualizar dashboard com estatísticas
- ? Gerenciar todas as mensagens
- ? Responder mensagens
- ? Gerenciar leads
- ? Exportar dados para CSV

## ?? Tecnologias Utilizadas

### Backend
- ASP.NET Core 8.0 (Razor Pages)
- Entity Framework Core 8.0
- ASP.NET Core Identity
- SQLite (Autenticação)
- Back4App (Mensagens e Leads)

### Frontend
- Bootstrap 5.3
- Bootstrap Icons
- JavaScript (Vanilla)
- CSS3 com variáveis e gradientes
- jQuery Validation

### Segurança
- HTTPS enforcement
- Anti-Forgery tokens
- Password hashing
- Role-based authorization
- Input sanitization

## ?? Documentação

- [?? Sistema de Mensagens](docs/README_SISTEMA_MENSAGENS.md)
- [?? Guia de Autenticação](docs/GUIA_AUTENTICACAO.md)
- [? Início Rápido](docs/INICIO_RAPIDO.md)
- [?? Resumo de Mudanças](docs/RESUMO_MUDANCAS.md)
- [??? Comandos Úteis](docs/COMANDOS_UTEIS.md)

## ?? Configuração

### Banco de Dados
O sistema usa SQLite para autenticação local:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=nossatv.db"
  }
}
```

### Back4App (Mensagens)
Configurado em `Program.cs`:
```csharp
X-Parse-Application-Id: z4XT6b7pn6D6TwfLjAkVImWCI6txKKF5fBJ9m2O3
X-Parse-REST-API-Key: oxumxDTkBG21lfrZC1xuXpwc2F1975cSq54OGhVp
```

### Identity Settings
```csharp
- Senha mínima: 6 caracteres
- Requer dígito: Sim
- Requer minúscula: Sim
- Lockout: 5 tentativas / 5 minutos
- Cookie válido por: 7 dias
```

## ?? Testes

### Testar como Usuário Comum
1. Cadastrar nova conta em `/Account/Register`
2. Preencher formulário de cadastro
3. Login automático após cadastro
4. Enviar mensagem via "Contato"
5. Visualizar histórico em "Minhas Mensagens"

### Testar como Administrador
1. Fazer login com `admin@nossatv.com` / `Admin@123`
2. Acessar "Painel Admin" (link dourado)
3. Visualizar dashboard com estatísticas
4. Gerenciar mensagens recebidas
5. Responder mensagens de usuários
6. Exportar leads para CSV

## ?? Screenshots

### Navegação - Não Autenticado
```
[Logo] Recursos | Planos | FAQ | Entrar | Cadastre-se | Assinar
```

### Navegação - Usuário Comum
```
[Logo] Recursos | Planos | FAQ | Contato | Minhas Msgs | Área Cliente | [?? Nome ?]
```

### Navegação - Administrador
```
[Logo] Recursos | Planos | FAQ | Contato | Minhas Msgs | Painel Admin | [?? admin ?]
```

## ?? Segurança

### Práticas Implementadas
- ? Autenticação com ASP.NET Core Identity
- ? Autorização baseada em roles
- ? HTTPS obrigatório
- ? Anti-Forgery tokens em formulários
- ? Password hashing automático
- ? Proteção contra XSS
- ? Validação de entrada
- ? Lockout após tentativas falhas

### Recomendações para Produção
- ?? Trocar credenciais do admin
- ?? Configurar email para recuperação de senha
- ?? Habilitar confirmação de email
- ?? Implementar 2FA para admins
- ?? Adicionar rate limiting
- ?? Configurar CORS apropriadamente

## ?? Deploy

### Build para Produção
```bash
dotnet publish -c Release -o ./publish
```

### Executar Publicação
```bash
cd publish
dotnet Nossa_TV.dll
```

### Variáveis de Ambiente
```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=https://+:443;http://+:80
```

## ?? Troubleshooting

### Banco de dados não criado
```bash
rm nossatv.db
dotnet run
```

### CSS não carrega
```bash
# Limpar cache do navegador
Ctrl + Shift + R

# Verificar arquivo
ls wwwroot/css/landing.css
```

### Erro de autenticação
```bash
# Resetar banco
rm nossatv.db
dotnet run
```

### Porta em uso
```bash
# Windows
netstat -ano | findstr :5001
taskkill /PID <PID> /F

# Linux/Mac
lsof -i :5001
kill -9 <PID>
```

## ?? Roadmap

### ? Concluído
- [x] Sistema de autenticação completo
- [x] Controle de acesso por roles
- [x] Painel administrativo
- [x] Sistema de mensagens
- [x] Gerenciamento de leads
- [x] Interface responsiva
- [x] Navegação dinâmica

### ?? Próximos Passos
- [ ] Recuperação de senha via email
- [ ] Confirmação de email
- [ ] Upload de avatar
- [ ] Notificações em tempo real
- [ ] Chat ao vivo
- [ ] Autenticação 2FA
- [ ] Login com redes sociais
- [ ] API REST para mobile
- [ ] Integração com CRM
- [ ] Analytics avançado

## ?? Contribuindo

Contribuições são bem-vindas! Para contribuir:

1. Fork o projeto
2. Crie uma branch (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## ?? Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## ?? Autores

- **Equipe Nossa TV** - *Desenvolvimento inicial*

## ?? Suporte

- ?? Email: suporte@nossatv.com
- ?? WhatsApp: (Botão flutuante no site)
- ?? Documentação: [/docs](docs/)

## ?? Agradecimentos

- Bootstrap pela UI framework
- Back4App pelo backend como serviço
- Microsoft pelo .NET e ASP.NET Core
- Comunidade open source

---

? **Desenvolvido com ?? pela equipe Nossa TV**

?? **Versão atual:** 1.0.0  
?? **Última atualização:** Janeiro 2025
