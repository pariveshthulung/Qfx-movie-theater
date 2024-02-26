using QFX.Models;

namespace QFX.Entity;

public class UserLocationPreference
{
    public long Id { get; set; }
    public long UserId {get;set;}

    public virtual User User {get;set;}
    public long LocationId {get;set;}

    public virtual Location Location {get;set;}
}
