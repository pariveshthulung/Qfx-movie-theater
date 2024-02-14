namespace QFX.Manager.Interface;

public interface IAuthManager 
{
    Task Login(string username,string password);
    Task Logout();
}
