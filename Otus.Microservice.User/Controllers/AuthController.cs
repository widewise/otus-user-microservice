using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Otus.Microservice.User.Services;
using Otus.Microservice.User.ViewModel;

namespace Otus.Microservice.User.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly UserManager<Models.User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;

    public AuthController(
        ILogger<AuthController> logger,
        UserManager<Models.User> userManager,
        ITokenService tokenService,
        AppDbContext context)
    {
        _logger = logger;
        _userManager = userManager;
        _tokenService = tokenService;
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }
 

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginViewModel
        {
            ReturnUrl = returnUrl
        });
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegistrationViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var result = await _userManager.CreateAsync(
            new Models.User { UserName = viewModel.Username, Email = viewModel.Email },
            viewModel.Password
        );
        if (result.Succeeded)
        {
            viewModel.Password = string.Empty;
            return RedirectToAction(nameof(Login));
        }

        foreach (var error in result.Errors)
        {
            _logger.LogError(
                "User registration error {ErrorCode}: {ErrorDescription}",
                error.Code,
                error.Description);
            TempData["Message"] = $"Registration error: {error.Code} {error.Description}";
        }
        return View(viewModel);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var managedUser = await _userManager.FindByEmailAsync(model.Email);
        if (managedUser == null)
        {
            TempData["Message"] = "Bad credentials";
            return View(model);
        }
        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, model.Password);
        if (!isPasswordValid)
        {
            TempData["Message"] = "Bad credentials";
            return View(model);
        }
        var userInDb = _context.Users.FirstOrDefault(u => u.Email == model.Email);
        if (userInDb is null)
        {
            TempData["Message"] = "User doesn't Exist";
            return View(model);
        }

        var accessToken = _tokenService.CreateToken(userInDb);
        await _context.SaveChangesAsync();
        HttpContext.Session.SetString("Token", accessToken);
        var l = HttpContext.Session.GetString("Token");
        return RedirectToLocal(model.ReturnUrl);
    }

    private ActionResult RedirectToLocal(string? returnUrl)
    {
        if (returnUrl != null && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        return Redirect("/swagger");
    }
}