using System.ComponentModel.DataAnnotations;

namespace Nossa_TV.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O email � obrigat�rio")]
        [EmailAddress(ErrorMessage = "Email inv�lido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha � obrigat�ria")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Lembrar-me")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
