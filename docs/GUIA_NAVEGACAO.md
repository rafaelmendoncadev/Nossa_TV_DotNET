# ?? Guia de Navega��o - Sistema de Mensagens

## Links Adicionados � Barra de Navega��o

### ?? Localiza��o
Os links foram integrados � barra de navega��o principal do site, localizada em `Views/Shared/_Layout.cshtml`.

---

## ?? Links Dispon�veis

### 1. **?? Contato** 
- **Rota:** `/Messages/Send`
- **Dispon�vel para:** Todos os usu�rios (autenticados e an�nimos)
- **Descri��o:** Formul�rio para enviar mensagens/perguntas
- **�cone:** `bi-envelope`
- **C�digo:**
```html
<a href="/Messages/Send" class="nav-link">
    <i class="bi bi-envelope"></i> Contato
</a>
```

---

### 2. **?? Minhas Mensagens**
- **Rota:** `/Messages/MyMessages`
- **Dispon�vel para:** Usu�rios autenticados
- **Descri��o:** Hist�rico de mensagens enviadas pelo usu�rio
- **�cone:** `bi-inbox`
- **Condi��o:** Exibido apenas se `User.Identity?.IsAuthenticated == true`
- **C�digo:**
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
- **Dispon�vel para:** Administradores
- **Descri��o:** Dashboard administrativo com estat�sticas e gerenciamento
- **�cone:** `bi-speedometer2`
- **Estilo:** Destaque com cor dourada (#ffd700)
- **Condi��o:** Exibido apenas se `User.IsInRole("Admin")`
- **C�digo:**
```html
@if (User.IsInRole("Admin"))
{
    <a href="/Admin/AdminMessages/Dashboard" class="nav-link" style="color: #ffd700;">
        <i class="bi bi-speedometer2"></i> Painel Admin
    </a>
}
```

---

### 4. **?? Nome do Usu�rio**
- **Dispon�vel para:** Usu�rios autenticados
- **Descri��o:** Exibe o nome do usu�rio logado
- **�cone:** `bi-person-circle`
- **C�digo:**
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

### CSS para Links com �cones
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
- Links exibidos horizontalmente na barra de navega��o
- �cones alinhados � esquerda do texto
- Efeitos hover suaves

### Mobile
- Menu hamb�rguer ativado
- Links em coluna vertical
- �cones com mais espa�amento
- Painel Admin ocupa largura total

---

## ??? Mapa de Navega��o do Sistema

```
Home (/)
?
??? ?? Contato (/Messages/Send)
?   ??? POST: Enviar mensagem
?
??? ?? �rea do Usu�rio (autenticado)
?   ??? ?? Minhas Mensagens (/Messages/MyMessages)
?   ?   ??? ?? Detalhes da Mensagem (/Messages/Detail/{id})
?   ?
?   ??? ?? Enviar Nova Mensagem (/Messages/Send)
?
??? ?? �rea Administrativa (admin role)
    ??? ?? Dashboard (/Admin/AdminMessages/Dashboard)
    ?   ??? Estat�sticas gerais
    ?   ??? A��es r�pidas
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

## ?? Fluxo de Navega��o do Usu�rio

### Usu�rio An�nimo
1. Acessa o site
2. Clica em **?? Contato** na navega��o
3. Preenche e envia mensagem
4. Recebe confirma��o

### Usu�rio Autenticado
1. Faz login
2. Visualiza **?? Minhas Mensagens** na navega��o
3. Pode acessar hist�rico de mensagens
4. Pode enviar novas mensagens via **?? Contato**
5. Recebe notifica��o visual de respostas

### Administrador
1. Faz login com conta admin
2. Visualiza **?? Painel Admin** (destaque dourado)
3. Acessa dashboard com estat�sticas
4. Gerencia mensagens e leads
5. Pode exportar dados

---

## ?? Benef�cios da Navega��o Integrada

### ? Para Usu�rios
- Acesso r�pido ao formul�rio de contato
- F�cil visualiza��o do hist�rico de mensagens
- Interface intuitiva com �cones

### ? Para Administradores
- Acesso direto ao painel administrativo
- Visual destacado para identifica��o r�pida
- Navega��o eficiente entre m�dulos

### ? Para o Sistema
- UX consistente em todo o site
- Navega��o responsiva
- F�cil manuten��o e expans�o

---

## ??? Customiza��o

### Adicionar Novo Link
1. Edite `Views/Shared/_Layout.cshtml`
2. Adicione o link dentro de `.nav-menu`
3. Aplique condi��es se necess�rio (`@if`)
4. Adicione �cone do Bootstrap Icons
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

## ? Checklist de Implementa��o

- [x] Link de Contato para todos os usu�rios
- [x] Link de Minhas Mensagens para autenticados
- [x] Link do Painel Admin para administradores
- [x] �cones Bootstrap Icons
- [x] Estilos responsivos
- [x] Destaque visual para admin
- [x] Link no footer para contato
- [x] Documenta��o completa

---

**Desenvolvido para Nossa TV - Sistema de Mensagens**
