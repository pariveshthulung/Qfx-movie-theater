using System.ComponentModel.DataAnnotations;

namespace QFX.ViewModels.Auth;

public class ForgetPasswordVm
{
    [Required , EmailAddress]
    public string? Email { get; set; }
    public string? ErrorMessage { get; set; }
    public long? VerificationCode { get; set; }
    public long? ConfirmVerificationCode { get; set; }
    public DateTime Expire { get; set; }
    public string? Password { get; set; }
    [Compare("Password")]
    public string? ConfirmPassword { get; set; }
}
