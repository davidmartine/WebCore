using Microsoft.EntityFrameworkCore;
using WebCore.Models;

namespace WebCore.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<TipoAplicacion> TipoAplicacions { get; set; }

        public DbSet<Producto> Productos { get; set; }


    }
}
