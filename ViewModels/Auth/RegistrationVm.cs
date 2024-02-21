using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QFX.ViewModels.Auth;

public class RegistrationVm
{
    public string? Name { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    public long PhoneNo { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public DateTime DateOfBirth { get; set; }

}
