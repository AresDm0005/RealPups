﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pups.Backend.Api.Dtos.User;
using Pups.Backend.Api.Models;
using Pups.Backend.Api.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Pups.Backend.Api.Controllers;

/// <summary>
/// Контроллер для работы с пользователями
/// </summary>
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    /// <inheritdoc/>
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // GET /users
    /// <summary>
    /// Получить список всех пользователей системы
    /// </summary>
    /// <returns>Коллекцию пользователей</returns>
    [HttpGet(Name = "Get Users")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<UserDto>))]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = (await _userService.GetUsers())
            .Select(x => x.AsDto());

        return Ok(users);
    }

    // GET /users/id
    /// <summary>
    /// Получить конкретного пользователя по его ID
    /// </summary>
    /// <param name="id" example="abcd1234-ab12-ab12-ab12-abcdef123456">ID пользователя</param>
    /// <returns>Пользователя</returns>
    /// <response code="200"></response>
    /// <response code="404"></response>
    [HttpGet("{id}", Name = "Get User")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(UserDto))]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<UserDto>> GetUser(Guid id)
    {
        var user = await _userService.GetUser(id);

        if (user is null)
            return NotFound();

        return Ok(user.AsDto());
    }

    // POST /users
    /// <summary>
    /// Создать нового пользователя
    /// </summary>
    /// <param name="userDto">Данные необходимые для создания пользователя</param>
    /// <returns></returns>
    /// <response code="201">Пользователь создан</response>
    [HttpPost]
    [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(UserDto))]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody, BindRequired] CreateUserDto userDto)
    {
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Username = userDto.Username,
            Email = userDto.Email,
            Info = userDto.Info,
            Created = DateTime.UtcNow,
            LastSeen = DateTime.UtcNow
        };

        await _userService.CreateUser(user);

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user.AsDto());
    }

    // PUT /users/id
    /// <summary>
    /// Обновить данные пользователя по его ID
    /// </summary>
    /// <param name="id" example="abcd1234-ab12-ab12-ab12-abcdef123456">ID пользователя</param>
    /// <param name="userDto">Измененные данные</param>
    /// <response code="204">Пользователь обновлен</response>
    /// <response code="404">Пользователя с данным ID не найдено</response>
    [HttpPut("{id}")]
    [SwaggerResponse((int)HttpStatusCode.NoContent)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> UpdateUser(Guid id, [FromBody, BindRequired] UpdateUserDto userDto)
    {
        var existingUser = await _userService.GetUser(id);

        if (existingUser is null)
            return NotFound();

        User user = new()
        {
            Id = existingUser.Id,
            Username = userDto.Username ?? existingUser.Username,
            Email = existingUser.Email,
            Info = userDto.Info ?? existingUser.Info,
            Created = existingUser.Created,
            LastSeen = userDto.LastSeen ?? existingUser.LastSeen
        };

        await _userService.UpdateUser(user);
        return NoContent();
    }
}