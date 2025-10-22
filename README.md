# ?? Nossa TV - Sistema Completo de Streaming

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=flat-square)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=flat-square&logo=bootstrap)
![SQLite](https://img.shields.io/badge/SQLite-3-003B57?style=flat-square&logo=sqlite)

Plataforma moderna de streaming com +400 canais ao vivo, sistema completo de autentica��o e painel administrativo.

## ? In�cio R�pido

```bash
# Clone o reposit�rio
cd Nossa_TV

# Execute a aplica��o
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
- Compat�vel com TV, Mobile, Tablet e Desktop
- Interface moderna e responsiva

### ?? Autentica��o
- Sistema completo com ASP.NET Core Identity
- Cadastro e login seguro
- Controle de acesso baseado em roles
- Sess�o persistente com "Lembrar-me"

### ?? Sistema de Mensagens
- Formul�rio de contato (requer autentica��o)
- Hist�rico de mensagens pessoais
- Valida��o client e server-side
- Integra��o com Back4App

### ?? Painel Administrativo
- Dashboard com estat�sticas
- Gerenciamento de mensagens
- Sistema de leads
- Exporta��o para CSV
- Restrito a administradores

### ?? Interface Moderna
- Design responsivo com Bootstrap 5
- CSS customizado com gradientes
- �cones do Bootstrap Icons
- Navega��o din�mica baseada em autentica��o

## ?? Pr�-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Navegador moderno (Chrome, Firefox, Edge, Safari)

## ?? Instala��o

### 1. Clone o Reposit�rio
```bash
git clone https://github.com/seu-usuario/Nossa_TV.git
cd Nossa_TV
```

### 2. Restaurar Depend�ncias
```bash
dotnet restore
```

### 3. Executar a Aplica��o
```bash
dotnet run
```

### 4. Acessar a Aplica��o
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
??? Services/            # L�gica de neg�cio
?   ??? MessageService.cs
?   ??? LeadService.cs
??? Data/                # Contexto EF
?   ??? ApplicationDbContext.cs
??? wwwroot/             # Arquivos est�ticos
?   ??? css/
?   ??? js/
?   ??? lib/
??? docs/                # Documenta��o
```

## ?? Funcionalidades por Tipo de Usu�rio

### ?? Visitante (N�o Autenticado)
- ? Visualizar p�gina inicial
- ? Ver recursos e planos
- ? Ler FAQ
- ? Cadastrar-se
- ? Fazer login

### ?? Usu�rio Comum (Autenticado)
- ? Todas as funcionalidades de visitante
- ? Enviar mensagens
- ? Ver hist�rico de mensagens
- ? Acessar �rea do cliente
- ? Gerenciar perfil

### ?? Administrador
- ? Todas as funcionalidades de usu�rio
- ? Acessar painel administrativo
- ? Visualizar dashboard com estat�sticas
- ? Gerenciar todas as mensagens
- ? Responder mensagens
- ? Gerenciar leads
- ? Exportar dados para CSV

## ?? Tecnologias Utilizadas

### Backend
- ASP.NET Core 8.0 (Razor Pages)
- Entity Framework Core 8.0
- ASP.NET Core Identity
- SQLite (Autentica��o)
- Back4App (Mensagens e Leads)

### Frontend
- Bootstrap 5.3
- Bootstrap Icons
- JavaScript (Vanilla)
- CSS3 com vari�veis e gradientes
- jQuery Validation

### Seguran�a
- HTTPS enforcement
- Anti-Forgery tokens
- Password hashing
- Role-based authorization
- Input sanitization

## ?? Documenta��o

- [?? Sistema de Mensagens](docs/README_SISTEMA_MENSAGENS.md)
- [?? Guia de Autentica��o](docs/GUIA_AUTENTICACAO.md)
- [? In�cio R�pido](docs/INICIO_RAPIDO.md)
- [?? Resumo de Mudan�as](docs/RESUMO_MUDANCAS.md)
- [??? Comandos �teis](docs/COMANDOS_UTEIS.md)

## ?? Configura��o

### Banco de Dados
O sistema usa SQLite para autentica��o local:
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
- Senha m�nima: 6 caracteres
- Requer d�gito: Sim
- Requer min�scula: Sim
- Lockout: 5 tentativas / 5 minutos
- Cookie v�lido por: 7 dias
```

## ?? Testes

### Testar como Usu�rio Comum
1. Cadastrar nova conta em `/Account/Register`
2. Preencher formul�rio de cadastro
3. Login autom�tico ap�s cadastro
4. Enviar mensagem via "Contato"
5. Visualizar hist�rico em "Minhas Mensagens"

### Testar como Administrador
1. Fazer login com `admin@nossatv.com` / `Admin@123`
2. Acessar "Painel Admin" (link dourado)
3. Visualizar dashboard com estat�sticas
4. Gerenciar mensagens recebidas
5. Responder mensagens de usu�rios
6. Exportar leads para CSV

## ?? Screenshots

### Navega��o - N�o Autenticado
```
[Logo] Recursos | Planos | FAQ | Entrar | Cadastre-se | Assinar
```

### Navega��o - Usu�rio Comum
```
[Logo] Recursos | Planos | FAQ | Contato | Minhas Msgs | �rea Cliente | [?? Nome ?]
```

### Navega��o - Administrador
```
[Logo] Recursos | Planos | FAQ | Contato | Minhas Msgs | Painel Admin | [?? admin ?]
```

## ?? Seguran�a

### Pr�ticas Implementadas
- ? Autentica��o com ASP.NET Core Identity
- ? Autoriza��o baseada em roles
- ? HTTPS obrigat�rio
- ? Anti-Forgery tokens em formul�rios
- ? Password hashing autom�tico
- ? Prote��o contra XSS
- ? Valida��o de entrada
- ? Lockout ap�s tentativas falhas

### Recomenda��es para Produ��o
- ?? Trocar credenciais do admin
- ?? Configurar email para recupera��o de senha
- ?? Habilitar confirma��o de email
- ?? Implementar 2FA para admins
- ?? Adicionar rate limiting
- ?? Configurar CORS apropriadamente

## ?? Deploy

### Build para Produ��o
```bash
dotnet publish -c Release -o ./publish
```

### Executar Publica��o
```bash
cd publish
dotnet Nossa_TV.dll
```

### Vari�veis de Ambiente
```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=https://+:443;http://+:80
```

## ?? Troubleshooting

### Banco de dados n�o criado
```bash
rm nossatv.db
dotnet run
```

### CSS n�o carrega
```bash
# Limpar cache do navegador
Ctrl + Shift + R

# Verificar arquivo
ls wwwroot/css/landing.css
```

### Erro de autentica��o
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

### ? Conclu�do
- [x] Sistema de autentica��o completo
- [x] Controle de acesso por roles
- [x] Painel administrativo
- [x] Sistema de mensagens
- [x] Gerenciamento de leads
- [x] Interface responsiva
- [x] Navega��o din�mica

### ?? Pr�ximos Passos
- [ ] Recupera��o de senha via email
- [ ] Confirma��o de email
- [ ] Upload de avatar
- [ ] Notifica��es em tempo real
- [ ] Chat ao vivo
- [ ] Autentica��o 2FA
- [ ] Login com redes sociais
- [ ] API REST para mobile
- [ ] Integra��o com CRM
- [ ] Analytics avan�ado

## ?? Contribuindo

Contribui��es s�o bem-vindas! Para contribuir:

1. Fork o projeto
2. Crie uma branch (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudan�as (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## ?? Licen�a

Este projeto est� sob a licen�a MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## ?? Autores

- **Equipe Nossa TV** - *Desenvolvimento inicial*

## ?? Suporte

- ?? Email: suporte@nossatv.com
- ?? WhatsApp: (Bot�o flutuante no site)
- ?? Documenta��o: [/docs](docs/)

## ?? Agradecimentos

- Bootstrap pela UI framework
- Back4App pelo backend como servi�o
- Microsoft pelo .NET e ASP.NET Core
- Comunidade open source

---

? **Desenvolvido com ?? pela equipe Nossa TV**

?? **Vers�o atual:** 1.0.0  
?? **�ltima atualiza��o:** Janeiro 2025
