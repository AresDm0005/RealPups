using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pups.Backend.Api.Dtos.Chat;
using Pups.Backend.Api.Models;
using Pups.Backend.Api.Models.LocalEnums;
using Pups.Backend.Api.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Pups.Backend.Api.Controllers;

/// <summary>
/// Контроллер для работы с чатами
/// </summary>
[ApiController]
[Route("[controller]")]
public class ChatsController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IChatService _chatService;

    public ChatsController(IChatService chatService, IUserService userService)
    {
        _chatService = chatService;
        _userService = userService;
    }

    // GET /chats
    /// <summary>
    /// Получить список всех чатов без указания участников
    /// </summary>
    /// <returns>Коллекцию чатов</returns>
    [HttpGet]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ChatDto>))]
    public async Task<ActionResult<IEnumerable<ChatDto>>> GetChats()
    {
        var chats = (await _chatService.GetChats())
            .Select(x => x.AsDto());

        return Ok(chats);
    }

    // GET /users/id
    /// <summary>
    /// Получить конкретный чат по его ID с/без указания участников
    /// </summary>
    /// <param name="id" example="abcd1234-ab12-ab12-ab12-abcdef123456">ID чата</param>
    /// <param name="includeMembers" example="true">
    /// Включать список участников (true) или нет (false)
    /// Базово - false
    /// </param>
    /// <returns>Чат</returns>
    /// <response code="200"></response>
    /// <response code="404">Чата с переданным ID не существует</response>
    [HttpGet("{id}")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ChatDto))]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ChatDto>> GetChat(Guid id, [FromQuery] bool includeMembers = false)
    {
        var chat = await _chatService.GetChat(id, includeMembers);
        if (chat is null)
            return NotFound();

        return Ok(chat.AsDto());
    }

    // GET /chats/userchats/userId
    /// <summary>
    /// Получить все чаты пользователя по его ID
    /// </summary>
    /// <param name="userId" example="abcd1234-ab12-ab12-ab12-abcdef123456">ID пользователя</param>
    /// <returns>Чат</returns>
    /// <response code="200"></response>
    /// <response code="404">Пользователя с переданным ID не существует</response>
    [HttpGet("UserChats/{userId}")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ChatDto>))]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<IEnumerable<ChatDto>>> GetChats(Guid userId)
    {
        var userExist = await _userService.DoesUserExist(userId);

        if (!userExist)
            return NotFound();

        var chats = (await _chatService.GetChats(userId))
            .Select(x => x.AsDto());
        return Ok(chats);
    }

    // POST /chats
    /// <summary>
    /// Создать новый чат
    /// </summary>
    /// <param name="chatDto">Данные необходимые для создания нового чата</param>
    /// <returns></returns>
    /// <response code="201">Чат создан</response>
    /// <response code="400">Переданы некорректные данные</response>
    /// <response code="404">Один или больше пользователей не найдено</response>
    [HttpPost]
    [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(ChatDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ChatDto>> CreateChat([FromBody, BindRequired] CreateChatDto chatDto)
    {
        if (chatDto.MembersIds.Count() == 0)
            return BadRequest();

        var cts = new CancellationTokenSource();
        bool userDontExist = false;
        await Parallel.ForEachAsync(chatDto.MembersIds, cts.Token, 
                async (userId, token) =>
                {
                    if (!await _userService.DoesUserExist(userId))
                    {
                        userDontExist = true;
                        cts.Cancel();
                    }
                });

        if (userDontExist)
            return NotFound();

        var type = chatDto.MembersIds.Count() switch
        {
            1 => ChatTypes.Solo,
            2 => ChatTypes.Chat,
            _ => ChatTypes.Group
        };

        if (type != ChatTypes.Group 
            && await _chatService.DoesChatExistWithMembers(chatDto.MembersIds))
        {
            return BadRequest();
        }

        var chat = new Chat()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            TypeId = ((int)type)
        };

        var chatMembers = chatDto.MembersIds
            .Select(userId => new ChatMember
            {
                ChatId = chat.Id,
                UserId = userId,
                ChatName = null,
                ChatStatusId = (int)ChatStatuses.Common
            });

        await _chatService.CreateChatAndMembers(chat, chatMembers);

        return CreatedAtAction(nameof(GetChat), 
            new { id = Guid.NewGuid(), includeMembers = true },
            chat.AsDto());
    }
}
