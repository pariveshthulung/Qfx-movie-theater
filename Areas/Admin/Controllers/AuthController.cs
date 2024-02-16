using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QFX.data;
using QFX.Manager.Interface;
using QFX.ViewModels.Auth;


namespace QFX.Areas.Admin.Controllers;
[AllowAnonymous]
[Area("Admin")]
public class AuthController : Controller
{
    private readonly IAuthManager _authmanager;
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notifyService;
    public AuthController(IAuthManager authmanager,ApplicationDbContext context,INotyfService notyfService)
    {   
        _context = context;
        _authmanager = authmanager;
        _notifyService = notyfService;
    }
    public IActionResult Login(){
        var vm = new LoginVm();
        return View(vm);
    }
    [HttpPost]
    public IActionResult Login(LoginVm vm){
        try{
            if(!ModelState.IsValid)
            {
                return View(vm);
            }

            _authmanager.Login(vm.Email,vm.Password);
            // if(!string.IsNullOrEmpty(vm.ReturnUrl))
            // {
            //     return LocalRedirect(vm.ReturnUrl);
            // }
            return RedirectToAction("Index","Public",new {area="Public"});
        }
        catch(Exception e)
        {
            vm.ErrorMessage = e.Message;
            return View(vm);
        }
    }


}
