using Newtonsoft.Json;

namespace Pups.Frontend.Models.Domain.Dtos;

public class ChatMemberDto
{
    [JsonProperty("chatId")]
    public Guid ChatId { get; init; }

    [JsonProperty("userId")]
    public Guid UserId { get; init; }
    
    [JsonProperty("chatName")]
    public string? ChatName { get; init; }

    [JsonProperty("chatStatusId")]
    public int ChatStatusId { get; init; }
}