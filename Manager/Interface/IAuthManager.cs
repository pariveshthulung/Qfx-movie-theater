using QFX.ViewModels.Auth;

namespace QFX.Manager.Interface;

public interface IAuthManager 
{
    Task Login(string username,string password);
    Task Registration(RegistrationVm vm);
    Task Logout();
}
