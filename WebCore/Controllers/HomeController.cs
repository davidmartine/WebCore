using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebCore.Datos;
using WebCore.Models;
using WebCore.Models.ViewModels;
using WebCore.Utilidades;

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

        public IActionResult Detalle(int Id)
        {
            List<CarroCompras> carroComprasLista = new List<CarroCompras>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras) != null
                && HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompras>>(WC.SessionCarroCompras);

            }
            DetalleVM detalleVM = new DetalleVM()
            {
                Producto = dbContext.Productos.Include(x => x.Categoria).Include(z => z.TipoAplcacion)
                           .Where(x => x.Id == Id).FirstOrDefault(),
                ExisteEnCarro = false
            };
            foreach (var item in carroComprasLista)
            {
                if (item.ProductoId == Id)
                {
                    detalleVM.ExisteEnCarro = true;

                }
            }
            return View(detalleVM);
        }

        [HttpPost, ActionName("Detalle")]
        public IActionResult DetallePost(int Id)
        {
            List<CarroCompras> carroComprasLista = new List<CarroCompras>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras) != null
                && HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras).Count() >0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompras>>(WC.SessionCarroCompras);

            }
            carroComprasLista.Add(new CarroCompras { ProductoId = Id });
            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasLista);

            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult RemoverDeCarro(int Id)
        {
            List<CarroCompras> carroComprasLista = new List<CarroCompras>();
            if (HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras) != null
                && HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasLista = HttpContext.Session.Get<List<CarroCompras>>(WC.SessionCarroCompras);

            }
            var productoARemover = carroComprasLista.SingleOrDefault(x => x.ProductoId == Id);
            if(productoARemover != null)
            {
                carroComprasLista.Remove(productoARemover);
            }

            //carroComprasLista.Add(new CarroCompras { ProductoId = Id });
            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasLista);

            return RedirectToAction(nameof(Index));
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