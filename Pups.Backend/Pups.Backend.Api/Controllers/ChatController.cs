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

    // POST /chats/solo
    /// <summary>
    /// Создать соло беседу (a-la saved messages).
    /// </summary>
    /// <remarks>
    /// Вызывается единожды для каждого пользователя при его создании.
    /// </remarks>
    /// <param name="soloChatDto">ID пользователя и сопутсвующие данные</param>
    /// <returns>Объект созданного чата (включая данные об участниках)</returns>
    /// <response code="201">Чат создан</response>
    /// <response code="400">Переданы некорректные данные</response>
    /// <response code="404">Пользователь с переданным ID не существует</response>
    [HttpPost("/Solo")]
    [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(ChatDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ChatDto>> CreateSoloChannel([FromBody, BindRequired] CreateSoloChatDto soloChatDto)
    {
        if (!await _userService.DoesUserExist(soloChatDto.CreatorId))
            return NotFound();

        if (!await _chatService.DoesChatExistWithMembers(new List<Guid> { soloChatDto.CreatorId }))
            return BadRequest();

        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            TypeId = (int)ChatTypes.Solo
        };

        var chatMembers = new List<ChatMember>
        {
            new ChatMember
            {
                ChatId = chat.Id,
                UserId = soloChatDto.CreatorId,
                ChatName = "In my mind",
                ChatStatusId = (int)ChatStatuses.Common                
            }
        };

        await _chatService.CreateChatAndMembers(chat, chatMembers);

        return CreatedAtAction(nameof(GetChat),
            new { id = Guid.NewGuid(), includeMembers = true },
            chat.AsDto());
    }

    // POST /chats/chat
    /// <summary>
    /// Создать чат между 2-мя пользователями
    /// </summary>
    /// <remarks>
    /// В приложении может быть только 1 не групповой чат между 2-мя пользователями
    /// </remarks>
    /// <param name="chatDto">Данные для создания чата</param>
    /// <returns>Объект созданного чата (включая данные об участниках)</returns>
    /// <response code="201">Чат создан</response>
    /// <response code="400">Переданы некорректные данные</response>
    /// <response code="404">Пользователь(-и) с переданным(-и) ID не существует(-ют)</response>
    [HttpPost("/Сhat")]
    [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(ChatDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ChatDto>> CreateChat([FromBody, BindRequired] CreateChatDto chatDto)
    {
        if (!await _userService.DoesUserExist(chatDto.CreatorId)
            || !await _userService.DoesUserExist(chatDto.ContactId))
            return NotFound();

        var userIds = new List<Guid> { chatDto.CreatorId, chatDto.ContactId };

        if (!await _chatService.DoesChatExistWithMembers(userIds))
            return BadRequest();

        if (chatDto.CreatorChatStatusId != null && 
            (chatDto.CreatorChatStatusId < 1 || chatDto.CreatorChatStatusId > 4))
            return BadRequest();

        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            TypeId = (int)ChatTypes.Chat
        };

        var userNames = userIds
            .Select(async userId => (await _userService.GetUser(userId))!.Username!)
            .Select(x => x.Result)
            .ToList();

        var chatMembers = new List<ChatMember>
        {
            new ChatMember
            {
                ChatId = chat.Id,
                UserId = chatDto.CreatorId,
                ChatName = chatDto.CreatorChatName ?? userNames[1],
                ChatStatusId = chatDto.CreatorChatStatusId ?? (int)ChatStatuses.Common
            },
            new ChatMember
            {
                ChatId = chat.Id,
                UserId = chatDto.ContactId,
                ChatName = userNames[0],
                ChatStatusId = (int)ChatStatuses.Common
            }
        };

        await _chatService.CreateChatAndMembers(chat, chatMembers);

        return CreatedAtAction(nameof(GetChat),
            new { id = Guid.NewGuid(), includeMembers = true },
            chat.AsDto());
    }

    // POST /chats/group
    /// <summary>
    /// Создать новую беседу
    /// </summary>
    /// <param name="groupDto">Данные необходимые для создания новой беседы</param>
    /// <returns>Объект созданной беседы (включая данные об участниках)</returns>
    /// <response code="201">Беседа создана</response>
    /// <response code="400">Переданы некорректные данные</response>
    /// <response code="404">Один или больше пользователей не найдено</response>
    [HttpPost("/Group")]
    [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(ChatDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ChatDto>> CreateChat([FromBody, BindRequired] CreateGroupDto groupDto)
    {
        if (groupDto.MembersIds.Count == 0)
            return BadRequest();

        if (groupDto.CreatorChatStatusId != null &&
            (groupDto.CreatorChatStatusId < 1 || groupDto.CreatorChatStatusId > 4))
            return BadRequest();

        if (groupDto.MembersIds.Contains(groupDto.CreatorId))
            groupDto.MembersIds.Remove(groupDto.CreatorId);

        var cts = new CancellationTokenSource();
        bool userDontExist = await _userService.DoesUserExist(groupDto.CreatorId);
        await Parallel.ForEachAsync(groupDto.MembersIds, cts.Token,
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

        var chat = new Chat()
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            TypeId = (int)ChatTypes.Group
        };

        var chatMembers = groupDto.MembersIds
            .Select(userId => new ChatMember
                {
                    ChatId = chat.Id,
                    UserId = userId,
                    ChatName = groupDto.GroupName,
                    ChatStatusId = (int)ChatStatuses.Common
                })
            .Append(new ChatMember
                {
                    ChatId = chat.Id,
                    UserId = groupDto.CreatorId,
                    ChatName = groupDto.GroupName,
                    ChatStatusId = groupDto.CreatorChatStatusId ?? (int)ChatStatuses.Common
                });

        await _chatService.CreateChatAndMembers(chat, chatMembers);

        return CreatedAtAction(nameof(GetChat),
            new { id = Guid.NewGuid(), includeMembers = true },
            chat.AsDto());
    }
}
