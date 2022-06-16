using Newtonsoft.Json;

namespace Pups.Frontend.Models.Domain.Dtos;

public record chatDto
{
    [JsonProperty("id")]
    public Guid Id { get; init; }

    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; init; }

    [JsonProperty("typeId")]
    public int TypeId { get; init; }

    [JsonProperty("members")]
    public ICollection<ChatMember>? Members { get; init; }
}