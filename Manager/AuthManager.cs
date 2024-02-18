using System.Security.Claims;
using System.Transactions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using QFX.Constants;
using QFX.data;
using QFX.Entity;
using QFX.Manager.Interface;
using QFX.ViewModels.Auth;

namespace QFX.Manager;

public class AuthManager : IAuthManager
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuthManager(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task Login(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x=> x.Email == username);
        if(user == null){
            throw new Exception("User doesnot exist!!");
        }
        if(!BCrypt.Net.BCrypt.Verify(password,user.PasswordHash)){
            throw new Exception("Invalid password");
        }
        var httpContext = _httpContextAccessor.HttpContext;
        var claim = new List<Claim>{
            new("ID", user.ID.ToString())
        };
        if(user.UserType == UserTypeConstants.Admin){
            claim.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType,"Admin"));
        }
        var claimsIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
        new ClaimsPrincipal(claimsIdentity));
    }

    public async Task Logout()
    {
        await _httpContextAccessor.HttpContext.SignOutAsync();
    }

    public async Task Registration(RegistrationVm vm)
    {
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var user = new User();
            user.Name = vm.Name;
            user.Email = vm.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(vm.Password);
            user.PhoneNo = vm.PhoneNo;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            tx.Complete();
    }
}
