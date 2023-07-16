using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Otus.Microservice.User.Models;
using Otus.Microservice.User.Services;

namespace Otus.Microservice.User.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserManager<Models.User> _userManager;

    public UserController(
        ILogger<UserController> logger,
        UserManager<Models.User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(Models.User), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] long userId)
    {
        if (!HttpContext.HasUserId(out var currentUserId))
        {
            _logger.LogInformation("The current user identifier is not set");
            return BadRequest("The current user identifier is not set");
        }

        if (!string.Equals(userId.ToString(), currentUserId, StringComparison.InvariantCultureIgnoreCase))
        {
            _logger.LogInformation(
                "Can't read another user data: Current user id {CurrentUserId} and request user id {UserId}",
                currentUserId,
                userId);
            return BadRequest("Can't read another user data");
        }

        var user = await _userManager.FindByIdAsync(currentUserId);
        if (user == null)
        {
            _logger.LogInformation("User with id {UserId} is not found", currentUserId);
            return NotFound(new Error("404", $"User is not found"));
        }

        return Ok(new UserModel
        {
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone
        });
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateUser([FromBody] UserModel createUser)
    {
        try
        {
            var result = await _userManager.CreateAsync(
                new Models.User
                {
                    UserName = createUser.Username,
                    Email = createUser.Email,
                    Phone = createUser.Phone,
                    FirstName = createUser.FirstName,
                    LastName = createUser.LastName
                });

            if (result.Succeeded) return Ok();

            var error = string.Join(";", result.Errors.Select(x => $"{x.Code}:{x.Description}"));
            _logger.LogError("Create user error: {Error}",error);
            return BadRequest($"Create user error: {error}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Create user error");
            throw;
        }
    }

    [HttpPut("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser([FromBody] UserModel user)
    {
        if (!HttpContext.HasUserId(out var currentUserId))
        {
            _logger.LogInformation("The current user identifier is not set");
            return BadRequest("The current user identifier is not set");
        }

        try
        {
            var existedUser = await _userManager.FindByIdAsync(currentUserId);
            if (existedUser == null)
            {
                _logger.LogInformation("User with id {UserId} is not found", currentUserId);
                return NotFound(new Error("404", $"User is not found"));
            }

            existedUser.Username = user.Username;
            existedUser.FirstName = user.FirstName;
            existedUser.LastName = user.LastName;
            existedUser.Email = user.Email;
            existedUser.Phone = user.Phone;
            var result = await _userManager.UpdateAsync(existedUser);
            if (result.Succeeded) return Ok();

            var error = string.Join(";", result.Errors.Select(x => $"{x.Code}:{x.Description}"));
            _logger.LogError("Update user error: {Error}",error);
            return BadRequest($"Update user error: {error}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Update user with id {UserId} error", currentUserId);
            throw;
        }
    }

    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser([FromRoute] long userId)
    {
        try
        {
            var existedUser = await _userManager.FindByIdAsync(userId.ToString());
            if (existedUser == null)
            {
                _logger.LogInformation("User with id {UserId} is not found", userId);
                return NotFound(new Error("404", $"User with id {userId} is not found"));
            }
            var result = await _userManager.DeleteAsync(existedUser);
            if (result.Succeeded) return Ok();

            var error = string.Join(";", result.Errors.Select(x => $"{x.Code}:{x.Description}"));
            _logger.LogError("Delete user error: {Error}",error);
            return BadRequest($"Delete user error: {error}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Delete user with id {UserId} error", userId);
            throw;
        }
    }
}