namespace Pups.Frontend.Models.Domain.Dtos;

public class UpdateUserDto
{
    public string? Username { get; init; }

    public string? Info { get; init; }

    public DateTime? LastSeen { get; init; }
}