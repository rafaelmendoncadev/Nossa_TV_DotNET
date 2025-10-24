using System.ComponentModel.DataAnnotations;

namespace Nossa_TV.ViewModels
{
    /// <summary>
    /// ViewModel para resposta/pergunta adicional do usuário após receber resposta do admin
    /// </summary>
    public class UserReplyViewModel
    {
        [Required(ErrorMessage = "A pergunta é obrigatória")]
        [StringLength(2000, ErrorMessage = "A pergunta deve ter no máximo 2000 caracteres")]
        public string ReplyContent { get; set; } = string.Empty;

        public string OriginalMessageId { get; set; } = string.Empty;
    }
}
