using Microsoft.AspNetCore.Identity;

namespace WebCore.Models
{
    public class UsuarioAplication : IdentityUser
    {
        public string NombreCompleto { get; set; }


    }
}
