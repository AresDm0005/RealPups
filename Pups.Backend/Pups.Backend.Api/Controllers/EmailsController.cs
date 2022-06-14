using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pups.Backend.Api.Dtos.Email;
using Pups.Backend.Api.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Pups.Backend.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailsController : ControllerBase
{
    private readonly IMailService _mailService;
    private readonly IUserService _userService;

    public EmailsController(IMailService mailService, IUserService userService)
    {
        _mailService = mailService;
        _userService = userService;
    }

    // POST /send/confirmation
    /// <summary>
    /// Отправить письмо для подтверждения почты (при регистрации)
    /// </summary>
    /// <param name="emailDto">Данные для отправки письма</param>
    /// <response code="204">Письмо успешно отправлено</response>
    /// <response code="400">При отправке письма произошла обшибка</response>
    /// <response code="404">Пользователь с переданным ID не найден</response>
    [HttpPost("Send/Confirmation")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> SendEmailConfirmationLetter([FromBody, BindRequired] ConfirmationLetterDto emailDto)
    {
        var user = await _userService.GetUser(emailDto.UserId);

        if (user is null)
            return NotFound();

        var result = await _mailService.SendEmailConfirmationLetter(user, emailDto.Code);

        if (!result)
            return BadRequest();

        return NoContent();
    }

    // POST /send/changeconfirmation
    /// <summary>
    /// Отправить письмо для подтверждения смены почты
    /// </summary>
    /// <param name="emailDto">Данные для отправки письма</param>
    /// <response code="204">Письмо успешно отправлено</response>
    /// <response code="400">При отправке письма произошла обшибка</response>
    /// <response code="404">Пользователь с переданным ID не найден</response>
    [HttpPost("Send/ChangeConfirmation")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> SendChangeEmailConfirmationLetter([FromBody, BindRequired] ChangeConfirmationLetterDto emailDto)
    {
        var user = await _userService.GetUser(emailDto.UserId);

        if (user is null)
            return NotFound();

        var result = await _mailService.SendEmailChangeConfirmationLetter(user, emailDto.NewEmail, emailDto.Code);

        if (!result)
            return BadRequest();

        return NoContent();
    }
}
