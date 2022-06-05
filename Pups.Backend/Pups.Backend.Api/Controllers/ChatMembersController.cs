using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pups.Backend.Api.Dtos.ChatMember;
using Pups.Backend.Api.Models;
using Pups.Backend.Api.Models.LocalEnums;
using Pups.Backend.Api.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Pups.Backend.Api.Controllers;

/// <summary>
/// Контроллер для работы с записями участников чатов
/// </summary>
[ApiController]
[Route("[controller]")]
public class ChatMembersController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly IChatMemberService _chatMemberService;

    public ChatMembersController(IChatMemberService chatMemberService, IChatService chatService)
    {
        _chatMemberService = chatMemberService;
        _chatService = chatService;
    }

    // GET /chatmembers
    /// <summary>
    /// Получить список всех участников всех чатов
    /// </summary>
    /// <returns>Коллекцию участников чатов</returns>
    [HttpGet]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ChatMemberDto>))]
    public async Task<ActionResult<IEnumerable<ChatMemberDto>>> GetAllChatMembers()
    {
        var members = (await _chatMemberService.GetAllChatMembers())
            .Select(x => x.AsDto());

        return Ok(members);
    }

    // GET /chatmembers/chatId
    /// <summary>
    /// Получить список участников чата по его ID
    /// </summary>
    /// <param name="chatId" example="abcd1234-ab12-ab12-ab12-abcdef123456">ID чата</param>
    /// <returns>Коллекцию участников чата</returns>
    /// <response code="200"></response>
    /// <response code="404">Чат с переданным ID не существует</response>
    [HttpGet("{chatId}")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ChatMemberDto>))]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<IEnumerable<ChatMemberDto>>> GetChatMembers(Guid chatId)
    {
        var existingChat = await _chatService.GetChat(chatId, true);

        if (existingChat is null)
            return NotFound();

        var members = existingChat.Members!
            .Select(x => x.AsDto());

        return Ok(members);
    }

    // GET /chatmembers/chatId/userId
    /// <summary>
    /// Получить конкретную запись участника чата по ID чата и ID пользователя
    /// </summary>
    /// <param name="chatId" example="abcd1234-ab12-ab12-ab12-abcdef123456">ID чата</param>
    /// <param name="userId" example="abcd1234-ab12-ab12-ab12-abcdef123456">ID пользователя</param>
    /// <returns>Запись участника чата</returns>
    /// <response code="200"></response>
    /// <response code="404">Запись об учатнике не существует</response>
    [HttpGet("{chatId}/{userId}")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ChatMemberDto))]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ChatMemberDto>> GetChatMember(Guid chatId, Guid userId)
    {
        var member = await _chatMemberService.GetChatMember(chatId, userId);

        if (member is null)
            return NotFound();

        return Ok(member.AsDto());
    }

    // POST /chatmembers
    /// <summary>
    /// Создать запись о новом участнике в чате
    /// </summary>
    /// <param name="chatMemberDto">Данные необходимые для создания записи о новом участнике</param>
    /// <returns></returns>
    /// <response code="201">Участник добавлен</response>
    /// <response code="400">Переданы некорректные данные</response>
    [HttpPost]
    [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(ChatMemberDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ChatMemberDto>> CreateChatMember([FromBody, BindRequired] CreateChatMemberDto chatMemberDto)
    {
        var existingChat = await _chatService.GetChat(chatMemberDto.ChatId, includeMembers: true);

        if (existingChat is null)
            return BadRequest();

        var type = (ChatTypes)existingChat.TypeId;

        if (type == ChatTypes.Chat || type == ChatTypes.Solo)
            return BadRequest();

        if (existingChat.Members!.Any(x => x.UserId == chatMemberDto.UserId))
            return BadRequest();

        ChatMember member = new()
        {
            ChatId = chatMemberDto.ChatId,
            UserId = chatMemberDto.UserId,
            ChatName = chatMemberDto.ChatName,
            ChatStatusId = chatMemberDto.ChatStatusId ?? (int)ChatStatuses.Common
        };

        await _chatMemberService.CreateChatMember(member);
        return CreatedAtAction(nameof(GetChatMember), new { chatId = member.ChatId, userId = member.UserId }, member.AsDto());
    }

    // PUT /chatmembers/chatId/userId
    /// <summary>
    /// Обновить конкретную запись участника чата по ID чата и ID пользователя
    /// </summary>
    /// <param name="chatId" example="abcd1234-ab12-ab12-ab12-abcdef123456">ID чата</param>
    /// <param name="userId" example="abcd1234-ab12-ab12-ab12-abcdef123456">ID пользователя</param>
    /// <param name="chatMemberDto">Данные необходимые для обновления записи об участнике чата</param>
    /// <response code="204"></response>    
    /// <response code="404">Записи участника с переданной сигнатурой не существует</response>
    [HttpPut("{chatId}/{userId}")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> UpdateMessage(Guid chatId, Guid userId, [FromBody, BindRequired] UpdateChatMemberDto chatMemberDto)
    {
        var existingChat = await _chatService.GetChat(chatId, includeMembers: true);

        if (existingChat is null)
            return NotFound();

        if (!existingChat.Members!.Any(x => x.UserId == userId))
            return NotFound();

        ChatMember member = new()
        {
            ChatId = chatId,
            UserId = userId,
            ChatName = chatMemberDto.ChatName,
            ChatStatusId = chatMemberDto.ChatStatusId ?? existingChat.TypeId
        };

        await _chatMemberService.UpdateChatMember(member);
        return NoContent();
    }
}
