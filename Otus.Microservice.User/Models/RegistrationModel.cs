using System.ComponentModel.DataAnnotations;

namespace Otus.Microservice.User.Models;

public class RegistrationModel
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}