namespace CorporatePortal.AuthServer.Models;

public class LoginViewModel
{
    public string? ReturnUrl { get; set; }
    public string? Error { get; set; }
}

public class LoginInputModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberLogin { get; set; }
    public string ReturnUrl { get; set; } = string.Empty;
}

