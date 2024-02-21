namespace QFX.Constants;

public class UserTypeConstants
{
    public const string Admin = "Admin";
    public const string Employee = "Employee";
    public const string User = "User";
    public static List<string>? UserType = new List<string>{
        Admin,Employee,User
    };

}
