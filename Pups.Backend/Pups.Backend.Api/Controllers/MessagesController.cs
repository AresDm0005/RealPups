using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pups.Backend.Api.Dtos.Message;
using Pups.Backend.Api.Models;
using Pups.Backend.Api.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Pups.Backend.Api.Controllers;

/// <summary>
/// Контроллер для работы с сообщениями
/// </summary>
[ApiController]
[Route("[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService, IChatService chatService)
    {
        _messageService = messageService;
        _chatService = chatService;
    }

    // GET /messages
    /// <summary>
    /// Получить список всех сообщений
    /// </summary>
    /// <returns>Коллекцию сообщений</returns>
    [HttpGet]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<MessageDto>))]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages()
    {
        var msgs = (await _messageService.GetMessages())
            .Select(x => x.AsDto());

        return Ok(msgs);
    }

    // GET /messages/id
    /// <summary>
    /// Получить конкретное сообщение по его ID
    /// </summary>
    /// <param name="id" example="1">ID сообщения</param>
    /// <returns>Пользователя</returns>
    /// <response code="200"></response>
    /// <response code="404">Сообщение с переданным ID не существует</response>
    [HttpGet("{id}")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(MessageDto))]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<MessageDto>> GetMessage(int id)
    {
        var msg = await _messageService.GetMessage(id);

        if (msg is null)
            return NotFound();

        return Ok(msg.AsDto());
    }

    // POST /messages
    /// <summary>
    /// Создать новое сообщение
    /// </summary>
    /// <param name="messageDto">Данные необходимые для создания сообщения</param>
    /// <returns></returns>
    /// <response code="201">Сообщение создано</response>
    /// <response code="400">Переданы некорректные данные</response>
    [HttpPost]
    [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(MessageDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<MessageDto>> CreateMessage([FromBody, BindRequired] CreateMessageDto messageDto)
    {
        var existingChat = await _chatService.GetChat(messageDto.ChatId);

        if (existingChat is null)
            return BadRequest();

        var isMember = await _chatService.IsUserAChatMember(messageDto.SenderId, messageDto.ChatId);

        if (!isMember)
            return BadRequest();

        if (messageDto.ReplyTo is not null)
        {
            var existingMessage = await _messageService.GetMessage(messageDto.ReplyTo.Value);
            if (existingMessage is null)
                return BadRequest();
        }

        Message msg = new()
        {
            ChatId = messageDto.ChatId,
            SenderId = messageDto.SenderId,
            Payload = messageDto.Payload,
            SendAt = DateTime.UtcNow,
            CheckedAt = null,
            Edited = false,
            ReplyTo = messageDto.ReplyTo
        };

        await _messageService.CreateMessage(msg);
        return CreatedAtAction(nameof(GetMessage), new { id = msg.Id }, msg.AsDto());
    }

    // PUT /chatmembers/id
    /// <summary>
    /// Обновить текст сообщения
    /// </summary>
    /// <param name="id" example="1">ID сообщения</param>
    /// <param name="messageDto">Данные необходимые для редактирования сообщения</param>
    /// <response code="204"></response>    
    /// <response code="404">Сообщения с переданным ID не существует</response>
    [HttpPut("{id}")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> UpdateMessage(int id, [FromBody, BindRequired] UpdateMessageDto messageDto)
    {
        var existingMessage = await _messageService.GetMessage(id);

        if (existingMessage is null)
            return NotFound();

        Message msg = new()
        {
            Id = existingMessage.Id,
            ChatId = existingMessage.ChatId,
            SenderId = existingMessage.SenderId,
            Payload = messageDto.Payload,
            SendAt = existingMessage.SendAt,
            CheckedAt = existingMessage.CheckedAt,
            Edited = existingMessage.Edited,
            ReplyTo = existingMessage.ReplyTo
        };

        await _messageService.UpdateMessage(msg);
        return NoContent();
    }

    // PATCH /chatmembers/id
    /// <summary>
    /// Отметить факт и время прочтения сообщения
    /// </summary>
    /// <param name="id" example="1">ID сообщения</param>
    /// <response code="204"></response>    
    /// <response code="404">Сообщения с переданным ID не существует</response>
    [HttpPatch("{id}")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> MessageChecked(int id)
    {
        var existringMessage = _messageService.GetMessage(id);

        if (existringMessage is null)
            return NotFound();

        Message msg = new()
        {
            Id = id,
            CheckedAt = DateTime.UtcNow
        };

        await _messageService.UpdateChecked(msg);
        return NoContent();
    }
}
