using System.Text.Json.Serialization;

namespace Nossa_TV.Models
{
    /// <summary>
    /// Representa um lead capturado através do sistema de mensagens
    /// </summary>
    public class Lead
    {
        [JsonPropertyName("objectId")]
        public string? ObjectId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("firstContactDate")]
        public DateTime FirstContactDate { get; set; }

        [JsonPropertyName("lastContactDate")]
        public DateTime LastContactDate { get; set; }

        [JsonPropertyName("messageCount")]
        public int MessageCount { get; set; }

        [JsonPropertyName("tags")]
        public List<string>? Tags { get; set; }

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}
