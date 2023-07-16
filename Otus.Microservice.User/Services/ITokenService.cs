namespace Otus.Microservice.User.Services;

public interface ITokenService
{
    string CreateToken(Models.User user);
}