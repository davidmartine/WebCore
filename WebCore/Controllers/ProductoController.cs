using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCore.Datos;
using WebCore.Models;
using WebCore.Models.ViewModels;

namespace WebCore.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductoController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Producto> listaProducto = dbContext.Productos.Include(x => x.Categoria)
                                                                     .Include(x => x.TipoAplcacion);

            return View(listaProducto);
        }

        public IActionResult Upsert(int? Id)
        {
            //IEnumerable<SelectListItem> categoriaDropDown = dbContext.Categorias.Select(x => new SelectListItem
            //{
            //    Text = x.NombreCategoria,
            //    Value = x.Id.ToString()
            //});
            //ViewBag.categoriaDropDown = categoriaDropDown;

            //Producto producto = new Producto();
            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = dbContext.Categorias.Select(x => new SelectListItem
                {
                    Text = x.NombreCategoria,
                    Value = x.Id.ToString()
                }),
                TipoAplicacionLista = dbContext.TipoAplicacions.Select(x => new SelectListItem
                {
                    Text = x.NombreTipoAplicacion,
                    Value = x.Id.ToString()
                })
            };
            if(Id == null)
            {
                //Crear nuevo producto
                return View(productoVM);
            }
            else
            {
                productoVM.Producto = dbContext.Productos.Find(Id);
                if(productoVM.Producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPaht = webHostEnvironment.WebRootPath;
                if(productoVM.Producto.Id == 0)
                {
                    //Crear
                    string upload = webRootPaht + WC.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productoVM.Producto.ImagenUrl = fileName + extension;
                    dbContext.Add(productoVM.Producto);
                }
                else
                {
                    //Actualizar
                    var objetoProducto = dbContext.Productos.AsNoTracking().FirstOrDefault(x => x.Id == productoVM.Producto.Id);
                    
                    if(files.Count > 0) //Se carga imagen nueva
                    {
                        string upload = webRootPaht + WC.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        //Borrar imagen anterior
                        var anteriorFile = Path.Combine(upload, objetoProducto.ImagenUrl);
                        if (System.IO.File.Exists(anteriorFile))
                        {
                            System.IO.File.Delete(anteriorFile);
                        }// fin de borrado imagen anterior


                        //crear nueva imagen
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productoVM.Producto.ImagenUrl = fileName + extension;

                    }
                    //caso si no se cambia la imagen
                    else
                    {
                        productoVM.Producto.ImagenUrl = objetoProducto.ImagenUrl;

                    }
                    dbContext.Productos.Update(productoVM.Producto);
                }
                dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            //se llenan nuevamente las listas
            productoVM.CategoriaLista = dbContext.Categorias.Select(x => new SelectListItem
            {
                Text = x.NombreCategoria,
                Value = x.Id.ToString()
            });
            productoVM.TipoAplicacionLista = dbContext.TipoAplicacions.Select(x => new SelectListItem
            {
                Text = x.NombreTipoAplicacion,
                Value = x.Id.ToString()
            });

            return View(productoVM);;
        }

        public IActionResult Eliminar(int? Id)
        {
            if(Id == null || Id == 0)
            {
                return NotFound();
            }

            Producto producto = dbContext.Productos.Include(x => x.Categoria)
                                                   .Include(x => x.TipoAplcacion)
                                                   .FirstOrDefault(x => x.Id == Id);

            if(producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Producto producto)
        {
            if(producto == null)
            {
                return NotFound();
            }
            //Eliminar imagen
            string upload = webHostEnvironment.WebRootPath + WC.ImagenRuta;
            

            //Borrar imagen anterior
            var anteriorFile = Path.Combine(upload, producto.ImagenUrl);
            if (System.IO.File.Exists(anteriorFile))
            {
                System.IO.File.Delete(anteriorFile);
            }// fin de borrado imagen anterior

            dbContext.Remove(producto);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
