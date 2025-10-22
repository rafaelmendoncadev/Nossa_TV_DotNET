# ??? Comandos Úteis - Nossa TV

## ?? Execução

### Executar em Desenvolvimento
```bash
dotnet run
```

### Executar em Produção
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

# Executar aplicação (cria banco automaticamente)
dotnet run
```

### Localização do Banco
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

### Admin Padrão
```
Email: admin@nossatv.com
Senha: Admin@123
```

### Criar Novo Usuário de Teste
```
1. Acesse: https://localhost:5001/Account/Register
2. Preencha o formulário
3. Login automático
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

### Ver Logs de Execução
```bash
dotnet run --verbosity detailed
```

## ?? Informações do Projeto

### Versão do .NET
```bash
dotnet --version
```

### Info do Projeto
```bash
dotnet --info
```

### Lista de Dependências
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

## ?? Arquivos de Configuração

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

### Publicar Aplicação
```bash
dotnet publish -c Release -o ./publish
```

### Executar Publicação
```bash
cd publish
dotnet Nossa_TV.dll
```

## ?? Limpeza

### Limpar Objetos Temporários
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

## ?? Documentação

### Ver Documentação Local
```
docs/README_SISTEMA_MENSAGENS.md
docs/GUIA_AUTENTICACAO.md
docs/INICIO_RAPIDO.md
docs/RESUMO_MUDANCAS.md
```

## ?? Erros Comuns e Soluções

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
# Parar aplicação
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

### CSS não carrega
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
git commit -m "Implementação de autenticação"
```

### Ver Diferenças
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
2. Fazer alterações
3. Salvar (hot reload automático)
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
1. Ctrl + C (parar aplicação)
2. rm nossatv.db
3. dotnet clean
4. dotnet restore
5. dotnet build
6. dotnet run
```

## ?? Suporte

### Logs de Erro
Localização: Console onde executou `dotnet run`

### Documentação Oficial
- ASP.NET Core: https://docs.microsoft.com/aspnet/core
- Entity Framework: https://docs.microsoft.com/ef/core
- Identity: https://docs.microsoft.com/aspnet/core/security/authentication/identity

---

**Dica:** Salve este arquivo nos favoritos para acesso rápido! ??
