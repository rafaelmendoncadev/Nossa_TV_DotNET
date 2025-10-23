using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nossa_TV.Models
{
    /// <summary>
    /// Representa uma resposta do administrador a uma mensagem
    /// </summary>
    [Table("message_replies")]
    public class MessageReply
    {
        [Key]
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("message_id")]
        public string MessageId { get; set; } = string.Empty;

        [JsonPropertyName("admin_user_id")]
        public string AdminUserId { get; set; } = string.Empty;

        [JsonPropertyName("reply_content")]
        public string ReplyContent { get; set; } = string.Empty;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
