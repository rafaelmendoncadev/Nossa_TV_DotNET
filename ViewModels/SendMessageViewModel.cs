using System.ComponentModel.DataAnnotations;

namespace Nossa_TV.ViewModels
{
    /// <summary>
    /// ViewModel para envio de mensagens
    /// </summary>
    public class SendMessageViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string SenderName { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
        public string SenderEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "O assunto é obrigatório")]
        [StringLength(200, ErrorMessage = "O assunto deve ter no máximo 200 caracteres")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "A mensagem é obrigatória")]
        [StringLength(2000, ErrorMessage = "A mensagem deve ter no máximo 2000 caracteres")]
        public string MessageContent { get; set; } = string.Empty;

        public string? Phone { get; set; }
    }
}
