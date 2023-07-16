namespace Otus.Microservice.User.ViewModel;

public class LoginViewModel
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? ReturnUrl { get; set; }
}