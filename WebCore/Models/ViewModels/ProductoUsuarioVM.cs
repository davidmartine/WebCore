namespace WebCore.Models.ViewModels
{
    public class ProductoUsuarioVM
    {
        public ProductoUsuarioVM()
        {
            ProductoLista = new List<Producto>();
        }
        public UsuarioAplication UsuarioAplication { get; set; }

        public IList<Producto> ProductoLista { get; set; }



    }
}
