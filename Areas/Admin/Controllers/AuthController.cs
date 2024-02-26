using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QFX.data;
using QFX.Manager.Interface;
using QFX.Provider.Interface;
using QFX.ViewModels.Auth;


namespace QFX.Areas.Admin.Controllers;
[AllowAnonymous]
[Area("Admin")]
public class AuthController : Controller
{
    private readonly IAuthManager _authmanager;
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notifyService;
    private readonly ICurrentUserProvider _currentUserProvider;

    public AuthController(IAuthManager authmanager, ApplicationDbContext context, INotyfService notyfService, ICurrentUserProvider currentUserProvider)
    {
        _context = context;
        _authmanager = authmanager;
        _notifyService = notyfService;
        _currentUserProvider = currentUserProvider;
    }
    public IActionResult Login()
    {
        var vm = new LoginVm();
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginVm vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        try
        {
            await _authmanager.Login(vm.Email, vm.Password);
            if (!string.IsNullOrEmpty(vm.ReturnUrl))
            {
                return LocalRedirect(vm.ReturnUrl);
            }
            var currentUser = await _currentUserProvider.GetCurrentUser();
            if (currentUser.UserType == Constants.UserTypeConstants.Admin)
            {
                return RedirectToAction("Index", "Movie", new { area = "Admin" });
            }
            else if(currentUser.UserType == Constants.UserTypeConstants.Employee)
            {
                return RedirectToAction("Index", "Movie", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Index", "Public", new { area = "Public" });
            }
        }
        catch (Exception e)
        {
            vm.ErrorMessage = e.Message;
            return View(vm);
        }
    }
    public IActionResult Registration()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Registration(RegistrationVm vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        try
        {
            var userExit = await _context.Users.AnyAsync(x => x.Email == vm.Email);
            if (!userExit)
            {
                await _authmanager.Registration(vm);
                return RedirectToAction("LogIn", "Auth");
            }
            _notifyService.Error("User already exits with same Email !!!");
            return View(vm);

        }
        catch (Exception e)
        {
            _notifyService.Error("User registration failed." + e.Message);
            return View(vm);

        }
    }
    public async Task<IActionResult> Logout()
    {
        await _authmanager.Logout();
        return RedirectToAction("Index", "Public", new { area = "Public" });
    }


}
