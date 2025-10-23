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

        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<MessageReply> MessageReplies { get; set; } = null!;
        public DbSet<Lead> Leads { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Defaults and mapping
            builder.Entity<Message>().Property(m => m.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<MessageReply>().Property(r => r.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Entity<Lead>().Property(l => l.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
