using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebCore.Datos;
using WebCore.Models;
using WebCore.Models.ViewModels;

namespace WebCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext dbContext;
        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context)
        {
            dbContext= context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Productos = dbContext.Productos.Include(x => x.Categoria).Include(z => z.TipoAplcacion),
                Categorias = dbContext.Categorias
            };

            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}