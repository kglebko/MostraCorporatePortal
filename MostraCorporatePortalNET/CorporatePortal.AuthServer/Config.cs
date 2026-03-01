using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace CorporatePortal.AuthServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("api", "Corporate Portal API")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // BFF client for Vue.js frontend
            new Client
            {
                ClientId = "corporate-portal-bff",
                ClientName = "Corporate Portal BFF",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                
                RedirectUris = { "http://localhost:5173/callback", "https://localhost:5173/callback" },
                PostLogoutRedirectUris =
                {
                    "http://localhost:5173/logout-callback",
                    "https://localhost:5173/logout-callback"
                },
                AllowedCorsOrigins = { "http://localhost:5173", "https://localhost:5173" },
                
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "api"
                },
                
                AllowAccessTokensViaBrowser = true,
                AccessTokenLifetime = 3600,
                IdentityTokenLifetime = 3600
            }
        };
}

