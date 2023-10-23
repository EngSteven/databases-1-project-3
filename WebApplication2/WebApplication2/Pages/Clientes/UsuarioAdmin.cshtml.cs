using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Xml.Linq;

namespace WebApplication2.Pages.Clientes
{
    public class UsuarioAdminModel : PageModel
    {
        public String Nombre = "";
        public String Filtro = "";
        public Empleado empleado = new Empleado();
        public List<Empleado> listaEmpleado = new List<Empleado>();
        public String errorMessage = "";

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=pruebajose2312.database.windows.net;Initial Catalog=prueba2312;Persist Security Info=True;User ID=adminjose;Password=Bases1234";
                String spName = "dbo.ListaEmpleados";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();                                      //Se abre la coneccion con la BD.
                    using (SqlCommand command = new SqlCommand(spName, connection))
                    {
                        //Variables para obtener el DataSet mandados de la BD.
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        DataTable table = new DataTable();

                        command.CommandType = CommandType.StoredProcedure;  //Indicar que el comando sera un SP.
                                                                            //Codigo para que detecte el output del SP.
                                                                            //command.Parameters.AddWithValue("@inCantidad", cantidad);
                        command.Parameters.AddWithValue("@inUsuario", Global.sesion);
                        command.Parameters.AddWithValue("@inIP", Global.IP);

                        SqlParameter resultCodeParam = new SqlParameter("@outResultCode", SqlDbType.Int);
                        resultCodeParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(resultCodeParam);
                        
                        //Porceso de obtener el DataSet.
                        adapter.SelectCommand = command;
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        //Si el output de errores por DataSet es 0 (No hay problemas).
                        if (dataSet.Tables[0].Rows[0][0].ToString().Equals("0"))
                        {
                            foreach (DataRow row in dataSet.Tables[1].Rows) //Recorra cada fila de la tabla con los datos y estraigala en el tipo ClienteInfo.
                            {
                                Empleado empleado = new Empleado();
                                empleado.Id = "" + row[0];
                                empleado.Nombre = "" + row[1];
                                empleado.Puesto = "" + row[2];
                                
                                
                                listaEmpleado.Add(empleado);             //Añadir cada fila al array para su visualizacion.
                            }
                        }
                        else
                        {
                            //En caso de que haya algun error al cargar la tabla de articulos.
                            errorMessage = "Error al cargar la lista de articulos.";
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        public void OnPost(string action, string idEmpleado)
        {
            Global.idUsuarioEmpleado = idEmpleado;

            if (action == "Editar")
            {
                // Realiza la lógica para la acción de "Editar" aquí.
                
                Console.WriteLine("Boton editar");
                Console.WriteLine("ID:" + idEmpleado);
                Response.Redirect("/Clientes/Modificar");
            }
            else if (action == "Borrar")
            {
                // Realiza la lógica para la acción de "Borrar" aquí.
                Console.WriteLine("Boton borrar");
                Console.WriteLine("ID:" + idEmpleado);
            }
            else if (action == "Impersonar")
            {
                // Realiza la lógica para la acción de "Impersonar" aquí.
                Console.WriteLine("Boton impersonar");
                Console.WriteLine("ID:" + idEmpleado);
            }

            // Puedes acceder a la fila correspondiente y sus datos utilizando el valor de `id`.

        }

        public async Task OnPostFiltrar()
        {
            try
            {
                Filtro = Request.Form["Filtrar"];
                empleado = new Empleado();

                String connectionString = "Data Source=pruebajose2312.database.windows.net;Initial Catalog=prueba2312;Persist Security Info=True;User ID=adminjose;Password=Bases1234";
                String spName = "FiltrarNombreEmpleado";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();                                      //Se abre la coneccion con la BD.
                    using (SqlCommand command = new SqlCommand(spName, connection))
                    {
                        //Variables para obtener el DataSet mandados de la BD.
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        DataTable table = new DataTable();

                        command.CommandType = CommandType.StoredProcedure;  //Indicar que el comando sera un SP.
                                                                            //Codigo para que detecte el output del SP.
  
                        command.Parameters.AddWithValue("@inNombre", Filtro);
                        command.Parameters.AddWithValue("@inUsuario", Global.sesion);
                        command.Parameters.AddWithValue("@inIP", Global.IP);
                        SqlParameter resultCodeParam = new SqlParameter("@outResultCode", SqlDbType.Int);
                        resultCodeParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(resultCodeParam);

                        //Porceso de obtener el DataSet.
                        adapter.SelectCommand = command;
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        //Si el output de errores por DataSet es 0 (No hay problemas).
                        if (dataSet.Tables[0].Rows[0][0].ToString().Equals("0"))
                        {
                            foreach (DataRow row in dataSet.Tables[1].Rows) //Recorra cada fila de la tabla con los datos y estraigala en el tipo ClienteInfo.
                            {
                                Empleado empleado = new Empleado();
                                empleado.Nombre = "" + row[0];
                                empleado.Puesto = "" + row[1];

                                listaEmpleado.Add(empleado);             //Añadir cada fila al array para su visualizacion.
                            }
                        }
                        else
                        {
                            //En caso de que haya algun error al cargar la tabla de articulos.
                            errorMessage = "Error al buscar por cantidad en la lista de articulos.";
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

        }

    }
}
