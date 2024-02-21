namespace QFX.Constants;

public class UserStatusConstants
{
    public const string Active = "Active";
    public const string Blocked = "Blocked";
    public const string Inactive = "Inactive";
    public static List<string>? UserStatus = new List<string>{
        Active,Blocked,Inactive
    };
}
