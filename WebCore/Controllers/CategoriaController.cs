using Microsoft.AspNetCore.Mvc;
using WebCore.Datos;
using WebCore.Models;

namespace WebCore.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CategoriaController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Categoria> listar = dbContext.Categorias;

            return View(listar);
        }

        //Get
        public IActionResult Crear()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                dbContext.Categorias.Add(categoria);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
            
        }

        public IActionResult Editar(int? Id)
        {
            if(Id == null || Id.Equals(0))
            {
                return NotFound();

            }
            var objeto = dbContext.Categorias.Find(Id);
            if(objeto == null) { return NotFound(); }

            return View(objeto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Categoria categoria)
        {
            if(ModelState.IsValid)
            {
                dbContext.Categorias.Update(categoria);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public IActionResult Eliminar(int? Id)
        {
            if(Id == null || Id.Equals(0))
            {
                return NotFound();
            }
            var objeto = dbContext.Categorias.Find(Id);
            if(objeto == null) { return NotFound(); }
            return View(objeto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Categoria categoria)
        {
            if(categoria == null)
            {
                return NotFound();
            }
            dbContext.Categorias.Remove(categoria);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
