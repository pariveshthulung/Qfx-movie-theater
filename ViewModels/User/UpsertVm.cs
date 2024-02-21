using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using QFX.Constants;

namespace QFX.ViewModels.UserVm;

public class UpsertVm
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public long PhoneNo { get; set; }
    public string? Password { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? ConfirmPassword { get; set; }
    public long? LocationID { get; set; }
    public List<Models.Location>? Location { get; set; }
    public string? UserType { get; set; }
    public string? UserStatus { get; set; }

    public SelectList LocationList()
    {
        return new SelectList(
            Location,
            nameof(Models.Location.ID),
            nameof(Models.Location.CityName),
            LocationID

        );
        
    }
    public SelectList UserTypeList()
    {
        return new SelectList(
            UserTypeConstants.UserType,
            UserType
        );
        
    }
    public SelectList UserStatusList()
    {
        return new SelectList(
            UserStatusConstants.UserStatus,
            UserStatus
        );
        
    }
}
