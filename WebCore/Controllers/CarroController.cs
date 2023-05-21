using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using WebCore.Datos;
using WebCore.Models;
using WebCore.Models.ViewModels;
using WebCore.Utilidades;

namespace WebCore.Controllers
{
    [Authorize]
    public class CarroController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;



        [BindProperty]
        public ProductoUsuarioVM ProductoUsuarioVM { get; set; }

        public CarroController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment,
                               IEmailSender emailSender)
        {
            _context= context;
            _webHostEnvironment= webHostEnvironment;
            _emailSender= emailSender;
        }

        
        public IActionResult Index()
        {
            List<CarroCompras> carroComprasList = new List<CarroCompras>();

            if(HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras) != null &&
               HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras).Count()>0)
            {
                carroComprasList = HttpContext.Session.Get<List<CarroCompras>>(WC.SessionCarroCompras);

            }

            List<int> prodEnCarro = carroComprasList.Select(x => x.ProductoId).ToList();
            IEnumerable<Producto> prodclist = _context.Productos.Where(x => prodEnCarro.Contains(x.Id));

            return View(prodclist);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Resumen));
        }


        public IActionResult Resumen()
        {
            //Traer el usuario que esta conectado
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            List<CarroCompras> carroComprasList = new List<CarroCompras>();

            if (HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras) != null &&
               HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasList = HttpContext.Session.Get<List<CarroCompras>>(WC.SessionCarroCompras);

            }

            List<int> prodEnCarro = carroComprasList.Select(x => x.ProductoId).ToList();
            IEnumerable<Producto> prodclist = _context.Productos.Where(x => prodEnCarro.Contains(x.Id));

            ProductoUsuarioVM = new ProductoUsuarioVM()
            {
                UsuarioAplication = _context.UsuarioAplications.FirstOrDefault(x => x.Id == claim.Value),
                ProductoLista = prodclist.ToList()
            };

            return View(ProductoUsuarioVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Resumen")]
        public async Task<IActionResult> ResumenPost(ProductoUsuarioVM productoUsuarioVM)
        {
            var rutaTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                               + "templates" + Path.DirectorySeparatorChar.ToString()
                               + "PlantillaOrden.html";

            var subject = "Nueva Ordern";
            string HtmlBody = "";

            using (StreamReader sr = System.IO.File.OpenText(rutaTemplate))
            {
                HtmlBody = sr.ReadToEnd();

            }
            //Nombre: { 0}
            //Email: { 1}
            //Telefono: { 2}
            //Productos: { 3}

            StringBuilder productoListSB = new StringBuilder();

            foreach (var prod in productoUsuarioVM.ProductoLista)
            {
                productoListSB.Append($" - Nombre: {prod.NombreProducto} " +
                    $"<span style='font-size: 14px;'> (Id: {prod.Id}) </span><br/>");

            }

            string messageBody = string.Format(HtmlBody,
                                 ProductoUsuarioVM.UsuarioAplication.NombreCompleto,
                                 ProductoUsuarioVM.UsuarioAplication.Email,
                                 ProductoUsuarioVM.UsuarioAplication.PhoneNumber,
                                 productoListSB.ToString());

            await _emailSender.SendEmailAsync(WC.EmailAdmin, subject,messageBody);


            return RedirectToAction(nameof(Confirmacion));
        }

        public IActionResult Confirmacion()
        {
            HttpContext.Session.Clear();
            return View();
        }


        public IActionResult Remover(int id)
        {
            List<CarroCompras> carroComprasList = new List<CarroCompras>();

            if (HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras) != null &&
               HttpContext.Session.Get<IEnumerable<CarroCompras>>(WC.SessionCarroCompras).Count() > 0)
            {
                carroComprasList = HttpContext.Session.Get<List<CarroCompras>>(WC.SessionCarroCompras);

            }

            carroComprasList.Remove(carroComprasList.FirstOrDefault(x => x.ProductoId == id));
            HttpContext.Session.Set(WC.SessionCarroCompras, carroComprasList);

            return RedirectToAction(nameof(Index));

        }
    }
}
