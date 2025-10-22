using System.ComponentModel.DataAnnotations;

namespace Nossa_TV.ViewModels
{
    /// <summary>
    /// ViewModel para envio de mensagens
    /// </summary>
    public class SendMessageViewModel
    {
        [Required(ErrorMessage = "O nome � obrigat�rio")]
        [StringLength(100, ErrorMessage = "O nome deve ter no m�ximo 100 caracteres")]
        public string SenderName { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email � obrigat�rio")]
        [EmailAddress(ErrorMessage = "Email inv�lido")]
        [StringLength(100, ErrorMessage = "O email deve ter no m�ximo 100 caracteres")]
        public string SenderEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "O assunto � obrigat�rio")]
        [StringLength(200, ErrorMessage = "O assunto deve ter no m�ximo 200 caracteres")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "A mensagem � obrigat�ria")]
        [StringLength(2000, ErrorMessage = "A mensagem deve ter no m�ximo 2000 caracteres")]
        public string MessageContent { get; set; } = string.Empty;

        public string? Phone { get; set; }
    }
}
