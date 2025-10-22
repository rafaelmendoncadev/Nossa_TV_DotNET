# ??? Comandos �teis - Nossa TV

## ?? Execu��o

### Executar em Desenvolvimento
```bash
dotnet run
```

### Executar em Produ��o
```bash
dotnet run --configuration Release
```

### Watch Mode (Hot Reload)
```bash
dotnet watch run
```

## ??? Banco de Dados

### Recriar Banco de Dados
```bash
# Deletar banco existente
rm nossatv.db

# Executar aplica��o (cria banco automaticamente)
dotnet run
```

### Localiza��o do Banco
```
Nossa_TV/nossatv.db
```

## ?? Pacotes NuGet

### Listar Pacotes Instalados
```bash
dotnet list package
```

### Atualizar Pacotes
```bash
dotnet restore
```

### Adicionar Novo Pacote
```bash
dotnet add package NomeDoPacote
```

## ?? Testes

### Build
```bash
dotnet build
```

### Build em Release
```bash
dotnet build --configuration Release
```

### Limpar Build
```bash
dotnet clean
```

## ?? Credenciais de Teste

### Admin Padr�o
```
Email: admin@nossatv.com
Senha: Admin@123
```

### Criar Novo Usu�rio de Teste
```
1. Acesse: https://localhost:5001/Account/Register
2. Preencha o formul�rio
3. Login autom�tico
```

## ?? Troubleshooting

### Limpar Cache do Navegador
```
Ctrl + Shift + R (Windows/Linux)
Cmd + Shift + R (Mac)
```

### Recompilar Projeto
```bash
dotnet clean
dotnet build
dotnet run
```

### Resetar Banco de Dados
```bash
# Windows
del nossatv.db
dotnet run

# Linux/Mac
rm nossatv.db
dotnet run
```

### Ver Logs de Execu��o
```bash
dotnet run --verbosity detailed
```

## ?? Informa��es do Projeto

### Vers�o do .NET
```bash
dotnet --version
```

### Info do Projeto
```bash
dotnet --info
```

### Lista de Depend�ncias
```bash
dotnet list package --include-transitive
```

## ?? URLs Importantes

### Desenvolvimento
```
HTTP:  http://localhost:5000
HTTPS: https://localhost:5001
```

### Rotas Principais
```
Home:           /
Login:          /Account/Login
Cadastro:       /Account/Register
Contato:        /Messages/Send
Minhas Msgs:    /Messages/MyMessages
Admin Panel:    /Admin/AdminMessages/Dashboard
Leads:          /Admin/AdminLeads
```

## ?? Desenvolvimento CSS

### Arquivo CSS Principal
```
wwwroot/css/landing.css
```

### Recarregar CSS (sem restart)
```
Ctrl + F5 no navegador
```

### Verificar CSS no Navegador
```
F12 ? Console ? Verificar erros
F12 ? Network ? Verificar se landing.css carregou
```

## ?? Arquivos de Configura��o

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=nossatv.db"
  }
}
```

### Back4App Config (Program.cs)
```csharp
X-Parse-Application-Id: z4XT6b7pn6D6TwfLjAkVImWCI6txKKF5fBJ9m2O3
X-Parse-REST-API-Key: oxumxDTkBG21lfrZC1xuXpwc2F1975cSq54OGhVp
```

## ?? Debug

### Executar com Debug
```bash
# Visual Studio
F5

# VS Code
Ctrl + F5
```

### Ver Logs em Tempo Real
```bash
dotnet run --verbosity normal
```

## ?? Deploy

### Publicar Aplica��o
```bash
dotnet publish -c Release -o ./publish
```

### Executar Publica��o
```bash
cd publish
dotnet Nossa_TV.dll
```

## ?? Limpeza

### Limpar Objetos Tempor�rios
```bash
dotnet clean
```

### Deletar Bin e Obj
```bash
# Windows
rmdir /s /q bin obj

# Linux/Mac
rm -rf bin obj
```

## ?? Documenta��o

### Ver Documenta��o Local
```
docs/README_SISTEMA_MENSAGENS.md
docs/GUIA_AUTENTICACAO.md
docs/INICIO_RAPIDO.md
docs/RESUMO_MUDANCAS.md
```

## ?? Erros Comuns e Solu��es

### Erro: "Port already in use"
```bash
# Windows
netstat -ano | findstr :5001
taskkill /PID <PID> /F

# Linux/Mac
lsof -i :5001
kill -9 <PID>
```

### Erro: "Database locked"
```bash
# Parar aplica��o
Ctrl + C

# Deletar banco
rm nossatv.db

# Executar novamente
dotnet run
```

### Erro: "Unable to build project graph"
```bash
dotnet clean
dotnet restore
dotnet build
```

### CSS n�o carrega
```bash
# Limpar cache do navegador
Ctrl + Shift + Delete

# Verificar arquivo existe
ls wwwroot/css/landing.css

# Recompilar
dotnet build
```

## ?? Git Commands (opcional)

### Status
```bash
git status
```

### Commit
```bash
git add .
git commit -m "Implementa��o de autentica��o"
```

### Ver Diferen�as
```bash
git diff
```

## ?? Monitoramento

### Ver Processos ASP.NET
```bash
# Windows
tasklist | findstr dotnet

# Linux/Mac
ps aux | grep dotnet
```

### Matar Processo
```bash
# Windows
taskkill /IM dotnet.exe /F

# Linux/Mac
pkill -9 dotnet
```

## ?? Workflows Comuns

### Desenvolvimento Normal
```bash
1. dotnet watch run
2. Fazer altera��es
3. Salvar (hot reload autom�tico)
4. Testar no navegador
```

### Deploy Completo
```bash
1. dotnet clean
2. dotnet restore
3. dotnet build --configuration Release
4. dotnet test (se houver testes)
5. dotnet publish -c Release
```

### Reset Completo
```bash
1. Ctrl + C (parar aplica��o)
2. rm nossatv.db
3. dotnet clean
4. dotnet restore
5. dotnet build
6. dotnet run
```

## ?? Suporte

### Logs de Erro
Localiza��o: Console onde executou `dotnet run`

### Documenta��o Oficial
- ASP.NET Core: https://docs.microsoft.com/aspnet/core
- Entity Framework: https://docs.microsoft.com/ef/core
- Identity: https://docs.microsoft.com/aspnet/core/security/authentication/identity

---

**Dica:** Salve este arquivo nos favoritos para acesso r�pido! ??
