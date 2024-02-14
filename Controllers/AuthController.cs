using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QFX.Manager;
using QFX.Manager.Interface;
using QFX.ViewModels.Auth;

namespace QFX.Controllers;
[AllowAnonymous]
public class AuthController : Controller
{
    private readonly IAuthManager _authmanager;
    public AuthController(IAuthManager authmanager)
    {
        _authmanager = authmanager;
    }
    public IActionResult Login(){
        var vm = new LoginVm();
        return View(vm);
    }
}
