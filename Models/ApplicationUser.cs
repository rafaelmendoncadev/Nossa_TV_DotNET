using Microsoft.AspNetCore.Identity;

namespace Nossa_TV.Models
{
    /// <summary>
    /// Modelo customizado de usu�rio que estende IdentityUser
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
    }
}
