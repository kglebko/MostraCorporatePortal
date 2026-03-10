using Microsoft.AspNetCore.Authentication;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CorporatePortal.AuthServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Events;

namespace CorporatePortal.AuthServer.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly SignInManager<Collaborator> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IEventService _events;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        SignInManager<Collaborator> signInManager,
        IIdentityServerInteractionService interaction,
        IEventService events,
        ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _interaction = interaction;
        _events = events;
        _logger = logger;
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        return View(new LoginViewModel { ReturnUrl = returnUrl ?? "/" });
    }

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginInputModel model)
    {
        var returnUrl = model.ReturnUrl ?? "/";

        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberLogin,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                var user = await _signInManager.UserManager.FindByNameAsync(model.Username);
                await _events.RaiseAsync(new UserLoginSuccessEvent(
                    user?.UserName,
                    user?.Id,
                    user?.UserName,
                    clientId: null));

                if (_interaction.IsValidReturnUrl(returnUrl) || Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return Redirect("~/");
            }

            await _events.RaiseAsync(new UserLoginFailureEvent(
                model.Username,
                "invalid credentials",
                clientId: null));

            ModelState.AddModelError(string.Empty, "Неверный логин или пароль");
        }

        return View(new LoginViewModel
        {
            ReturnUrl = returnUrl,
            Error = "Неверный логин или пароль",
            RememberLogin = model.RememberLogin
        });
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout(string? logoutId = null)
    {
        await _signInManager.SignOutAsync();

        var subjectId = User?.FindFirst("sub")?.Value;
        var name = User?.Identity?.Name;

        await _events.RaiseAsync(
            new UserLogoutSuccessEvent(subjectId, name));

        var logoutContext = await _interaction.GetLogoutContextAsync(logoutId);

        if (logoutContext?.PostLogoutRedirectUri != null)
        {
            return Redirect(logoutContext.PostLogoutRedirectUri);
        }

        return Redirect("~/");
    }
}

