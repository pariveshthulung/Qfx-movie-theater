using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using QFX.data;
using QFX.Entity;
using QFX.Manager.Interface;

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
        var user = await _context.Users.FirstOrDefaultAsync(x=> x.Email==username);
        if(user == null){
            throw new Exception("User doesnot exist!!");
        }
        if(!BCrypt.Net.BCrypt.Verify(password,user.PasswordHash)){
            throw new Exception("Invalid password");
        }
        var httpContext = _httpContextAccessor.HttpContext;
        var claims = new List<Claim>{
            new("ID", user.ID.ToString())
        };
        var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
        new ClaimsPrincipal(claimIdentity));
    }

    public async Task Logout()
    {
        await _httpContextAccessor.HttpContext.SignOutAsync();
    }
}
