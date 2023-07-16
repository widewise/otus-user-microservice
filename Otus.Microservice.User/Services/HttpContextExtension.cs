using System.Security.Claims;

namespace Otus.Microservice.User.Services;

public static class HttpContextExtension
{
    public static bool HasUserId(this HttpContext httpContext, out string userId)
    {
        var innerUserId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var hasUserId = !string.IsNullOrEmpty(innerUserId?.Value);
        userId = innerUserId?.Value ?? string.Empty;
        return hasUserId;
    }
}