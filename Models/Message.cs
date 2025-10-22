using System.Text.Json.Serialization;

namespace Nossa_TV.Models
{
    /// <summary>
    /// Representa uma mensagem enviada por um usuário
    /// </summary>
    public class Message
    {
        [JsonPropertyName("objectId")]
        public string? ObjectId { get; set; }

        [JsonPropertyName("userId")]
        public string? UserId { get; set; }

        [JsonPropertyName("senderName")]
        public string SenderName { get; set; } = string.Empty;

        [JsonPropertyName("senderEmail")]
        public string SenderEmail { get; set; } = string.Empty;

        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        [JsonPropertyName("messageContent")]
        public string MessageContent { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = MessageStatus.New.ToString();

        [JsonPropertyName("isRead")]
        public bool IsRead { get; set; }

        [JsonPropertyName("readAt")]
        public DateTime? ReadAt { get; set; }

        [JsonPropertyName("repliedAt")]
        public DateTime? RepliedAt { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}
