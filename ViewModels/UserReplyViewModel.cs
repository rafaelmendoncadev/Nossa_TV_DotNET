using System.ComponentModel.DataAnnotations;

namespace Nossa_TV.ViewModels
{
    /// <summary>
    /// ViewModel para resposta/pergunta adicional do usu�rio ap�s receber resposta do admin
    /// </summary>
    public class UserReplyViewModel
    {
        [Required(ErrorMessage = "A pergunta � obrigat�ria")]
        [StringLength(2000, ErrorMessage = "A pergunta deve ter no m�ximo 2000 caracteres")]
        public string ReplyContent { get; set; } = string.Empty;

        public string OriginalMessageId { get; set; } = string.Empty;
    }
}
