using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Nossa_TV.Services;
using Nossa_TV.ViewModels;

namespace Nossa_TV.Controllers
{
    /// <summary>
    /// Controller para área pública de mensagens
    /// </summary>
    public class MessagesController : Controller
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Send()
        {
            return View(new SendMessageViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Send(SendMessageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.Identity!.Name;
            var result = await _messageService.SendMessageAsync(model, userId);

            if (result)
            {
                TempData["SuccessMessage"] = "Mensagem enviada com sucesso! Responderemos em breve.";
                return RedirectToAction("Send");
            }

            ModelState.AddModelError("", "Erro ao enviar mensagem. Tente novamente.");
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyMessages()
        {
            var userId = User.Identity!.Name!;
            var viewModel = await _messageService.GetUserMessagesAsync(userId);
            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Detail(string id)
        {
            var message = await _messageService.GetMessageDetailAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            // Verificar se a mensagem pertence ao usuário
            var userId = User.Identity!.Name;
            if (message.UserId != userId)
            {
                return Forbid();
            }

            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Reply(string id, [FromForm] UserReplyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor, preencha todos os campos corretamente.";
                return RedirectToAction("Detail", new { id });
            }

            // Obter a mensagem original
            var originalMessage = await _messageService.GetMessageDetailAsync(id);
            if (originalMessage == null)
            {
                return NotFound();
            }

            // Verificar se a mensagem pertence ao usuário
            var userId = User.Identity!.Name;
            if (originalMessage.UserId != userId)
            {
                return Forbid();
            }

            // Verificar se a mensagem foi respondida
            if (originalMessage.Replies.Count == 0)
            {
                TempData["ErrorMessage"] = "Esta mensagem ainda não foi respondida pelo administrador.";
                return RedirectToAction("Detail", new { id });
            }

            // Criar uma nova mensagem com a pergunta do usuário
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
                TempData["SuccessMessage"] = "Pergunta enviada com sucesso! Responderemos em breve.";
                return RedirectToAction("MyMessages");
            }

            TempData["ErrorMessage"] = "Erro ao enviar sua pergunta. Tente novamente.";
            return RedirectToAction("Detail", new { id });
        }
    }
}
