using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace WebCore.Models
{
    public class TipoAplicacion
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="El Campo {0} es Obligatorio")]
        [MaxLength(200)]
        public string? NombreTipoAplicacion { get; set; }


    }
}
