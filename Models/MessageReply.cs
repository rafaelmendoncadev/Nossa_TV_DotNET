using System.Text.Json.Serialization;

namespace Nossa_TV.Models
{
    /// <summary>
    /// Representa uma resposta do administrador a uma mensagem
    /// </summary>
    public class MessageReply
    {
        [JsonPropertyName("objectId")]
        public string? ObjectId { get; set; }

        [JsonPropertyName("messageId")]
        public string MessageId { get; set; } = string.Empty;

        [JsonPropertyName("adminUserId")]
        public string AdminUserId { get; set; } = string.Empty;

        [JsonPropertyName("replyContent")]
        public string ReplyContent { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}
