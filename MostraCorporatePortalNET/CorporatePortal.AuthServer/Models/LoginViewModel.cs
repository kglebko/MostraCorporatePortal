namespace CorporatePortal.AuthServer.Models;

public class LoginViewModel
{
    public string? ReturnUrl { get; set; }

    public string? Error { get; set; }

    public bool RememberLogin { get; set; } 
}

public class LoginInputModel
{
    public string Username { get; set; }

    public string Password { get; set; }

    public bool RememberLogin { get; set; }

    public string? ReturnUrl { get; set; }
}

