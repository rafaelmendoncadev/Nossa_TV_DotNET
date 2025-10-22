using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nossa_TV.Services;
using System.Text;

namespace Nossa_TV.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller para gerenciamento administrativo de leads
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class AdminLeadsController : Controller
    {
        private readonly ILeadService _leadService;

        public AdminLeadsController(ILeadService leadService)
        {
            _leadService = leadService;
        }

        [HttpGet("")]
        [HttpGet("Index")]
        public async Task<IActionResult> Index(string? search, string? tag, int page = 1)
        {
            var viewModel = await _leadService.GetLeadsAsync(search, tag, page);
            return View(viewModel);
        }

        [HttpGet("Detail/{id}")]
        public async Task<IActionResult> Detail(string id)
        {
            var lead = await _leadService.GetLeadDetailAsync(id);

            if (lead == null)
            {
                return NotFound();
            }

            return View(lead);
        }

        [HttpPost("AddTag")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTag(string leadId, string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                TempData["ErrorMessage"] = "A tag não pode estar vazia.";
                return RedirectToAction("Detail", new { id = leadId });
            }

            var result = await _leadService.AddTagAsync(leadId, tag);

            if (result)
            {
                TempData["SuccessMessage"] = "Tag adicionada com sucesso.";
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao adicionar tag.";
            }

            return RedirectToAction("Detail", new { id = leadId });
        }

        [HttpPost("RemoveTag")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveTag(string leadId, string tag)
        {
            var result = await _leadService.RemoveTagAsync(leadId, tag);

            if (result)
            {
                TempData["SuccessMessage"] = "Tag removida com sucesso.";
            }

            return RedirectToAction("Detail", new { id = leadId });
        }

        [HttpPost("UpdateNotes")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateNotes(string leadId, string notes)
        {
            var result = await _leadService.UpdateNotesAsync(leadId, notes);

            if (result)
            {
                TempData["SuccessMessage"] = "Notas atualizadas com sucesso.";
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao atualizar notas.";
            }

            return RedirectToAction("Detail", new { id = leadId });
        }

        [HttpGet("Export")]
        public async Task<IActionResult> Export()
        {
            var csvBytes = await _leadService.ExportLeadsAsync();
            var fileName = $"leads_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            
            return File(csvBytes, "text/csv", fileName);
        }
    }
}
