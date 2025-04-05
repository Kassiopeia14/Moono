using System.Text.Json.Serialization;

namespace modMnemosyneJSONModels;

public class MessageItem
{
    [JsonPropertyName("text")]
    public required string Text { get; set; }
}
