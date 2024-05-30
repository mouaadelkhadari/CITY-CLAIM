using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using useCase.Areas.Identity.Data;
using useCase.Data;

namespace useCase.Controllers
{
    public class InteretController : Controller
    {
        private readonly useCaseDbContext _usecaseDbContext;
        private readonly UserManager<Utilisateur> _userManager;

        public InteretController(useCaseDbContext usecaseDbContext, UserManager<Utilisateur> userManager)
        {
            _usecaseDbContext = usecaseDbContext;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult AddInteret()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Identity/Account/Login");
            }
            var employees = _userManager.Users.Where(u => u.Role == "Employee").ToList();
            ViewBag.Employees = employees;
            return View();
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddInteret(Interet addInteretRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(addInteretRequest.Name) || string.IsNullOrEmpty(addInteretRequest.IdResponsable))
                {
                    ModelState.AddModelError("", "Le nom de l'intérêt et le responsable sont obligatoires.");
                    return View(addInteretRequest);
                }

                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser.Role != "Admin")
                {
                    return Forbid();
                }

                var interet = new Interet()
                {
                    Name = addInteretRequest.Name,
                    Localisation = addInteretRequest.Localisation,
                    IdResponsable = addInteretRequest.IdResponsable,
                    Responsable = await _userManager.FindByIdAsync(addInteretRequest.IdResponsable)
                };

                _usecaseDbContext.Interets.Add(interet);
                _usecaseDbContext.SaveChanges();

                TempData["SuccessMessage"] = "Votre intérêt a été ajouté avec succès.";

                return RedirectToAction("AddInteret");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Une erreur s'est produite lors de l'ajout de l'intérêt : " + ex.Message);
                return View(addInteretRequest);
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult DisplayInterets()
        {
            var interets = _usecaseDbContext.Interets.ToList();
            return View(interets);
        }

    }
}
