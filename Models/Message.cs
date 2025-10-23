using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nossa_TV.Models
{
    /// <summary>
    /// Representa uma mensagem enviada por um usuário
    /// </summary>
    [Table("messages")]
    public class Message
    {
        [Key]
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }

        [JsonPropertyName("sender_name")]
        public string SenderName { get; set; } = string.Empty;

        [JsonPropertyName("sender_email")]
        public string SenderEmail { get; set; } = string.Empty;

        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        [JsonPropertyName("message_content")]
        public string MessageContent { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = "New";

        [JsonPropertyName("is_read")]
        public bool IsRead { get; set; }

        [JsonPropertyName("read_at")]
        public DateTime? ReadAt { get; set; }

        [JsonPropertyName("replied_at")]
        public DateTime? RepliedAt { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
