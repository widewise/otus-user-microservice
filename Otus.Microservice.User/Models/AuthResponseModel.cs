namespace Otus.Microservice.User.Models;

public class AuthResponseModel
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
}