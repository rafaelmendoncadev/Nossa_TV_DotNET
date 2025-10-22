using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nossa_TV.Models;

namespace Nossa_TV.Data
{
    /// <summary>
    /// Contexto do banco de dados da aplicação
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Customizações adicionais do modelo podem ser feitas aqui
        }
    }
}
