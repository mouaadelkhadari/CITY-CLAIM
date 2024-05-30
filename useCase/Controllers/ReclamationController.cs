    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;
    using useCase.Areas.Identity.Data;
    using useCase.Data;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Authorization;

    namespace useCase.Controllers
    {
        public class ReclamationController : Controller
        {
            private readonly useCaseDbContext _dbContext;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly UserManager<Utilisateur> _userManager;

            public ReclamationController(useCaseDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<Utilisateur> userManager)
            {
                this._dbContext = dbContext;
                _httpContextAccessor = httpContextAccessor;
                this._userManager = userManager;
            }


            [Authorize]
            public IActionResult CreateReclamation()
            {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Identity/Account/Login");
            }

            var interets = _dbContext.Interets.ToList();
                ViewBag.Interets = new SelectList(interets, "Id", "Name");
                return View();
            }

            [Authorize]
            public IActionResult DisplayReclamations()
            {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Identity/Account/Login");
            }
            var reclamations = _dbContext.Reclamations.ToList();
                return View(reclamations);
            }


        [Authorize]
        [HttpPost]
            public async Task<IActionResult> AddReclamation(Reclamation AddReclamationRequest)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    var reclamation = new Reclamation()
                    {
                        NomUtilisateur = currentUser.FirstName,
                        PrenomUtilisateur = currentUser.LastName,
                        DateCreation = DateTime.UtcNow,
                        InteretName = AddReclamationRequest.InteretName,
                        Description = AddReclamationRequest.Description,
                        Address = AddReclamationRequest.Address,

                    };

                    var interet = _dbContext.Interets.FirstOrDefault(i => i.Name == AddReclamationRequest.InteretName);
                    if (interet != null)
                    {
                        reclamation.Interet = interet;
                    }

                    // Ajouter la réclamation avec son statut associé
                    _dbContext.CreerReclamationAvecStatut(reclamation, "En attente");

                    TempData["SuccessMessage"] = "Your claim has been successfully submitted.";

                    return RedirectToAction("CreateReclamation");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

        [Authorize]
        public async Task<IActionResult> ReclamationsParNomEtPrenom()
        {
            /*var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToPage("/Identity/Account/Login");
            }*/

            var currentUser = await _userManager.GetUserAsync(User);
            var reclamationsUtilisateur = _dbContext.Reclamations.
                Where(r => r.NomUtilisateur == currentUser.FirstName && r.PrenomUtilisateur == currentUser.LastName)
                .ToList();
            ViewBag.NomUtilisateur = currentUser.FirstName;
            ViewBag.PrenomUtilisateur = currentUser.LastName;
            return View(reclamationsUtilisateur);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var reclamation = await _dbContext.Reclamations.FindAsync(id);
            if (reclamation == null)
            {
                return NotFound();
            }

            _dbContext.Reclamations.Remove(reclamation);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(ReclamationsParNomEtPrenom));
        }

        [Authorize]
        public IActionResult ModifierReclamation(int idReclamation)
        {
            var reclamation = _dbContext.Reclamations.Find(idReclamation);
            if(reclamation == null)
            {
                return NotFound();
            }

            ViewBag.Interets = _dbContext.Interets.ToList();
            
            return View(reclamation);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ModifierReclamation(Reclamation reclamation, int id)
        {
            var existingReclamation = _dbContext.Reclamations.Find(id);
            if(existingReclamation == null)
            {
                return NotFound();
            }
            existingReclamation.InteretName = reclamation.InteretName;
            existingReclamation.Description = reclamation.Description;
            existingReclamation.Address = reclamation.Address;

            _dbContext.Reclamations.Update(existingReclamation);
            _dbContext.SaveChanges();

            return RedirectToAction("ReclamationsParNomEtPrenom", "Reclamation");

        }


        [Authorize]
        [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(Reclamation reclamation)
            {
                if (ModelState.IsValid)
                {
                    _dbContext.Add(reclamation);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(reclamation);
            }


        [Authorize]
        [HttpGet]
            public async Task<IActionResult> GetReclamationStatus(int id)
            {
                var reclamation = await _dbContext.Reclamations.FirstOrDefaultAsync(r => r.Id == id);
                if (reclamation == null)
                {
                    return NotFound();
                }

                var statut = await _dbContext.Statuts.FirstOrDefaultAsync(s => s.ReclamationId == id);
                if (statut == null)
                {
                    return NotFound();
                }

                return Ok(statut.status);
            }

        [Authorize]
        [HttpPost]
            public IActionResult ModifyState(int reclamationId, string newStatus)
            {
                var reclamation = _dbContext.Reclamations.FirstOrDefault(r => r.Id == reclamationId);
                if (reclamation == null)
                {
                    return NotFound();
                }

                // Mettre à jour le statut de la réclamation
                reclamation.status = newStatus;
                _dbContext.SaveChanges();

                // Retourner une réponse JSON indiquant le succès de la modification
                return Json(new { success = true });
            }


        




    }
}
