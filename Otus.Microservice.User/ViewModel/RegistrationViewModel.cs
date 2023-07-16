using System.ComponentModel.DataAnnotations;

namespace Otus.Microservice.User.ViewModel;

public class RegistrationViewModel
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}