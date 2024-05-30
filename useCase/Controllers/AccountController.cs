using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using useCase.Areas.Identity.Data;

namespace useCase.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Utilisateur> _signInManager;

        public AccountController(SignInManager<Utilisateur> signInManager)
        {
            _signInManager = signInManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            HttpContext.Session.Remove("UserId");

            return RedirectToAction("Index", "Home");
        }

    }
}
