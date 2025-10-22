using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nossa_TV.Services;
using Nossa_TV.ViewModels;

namespace Nossa_TV.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller para gerenciamento administrativo de mensagens
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class AdminMessagesController : Controller
    {
        private readonly IMessageService _messageService;

        public AdminMessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index(string? status, DateTime? date, int page = 1)
        {
            var viewModel = await _messageService.GetMessagesAsync(status, date, page);
            return View(viewModel);
        }

        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var stats = await _messageService.GetDashboardStatsAsync();
            return View(stats);
        }

        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Detail(string id)
        {
            var message = await _messageService.GetMessageDetailAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            // Marcar como lida automaticamente ao visualizar
            if (!message.IsRead)
            {
                await _messageService.MarkAsReadAsync(id);
            }

            return View(message);
        }

        [HttpPost("Reply/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(string id, [FromForm] string replyContent)
        {
            if (string.IsNullOrWhiteSpace(replyContent))
            {
                TempData["ErrorMessage"] = "A resposta não pode estar vazia.";
                return RedirectToAction("Detail", new { id });
            }

            var adminUserId = User.Identity?.Name ?? "admin";
            var result = await _messageService.ReplyToMessageAsync(id, replyContent, adminUserId);

            if (result)
            {
                TempData["SuccessMessage"] = "Resposta enviada com sucesso!";
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao enviar resposta.";
            }

            return RedirectToAction("Detail", new { id });
        }

        [HttpPost("MarkAsRead/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            var result = await _messageService.MarkAsReadAsync(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Mensagem marcada como lida.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost("Archive/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Archive(string id)
        {
            var result = await _messageService.ArchiveMessageAsync(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Mensagem arquivada com sucesso.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _messageService.DeleteMessageAsync(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Mensagem excluída com sucesso.";
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao excluir mensagem.";
            }

            return RedirectToAction("Index");
        }
    }
}
