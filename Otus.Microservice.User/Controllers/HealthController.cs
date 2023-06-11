using Microsoft.AspNetCore.Mvc;
using Otus.Microservice.User.Models;

namespace Otus.Microservice.User.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public HealthModel GetHealth()
    {
        return new HealthModel
        {
            Status = "OK"
        };
    }
}