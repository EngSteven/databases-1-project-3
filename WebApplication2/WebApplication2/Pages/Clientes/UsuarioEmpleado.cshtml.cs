using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication2.Pages.Clientes
{
    public class UsuarioEmpleadoModel : PageModel
    {
        public List<ClaseArticulo> listaArticulos = new List<ClaseArticulo>();
        public String nombreUsuarioEmpleado = Global.nombreUsuarioEmpleado;
        public String docIdentificacionEmpleado = Global.docIdentificacionEmpleado;
        public String errorMessage = "";
        public void OnGet()
        {

        }
    }
}
