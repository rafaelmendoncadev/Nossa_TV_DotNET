# ?? Guia de Navegação - Sistema de Mensagens

## Links Adicionados à Barra de Navegação

### ?? Localização
Os links foram integrados à barra de navegação principal do site, localizada em `Views/Shared/_Layout.cshtml`.

---

## ?? Links Disponíveis

### 1. **?? Contato** 
- **Rota:** `/Messages/Send`
- **Disponível para:** Todos os usuários (autenticados e anônimos)
- **Descrição:** Formulário para enviar mensagens/perguntas
- **Ícone:** `bi-envelope`
- **Código:**
```html
<a href="/Messages/Send" class="nav-link">
    <i class="bi bi-envelope"></i> Contato
</a>
```

---

### 2. **?? Minhas Mensagens**
- **Rota:** `/Messages/MyMessages`
- **Disponível para:** Usuários autenticados
- **Descrição:** Histórico de mensagens enviadas pelo usuário
- **Ícone:** `bi-inbox`
- **Condição:** Exibido apenas se `User.Identity?.IsAuthenticated == true`
- **Código:**
```html
@if (User.Identity?.IsAuthenticated == true)
{
    <a href="/Messages/MyMessages" class="nav-link">
        <i class="bi bi-inbox"></i> Minhas Mensagens
    </a>
}
```

---

### 3. **?? Painel Admin**
- **Rota:** `/Admin/AdminMessages/Dashboard`
- **Disponível para:** Administradores
- **Descrição:** Dashboard administrativo com estatísticas e gerenciamento
- **Ícone:** `bi-speedometer2`
- **Estilo:** Destaque com cor dourada (#ffd700)
- **Condição:** Exibido apenas se `User.IsInRole("Admin")`
- **Código:**
```html
@if (User.IsInRole("Admin"))
{
    <a href="/Admin/AdminMessages/Dashboard" class="nav-link" style="color: #ffd700;">
        <i class="bi bi-speedometer2"></i> Painel Admin
    </a>
}
```

---

### 4. **?? Nome do Usuário**
- **Disponível para:** Usuários autenticados
- **Descrição:** Exibe o nome do usuário logado
- **Ícone:** `bi-person-circle`
- **Código:**
```html
@if (User.Identity?.IsAuthenticated == true)
{
    <a href="#" class="nav-link">
        <i class="bi bi-person-circle"></i> @User.Identity.Name
    </a>
}
```

---

## ?? Estilos Aplicados

### CSS para Links com Ícones
```css
.nav-link i {
    margin-right: 0.25rem;
    font-size: 1rem;
}
```

### Destaque do Painel Admin
```css
.nav-link[style*="color: #ffd700"] {
    background: linear-gradient(135deg, rgba(255, 215, 0, 0.1), rgba(255, 215, 0, 0.2));
    padding: 0.5rem 1rem;
    border-radius: var(--radius-full);
    border: 1px solid rgba(255, 215, 0, 0.3);
}

.nav-link[style*="color: #ffd700"]:hover {
    background: linear-gradient(135deg, rgba(255, 215, 0, 0.2), rgba(255, 215, 0, 0.3));
    box-shadow: 0 4px 12px rgba(255, 215, 0, 0.2);
}
```

---

## ?? Responsividade

### Desktop
- Links exibidos horizontalmente na barra de navegação
- Ícones alinhados à esquerda do texto
- Efeitos hover suaves

### Mobile
- Menu hambúrguer ativado
- Links em coluna vertical
- Ícones com mais espaçamento
- Painel Admin ocupa largura total

---

## ??? Mapa de Navegação do Sistema

```
Home (/)
?
??? ?? Contato (/Messages/Send)
?   ??? POST: Enviar mensagem
?
??? ?? Área do Usuário (autenticado)
?   ??? ?? Minhas Mensagens (/Messages/MyMessages)
?   ?   ??? ?? Detalhes da Mensagem (/Messages/Detail/{id})
?   ?
?   ??? ?? Enviar Nova Mensagem (/Messages/Send)
?
??? ?? Área Administrativa (admin role)
    ??? ?? Dashboard (/Admin/AdminMessages/Dashboard)
    ?   ??? Estatísticas gerais
    ?   ??? Ações rápidas
    ?
    ??? ?? Gerenciar Mensagens (/Admin/AdminMessages)
    ?   ??? Lista com filtros
    ?   ??? ?? Detalhes e Resposta (/Admin/AdminMessages/Detail/{id})
    ?   ??? ? Marcar como Lida
    ?   ??? ?? Responder
    ?   ??? ?? Arquivar
    ?   ??? ??? Excluir
    ?
    ??? ?? Gerenciar Leads (/Admin/AdminLeads)
        ??? Lista com busca e filtros
        ??? ?? Detalhes do Lead (/Admin/AdminLeads/Detail/{id})
        ??? ??? Adicionar/Remover Tags
        ??? ?? Atualizar Notas
        ??? ?? Exportar CSV
```

---

## ?? Fluxo de Navegação do Usuário

### Usuário Anônimo
1. Acessa o site
2. Clica em **?? Contato** na navegação
3. Preenche e envia mensagem
4. Recebe confirmação

### Usuário Autenticado
1. Faz login
2. Visualiza **?? Minhas Mensagens** na navegação
3. Pode acessar histórico de mensagens
4. Pode enviar novas mensagens via **?? Contato**
5. Recebe notificação visual de respostas

### Administrador
1. Faz login com conta admin
2. Visualiza **?? Painel Admin** (destaque dourado)
3. Acessa dashboard com estatísticas
4. Gerencia mensagens e leads
5. Pode exportar dados

---

## ?? Benefícios da Navegação Integrada

### ? Para Usuários
- Acesso rápido ao formulário de contato
- Fácil visualização do histórico de mensagens
- Interface intuitiva com ícones

### ? Para Administradores
- Acesso direto ao painel administrativo
- Visual destacado para identificação rápida
- Navegação eficiente entre módulos

### ? Para o Sistema
- UX consistente em todo o site
- Navegação responsiva
- Fácil manutenção e expansão

---

## ??? Customização

### Adicionar Novo Link
1. Edite `Views/Shared/_Layout.cshtml`
2. Adicione o link dentro de `.nav-menu`
3. Aplique condições se necessário (`@if`)
4. Adicione ícone do Bootstrap Icons
5. Teste responsividade

### Exemplo de Novo Link:
```html
<a href="/NovaPagina" class="nav-link">
    <i class="bi bi-star"></i> Nova Funcionalidade
</a>
```

---

## ?? Recursos

- **Bootstrap Icons:** https://icons.getbootstrap.com/
- **Layout Principal:** `Views/Shared/_Layout.cshtml`
- **Estilos:** `wwwroot/css/landing.css`
- **JavaScript:** `wwwroot/js/landing.js`

---

## ? Checklist de Implementação

- [x] Link de Contato para todos os usuários
- [x] Link de Minhas Mensagens para autenticados
- [x] Link do Painel Admin para administradores
- [x] Ícones Bootstrap Icons
- [x] Estilos responsivos
- [x] Destaque visual para admin
- [x] Link no footer para contato
- [x] Documentação completa

---

**Desenvolvido para Nossa TV - Sistema de Mensagens**
