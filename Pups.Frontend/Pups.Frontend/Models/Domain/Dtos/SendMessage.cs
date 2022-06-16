using Newtonsoft.Json;

namespace Pups.Frontend.Models.Domain.Dtos;

public class SendMessage
{
    [JsonProperty("chatId")]
    public Guid ChatId { get; set; }
    [JsonProperty("senderId")]
    public Guid SenderId { get; set; }
    [JsonProperty("payload")]
    public string Payload { get; set; }
    [JsonProperty("replyTo")]
    public int ReplyTo { get; set; }
}