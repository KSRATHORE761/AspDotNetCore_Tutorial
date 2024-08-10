using CookieAuthentication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace CookieAuthentication.Controllers
{
    public class AccountController : Controller
    {
        public List<UserModel> users = null; 
        public AccountController() 
        {
            users = new List<UserModel>();
            users.Add(new UserModel()
            {
                UserId = 1,
                UserName = "Kuldeep",
                Password = "123",
                Role = "Admin"
            });
            users.Add(new UserModel()
            {
                UserId = 2, 
                UserName = "Other", 
                Password = "123", 
                Role = "User"
            });
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string ReturnUrl = "/")
        {
            LoginModel modelObj = new LoginModel();
            modelObj.ReturnUrl = ReturnUrl;
            return View(modelObj);
        }
        [HttpPost]
        public async Task <IActionResult> Login(LoginModel loginModelObj)
        {
            if (ModelState.IsValid) 
            {
                var user = users.Where(x=> x.UserName== loginModelObj.UserName && x.Password==loginModelObj.Password).FirstOrDefault();
                if (user == null) 
                {
                    ViewBag.Message = "Invalid Credential";
                    return View(loginModelObj);
                }
                else
                {
                    //A claim is a statement about a subject by an issuer and    
                    //represent attributes of the subject that are useful in the context of authentication and authorization operations.    
                    var claims = new List<Claim>() 
                    { 
                        new Claim(ClaimTypes.NameIdentifier,Convert.ToString(user.UserId)),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role,user.Role),
                        new Claim("FavoriteDrink", "Tea")
                    };
                    //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme   
                    var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                    var principal = new ClaimsPrincipal(identity);
                    //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal ,new AuthenticationProperties()
                    {
                        IsPersistent = loginModelObj.RememberMe,
                    });
                    return LocalRedirect(loginModelObj.ReturnUrl);
                }

            }
            return View(loginModelObj);
        }
        public async Task<ActionResult> Logout()
        {
            //SignOutAsync is Extension method for SignOut 
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page    
            return LocalRedirect("/");
        }
    }
}
