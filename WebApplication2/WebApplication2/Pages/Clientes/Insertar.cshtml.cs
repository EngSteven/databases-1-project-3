using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Net;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace WebApplication2.Pages.Clientes
{
    public class InsertarModel : PageModel
    {

        public List<String> listaTipoDocIdentidad = new List<String>();
        public List<String> listaPuesto = new List<String>();
        public List<String> listaDepartamento = new List<String>();
        public String codigoIngresado = "";
        public String errorMessage = "";                                    //Variable para los mensajes de error
        public String successMessage = "";
        public Empleado empleado = new Empleado();

        public static String[] caracteresDisponibles = { "0123456789", "abcdefghijklmnopqrstuvwxyz", "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "@#!$*?" };
        public String nombreUsuario = "";
        public String clave = "";

        public void OnGet()
        {
            try
            {
                String SPNombre = "dbo.BuscarEmpleadoPorId";
                String connectionString = "Data Source=pruebajose2312.database.windows.net;Initial Catalog=prueba2312;Persist Security Info=True;User ID=adminjose;Password=Bases1234";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();                                      //Se abre la coneccion con la BD.
                    using (SqlCommand command = new SqlCommand(SPNombre, connection))
                    {
                        //Variables para obtener el DataSet mandados de la BD.
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        DataTable table = new DataTable();

                        command.CommandType = CommandType.StoredProcedure;  //Indicar que el comando sera un SP.
                        command.Parameters.AddWithValue("@inIdTipoEvento", 0);
                        command.Parameters.AddWithValue("@inIdEmpleado", 1);
                        command.Parameters.AddWithValue("@inUsuario", Global.sesion);
                        command.Parameters.AddWithValue("@inIP", Global.IP);

                        //Codigo para que detecte el output del SP.
                        SqlParameter resultCodeParam = new SqlParameter("@outResultCode", SqlDbType.Int);
                        resultCodeParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(resultCodeParam);
                        //command.ExecuteNonQuery();


                        //Porceso de obtener el DataSet.
                        adapter.SelectCommand = command;
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        //Si el output de errores por DataSet es 0 (No hay problemas).
                        if (dataSet.Tables[0].Rows[0][0].ToString().Equals("0"))
                        {
                            foreach (DataRow row in dataSet.Tables[2].Rows) //Recorra cada fila de la tabla con los datos y estraigala en el tipo ClienteInfo.
                            {
                                String tipoDocIdentidad = "" + row[0];
                                listaTipoDocIdentidad.Add(tipoDocIdentidad);             //A?adir cada fila al array para su visualizacion.
                            }

                            foreach (DataRow row in dataSet.Tables[3].Rows) //Recorra cada fila de la tabla con los datos y estraigala en el tipo ClienteInfo.
                            {
                                String puesto = "" + row[0];
                                listaPuesto.Add(puesto);             //A?adir cada fila al array para su visualizacion.
                            }

                            foreach (DataRow row in dataSet.Tables[4].Rows) //Recorra cada fila de la tabla con los datos y estraigala en el tipo ClienteInfo.
                            {
                                String departamento = "" + row[0];
                                listaDepartamento.Add(departamento);             //A?adir cada fila al array para su visualizacion.
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

        public void OnPost()
        {
            empleado.Nombre = Request.Form["Nombre"];
            empleado.TipoDocIdentidad = Request.Form["TipoDocIdentidad"];
            empleado.ValorDocIdentidad = Request.Form["ValorDocIdentidad"];
            empleado.Puesto = Request.Form["Puesto"];
            empleado.Departamento = Request.Form["Departamento"];

            Console.WriteLine(empleado.TipoDocIdentidad);

            try
            {
                String connectionString = "Data Source=pruebajose2312.database.windows.net;Initial Catalog=prueba2312;Persist Security Info=True;User ID=adminjose;Password=Bases1234";
                String spNombre = "dbo.InsertarEmpleado";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();

                    if (empleado.Nombre.Length == 0 || empleado.ValorDocIdentidad.Length == 0)
                    {
                        errorMessage = "Todos los datos son requeridos.";
                        errores();
                        return;
                    }

                    //Comprobar el formato

                    //Comprobar que el nombre solo contenga letras o guines
                    
                    if (!VerificarFormatoNombreApellido(empleado.Nombre))
                    {
                        errorMessage = "El campo Nombre debe contener un nombre y un apellido, separados por espacio en blanco y con sin caracteres especiales";
                        errores();
                        return;
                    }

                    //Comprobar que el precio solo contenga numeros o comas
                    if (!empleado.ValorDocIdentidad.All(c => (c >= '0' && c <= '9')) || empleado.ValorDocIdentidad.Length != 9)
                    {
                        errorMessage = "El documento identidad solo debe contener 9 valores numéricos";
                        errores();
                        return;
                    }

                    nombreUsuario = generarUsuarioEmpleado(empleado.Nombre, empleado.ValorDocIdentidad);
                    clave = generarClaveAleatoria(caracteresDisponibles);

                

                    using (SqlCommand command = new SqlCommand(spNombre, connection))
                    {
                        Console.WriteLine("Prueba insertar");
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@inNombre", empleado.Nombre);
                        command.Parameters.AddWithValue("@inTipoDocIdentidad", empleado.TipoDocIdentidad);
                        command.Parameters.AddWithValue("@inValorDocIdentidad", empleado.ValorDocIdentidad);
                        command.Parameters.AddWithValue("@inPuesto", empleado.Puesto);
                        command.Parameters.AddWithValue("@inDepartamento", empleado.Departamento);
                        command.Parameters.AddWithValue("@inUsuarioEmpleado", nombreUsuario);
                        command.Parameters.AddWithValue("@inClaveEmpleado", clave);
                        command.Parameters.AddWithValue("@inUsuario", Global.sesion);
                        command.Parameters.AddWithValue("@inIP", Global.IP);

                        SqlParameter resultCodeParam = new SqlParameter("@outResultCode", SqlDbType.Int);
                        resultCodeParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(resultCodeParam);

                        command.ExecuteNonQuery();

                        int resultCode = (int)command.Parameters["@outResultCode"].Value;
                        Console.WriteLine("Out result: " + resultCode);
                        if (resultCode == 50001) //codigo generado en el SP que dice si ya un nombre del articulo existe o no
                        {
                            errorMessage = "El valor documento identidad ingresado ya existe";
                            errores();
                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            empleado.Nombre = "";
            empleado.TipoDocIdentidad = "";
            empleado.ValorDocIdentidad = "";
            empleado.Puesto = "";
            empleado.Departamento = "";
            Response.Redirect("/Exito");
        }

        static bool VerificarFormatoNombreApellido(string input)
        {
            string patron = @"^[a-zA-Z]+(?: [a-zA-Z]+)*$";

            // Utilizar la clase Regex para comprobar si el input coincide con el patrón
            bool coincideConPatron = Regex.IsMatch(input.Trim(), patron);

            // Verificar si hay exactamente un espacio en el input
            bool tieneUnEspacio = input.Trim().Split(' ').Length == 2;

            return coincideConPatron && tieneUnEspacio;
        }

        public void errores()
        {
            try
            {
                String SPNombre = "dbo.BuscarEmpleadoPorId";
                String connectionString = "Data Source=pruebajose2312.database.windows.net;Initial Catalog=prueba2312;Persist Security Info=True;User ID=adminjose;Password=Bases1234";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();                                      //Se abre la coneccion con la BD.
                    using (SqlCommand command = new SqlCommand(SPNombre, connection))
                    {
                        //Variables para obtener el DataSet mandados de la BD.
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        DataTable table = new DataTable();

                        command.CommandType = CommandType.StoredProcedure;  //Indicar que el comando sera un SP.
                        command.Parameters.AddWithValue("@inIdEmpleado", 1);
                        command.Parameters.AddWithValue("@inUsuario", Global.sesion);
                        command.Parameters.AddWithValue("@inIP", Global.IP);

                        //Codigo para que detecte el output del SP.
                        SqlParameter resultCodeParam = new SqlParameter("@outResultCode", SqlDbType.Int);
                        resultCodeParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(resultCodeParam);
                        //command.ExecuteNonQuery();


                        //Porceso de obtener el DataSet.
                        adapter.SelectCommand = command;
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        //Si el output de errores por DataSet es 0 (No hay problemas).
                        if (dataSet.Tables[0].Rows[0][0].ToString().Equals("0"))
                        {
                            foreach (DataRow row in dataSet.Tables[2].Rows) //Recorra cada fila de la tabla con los datos y estraigala en el tipo ClienteInfo.
                            {
                                String tipoDocIdentidad = "" + row[0];
                                listaTipoDocIdentidad.Add(tipoDocIdentidad);             //A?adir cada fila al array para su visualizacion.
                            }

                            foreach (DataRow row in dataSet.Tables[3].Rows) //Recorra cada fila de la tabla con los datos y estraigala en el tipo ClienteInfo.
                            {
                                String puesto = "" + row[0];
                                listaPuesto.Add(puesto);             //A?adir cada fila al array para su visualizacion.
                            }

                            foreach (DataRow row in dataSet.Tables[4].Rows) //Recorra cada fila de la tabla con los datos y estraigala en el tipo ClienteInfo.
                            {
                                String departamento = "" + row[0];
                                listaDepartamento.Add(departamento);             //A?adir cada fila al array para su visualizacion.
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

        public static String generarClaveAleatoria(String[] disponibles)
        {
            String clave = "";
            Random rand = new Random();
            for (int i = 0; i < 6; i++)
            {
                int idTipoCaracter = rand.Next(0, 4);
                String tipoCaracter = disponibles[idTipoCaracter];
                int idCaracter = rand.Next(0, tipoCaracter.Length);
                char caracter = tipoCaracter[idCaracter];
                clave += caracter;
            }
            Console.WriteLine(clave);
            return clave;
        }

        public static String quitarTildes(String texto)
        {
            StringBuilder textoNormalizado = new StringBuilder(texto.Length);

            foreach (char c in texto)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    textoNormalizado.Append(c);
                }
            }
            return textoNormalizado.ToString();
        }

        public static String generarUsuarioEmpleado(String nombreCompleto, String valorDoc)
        {
            String[] separacion = nombreCompleto.Split(' ');
            String nombre = separacion[0].Normalize(NormalizationForm.FormD);
            String apellido = separacion[1].Normalize(NormalizationForm.FormD);
            String nombreNormalizado = quitarTildes(nombre);
            String apellidoNormalizado = quitarTildes(apellido);

            if (nombreNormalizado.Length < 2)
            {
                nombreNormalizado += "ab";
            }

            if (apellidoNormalizado.Length < 4)
            {
                apellidoNormalizado += "abcd";
            }


            nombreNormalizado = nombreNormalizado.Substring(0, 2);
            apellidoNormalizado = apellidoNormalizado.Substring(0, 4).ToLowerInvariant();
            valorDoc = valorDoc.Substring(valorDoc.Length - 4);
            String nombreUsuario = nombreNormalizado + apellidoNormalizado + valorDoc;

            Console.WriteLine("Nombre: " + nombreNormalizado + "\nApellido: " + apellidoNormalizado + "\nValor documento: " + valorDoc);
            Console.WriteLine("Nombre usuario: " + nombreUsuario);

            return nombreUsuario;
        }

    }
}