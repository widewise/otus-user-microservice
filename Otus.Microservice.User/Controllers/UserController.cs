using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Otus.Microservice.User.Models;

namespace Otus.Microservice.User.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly AppDbContext _dbContext;

    public UserController(
        ILogger<UserController> logger,
        AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(Models.User), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] long userId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            _logger.LogInformation("User with id {UserId} is not found", userId);
            return NotFound(new Error("404", $"User with id {userId} is not found"));
        }

        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateUser([FromBody] Models.User createUser)
    {
        try
        {
            await _dbContext.AddAsync(createUser);
            await _dbContext.SaveChangesAsync();
            return Ok();
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
    public async Task<IActionResult> UpdateUser([FromRoute] long userId, [FromBody] Models.User user)
    {
        try
        {
            var existedUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (existedUser == null)
            {
                _logger.LogInformation("User with id {UserId} is not found", userId);
                return NotFound(new Error("404", $"User with id {userId} is not found"));
            }

            existedUser.Username = user.Username;
            existedUser.FirstName = user.FirstName;
            existedUser.LastName = user.LastName;
            existedUser.Email = user.Email;
            existedUser.Phone = user.Phone;
            _dbContext.Users.Entry(existedUser).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Update user with id {UserId} error", userId);
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
            var existedUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (existedUser == null)
            {
                _logger.LogInformation("User with id {UserId} is not found", userId);
                return NotFound(new Error("404", $"User with id {userId} is not found"));
            }

            _dbContext.Users.Remove(existedUser);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Delete user with id {UserId} error", userId);
            throw;
        }
    }
}