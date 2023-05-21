using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using WebCore.Datos;
using WebCore.Models;

namespace WebCore.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class TipoAplicacionController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public TipoAplicacionController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
    
        public IActionResult Index()
        {
            IEnumerable<TipoAplicacion> listarTipoAplicaion = dbContext.TipoAplicacions;

            return View(listarTipoAplicaion);
        }

        //get
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(TipoAplicacion tipoAplicacion)
        {
            dbContext.TipoAplicacions.Add(tipoAplicacion);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Editar(int? Id)
        {
            if(Id == null || Id.Equals(0)) 
            {
                return NotFound();
            }
            var obj = dbContext.TipoAplicacions.Find(Id);
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(TipoAplicacion tipoAplicacion)
        {
            if (ModelState.IsValid)
            {
                dbContext.TipoAplicacions.Update(tipoAplicacion);
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(tipoAplicacion);
        }

        public IActionResult Eliminar(int? Id)
        {
            if(Id == null || Id == 0)
            {
                return NotFound();
            }
            var objet = dbContext.TipoAplicacions.Find(Id);
            if(objet == null)
            {
                return NotFound();
            }
            return View(objet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(TipoAplicacion tipoAplicacion)
        {
            if(tipoAplicacion == null)
            {
                return NotFound();
            }
            dbContext.Remove(tipoAplicacion);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
