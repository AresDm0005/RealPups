using Pups.Backend.Api.Models;
using Pups.Backend.Api.Dtos.Chat;
using Pups.Backend.Api.Dtos.ChatMember;
using Pups.Backend.Api.Dtos.Message;
using Pups.Backend.Api.Dtos.User;

namespace Pups.Backend.Api;

public static class Extensions
{
    public static UserDto AsDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Info = user.Info!,
            Created = user.Created,
            LastSeen = user.LastSeen
        };
    }

    public static ChatDto AsDto(this Chat chat)
    {
        return new ChatDto
        {
            Id = chat.Id,
            CreatedAt = chat.CreatedAt,
            TypeId = chat.TypeId,
            Members = chat.Members?.Select(x => x.AsDto()).ToList()
        };
    }

    public static ChatMemberDto AsDto(this ChatMember chatMember)
    {
        return new ChatMemberDto
        {
            ChatId = chatMember.ChatId,
            UserId = chatMember.UserId,
            ChatName = chatMember.ChatName,
            ChatStatusId = chatMember.ChatStatusId            
        };
    }

    public static MessageDto AsDto(this Message msg)
    {
        return new MessageDto
        {
            Id = msg.Id,
            ChatId = msg.ChatId,
            SenderId = msg.SenderId,
            Payload = msg.Payload,
            SendAt = msg.SendAt,
            CheckedAt = msg.CheckedAt,
            Edited = msg.Edited,
            ReplyTo = msg.ReplyTo
        };
    }
}
