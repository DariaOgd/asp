using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCBooksWebApi.Models.Domain;
using System.Security.Claims;
using MVCBooksWebApi.Data;

[AllowAnonymous]
public class AccountController : Controller
{
    private readonly MvcBooksDbContext _context;

    public AccountController(MvcBooksDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(User user)
    {
        var loggedInUser = _context.Users.SingleOrDefault(u => u.Username == user.Username && u.Password == user.Password);

        if (loggedInUser != null)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, loggedInUser.Role), 
      
        };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
       
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Books");
        }

        ViewData["ErrorMessage"] = "Niepoprawne login lub hasło.";
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}