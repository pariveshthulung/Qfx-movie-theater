using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.CodeAnalysis;
using QFX.Constants;

namespace QFX.Entity;

public class User
{
    public long ID { get; set; }
    public string? Name { get; set; }
    [EmailAddress]
    public string? Email { get; set; }
    public long PhoneNo { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? PasswordHash { get; set; }
    public string? UserType { get; set; } = UserTypeConstants.User;
    public string? UserStatus { get; set; } = UserStatusConstants.Active;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public long? LocationID { get; set; }
    [ForeignKey("LocationID")]
    public virtual Models.Location? Location { get; set; }

}
