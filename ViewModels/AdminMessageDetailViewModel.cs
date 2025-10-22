using Nossa_TV.Models;
using System.ComponentModel.DataAnnotations;

namespace Nossa_TV.ViewModels
{
    /// <summary>
    /// ViewModel para detalhes e resposta de mensagens no painel admin
    /// </summary>
    public class AdminMessageDetailViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string MessageContent { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime? RepliedAt { get; set; }
        
        public List<MessageReplyViewModel> Replies { get; set; } = new();

        [Required(ErrorMessage = "A resposta é obrigatória")]
        [StringLength(2000, ErrorMessage = "A resposta deve ter no máximo 2000 caracteres")]
        public string? ReplyContent { get; set; }
    }

    public class MessageReplyViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string AdminUserId { get; set; } = string.Empty;
        public string ReplyContent { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
