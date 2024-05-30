using useCase.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using useCase.Models;

namespace useCase.Controllers
{
    public class UserController : Controller
    {
        private readonly useCaseDbContext usecaseDbContext;

        public UserController(useCaseDbContext usecaseDbContext)
        {
            this.usecaseDbContext = usecaseDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
