using CorporatePortal.AuthServer.Models;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;

namespace CorporatePortal.AuthServer.Services;

public class CustomProfileService : IProfileService
{
    private readonly UserManager<Collaborator> _userManager;

    public CustomProfileService(UserManager<Collaborator> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        if (user == null) return;

        var claims = context.Subject.Claims.ToList();
        
        claims.Add(new System.Security.Claims.Claim("first_name", user.FirstName ?? ""));
        claims.Add(new System.Security.Claims.Claim("last_name", user.LastName ?? ""));
        claims.Add(new System.Security.Claims.Claim("full_name", user.FullName ?? ""));
        if (!string.IsNullOrEmpty(user.Photo))
        {
            claims.Add(new System.Security.Claims.Claim("photo", user.Photo));
        }

        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        context.IsActive = user != null;
    }
}

