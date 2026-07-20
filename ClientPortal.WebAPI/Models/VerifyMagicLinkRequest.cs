namespace ClientPortal.WebAPI.Models;

public class VerifyMagicLinkRequest
{
    public string Token { get; set; } = string.Empty;
}