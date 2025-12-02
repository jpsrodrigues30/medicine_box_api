using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace medicine_box_api.Domain.Dtos;
public record MqttMessage
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    [JsonPropertyName("source")]
    public string Source { get; set; } = string.Empty;
    [JsonPropertyName("target")]
    public string Target { get; set; } = string.Empty;
    [JsonPropertyName("sent_at")]
    public DateTime? SentAt { get; set; } = DateTime.UtcNow;
    [JsonPropertyName("metadata")]
    public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
}
