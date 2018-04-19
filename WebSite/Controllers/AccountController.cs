using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebSite.ViewModel;

namespace WebSite.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Index([FromQuery] string ReturnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Home", "Index");
            }

            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Login Login, string returnUrl)
        {
            //TODO: Validar usuário no banco de dados
            if (Login.UserName == "anderson" && Login.Password == "ajesus")
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, Login.UserName),
                    new Claim(ClaimTypes.Role, "admin")
                };

                var id = new ClaimsIdentity(claims, "password");

                var principal = new ClaimsPrincipal(id);

                await HttpContext.SignInAsync("app", principal, new AuthenticationProperties() { IsPersistent = Login.IsPersistent });

                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return LocalRedirect(returnUrl);
                }
            }

            ViewBag.MsgError = "Usuário/Senha inválido";
            return View();
        }

        public async Task<IActionResult> Logoff()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}