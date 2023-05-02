using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCore.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre del Producto es Obligatorio")]
        public string NombreProducto { get; set; }

        [Required(ErrorMessage = "Descripcion es Obligatorio")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La Observacion es Obligatorio")]
        public string Observacion { get; set; }

        [Required(ErrorMessage = "El Precio del Producto es Obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
        public double Precio { get; set; }

        public string? ImagenUrl { get; set; }

        //Foring Key

        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual Categoria? Categoria { get; set; }

        public int TipoAplicacionId { get; set; }


        [ForeignKey("TipoAplicacionId")]
        public virtual TipoAplicacion? TipoAplcacion { get; set; }


    }
}
